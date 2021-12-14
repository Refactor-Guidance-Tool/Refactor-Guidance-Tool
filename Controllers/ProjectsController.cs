using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool.Controllers;

[ApiController]
[Route("Projects")]
public class ProjectsController : ControllerBase {
	private readonly ILogger<ProjectsController> _logger;

	private readonly Dictionary<ProjectLanguage, RefactoringProvider> _refactoringProviders;
	private readonly ProjectFactory _projectFactory;
	private readonly ProjectStore _projectStore;

	private readonly CodeElementCache _codeElementCache;

	public ProjectsController(ILogger<ProjectsController> logger, Dictionary<ProjectLanguage, RefactoringProvider> refactoringProviders, ProjectFactory projectFactory,
		ProjectStore projectStore) {
		this._logger = logger;

		this._refactoringProviders = refactoringProviders;
		this._projectFactory = projectFactory;
		this._projectStore = projectStore;

		this._codeElementCache = new CodeElementCache();
	}

	private record RegisterProjectResponse(string ProjectUuid) {
		[Required]
		public string ProjectUuid { get; } = ProjectUuid;
	}

	[HttpPost]
	[Route("")]
	public IActionResult PostProject([Required]ProjectLanguage projectLanguage, [Required]string projectPath) {
		var project = this._projectFactory.CreateProject(projectLanguage, projectPath);
		project.UpdateDatabase();
		
		this._projectStore.Insert(project);

		return this.Ok(new RegisterProjectResponse(project.Uuid.ToString()));
	}

	public record GetProjectResponse(string Path, ProjectLanguage Language) {
		[Required]
		public string Path { get; } = Path;
		public ProjectLanguage Language { get; } = Language;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProjectResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{projectId}")]
	public IActionResult GetProject(string projectId) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);

		return projectResult.Match<IActionResult>(
			project => this.Ok(new GetProjectResponse(project.ProjectPath, project.ProjectLanguage)),
			projectNotFound => this.NotFound());
	}
	
	public record GetProjectsResponse(IReadOnlyList<string> ProjectUuids) {
		[Required]
		public IReadOnlyList<string> ProjectUuids { get; } = ProjectUuids;
	}
		
	[HttpGet]
	[Route("")]
	public GetProjectsResponse GetProjects() {
		var projectUuids = this._projectStore.GetProjects()
			.Select(p => p.Uuid.ToString())
			.ToList();

		return new GetProjectsResponse(projectUuids);
	}

	private record GetAllRefactoringsResponse(IReadOnlyList<RefactoringDTO> Refactorings) {
		[Required]
		public IReadOnlyList<RefactoringDTO> Refactorings { get; } = Refactorings;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllRefactoringsResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{projectId}/refactorings")]
	public IActionResult GetRefactorings(string projectId) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);

		return projectResult.Match<IActionResult>(
			project => {
				var refactoringProvider = this._refactoringProviders[project.ProjectLanguage];
				var refactorings = refactoringProvider.GetRefactorings();

				return this.Ok(new GetAllRefactoringsResponse(refactorings));
			},
			projectNotFound => this.NotFound());
	}

	[HttpDelete]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{projectId}")]
	public IActionResult DeleteProject(string projectId) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);

		return projectResult.Match<IActionResult>(project => {
			this._codeElementCache.Invalidate(project.Uuid);
			
			this._projectStore.Delete(project);
			project.Delete();

			return this.Ok();
		}, projectNotFound => this.NotFound());
	}
	
	private record GetCodeElementsResponse(IReadOnlyList<CodeElement> CodeElements, int Remaining) {
		[Required]
		public IReadOnlyList<CodeElement> CodeElements { get; } = CodeElements;

		[Required] public int Remaining { get; } = Remaining;
	}
	
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type=typeof(GetCodeElementsResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{projectId}/codeElements")]
	public IActionResult GetCodeElements(string projectId, int offset, int limit, string nameFilter, string typeFilter) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);

		var allowList = new HashSet<string>(typeFilter.Split(','));

		nameFilter = nameFilter[1..];
		
		return projectResult.Match<IActionResult>(project => {
			if (!this._codeElementCache.TryGet(project.Uuid, out var codeElements)) {
				codeElements = project.GetCodeElements();
				this._codeElementCache.Register(project.Uuid, codeElements);
			}

			var filteredCodeElements = codeElements
				.Where(element => allowList.Contains(element.Type))
				.Where(element => nameFilter == string.Empty || element.Name.ToLower().Contains(nameFilter.ToLower())).ToList();
			var selectedCodeElements = filteredCodeElements.Skip(offset).Take(limit).ToList();
			return this.Ok(new GetCodeElementsResponse(selectedCodeElements, Math.Max(0, filteredCodeElements.Count - offset - limit)));
		}, projectNotFound => this.NotFound());
	}
	
	private record GetCodeElementTypesResponse(IReadOnlyList<string> CodeElementTypes) {
		[Required]
		public IReadOnlyList<string> CodeElementTypes { get; } = CodeElementTypes;
	}
	
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type=typeof(GetCodeElementTypesResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{projectId}/codeElementTypes")]
	public IActionResult GetCodeElementTypes(string projectId) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);

		return projectResult.Match<IActionResult>(project => {
			var types = new List<string>() {
				"Class",
				"Interface"
			};
			return this.Ok(new GetCodeElementTypesResponse(types));
		}, projectNotFound => this.NotFound());
	}
	
	[HttpPatch]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{projectId}")]
	public IActionResult PatchProject(string projectId) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);

		return projectResult.Match<IActionResult>(project => {
			project.UpdateDatabase();
			this._codeElementCache.Invalidate(project.Uuid);
			
			return this.Ok();
		}, projectNotFound => this.NotFound());
	}
}