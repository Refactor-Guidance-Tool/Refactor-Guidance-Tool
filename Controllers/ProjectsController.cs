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

	public ProjectsController(ILogger<ProjectsController> logger, Dictionary<ProjectLanguage, RefactoringProvider> refactoringProviders, ProjectFactory projectFactory,
		ProjectStore projectStore) {
		this._logger = logger;

		this._refactoringProviders = refactoringProviders;
		this._projectFactory = projectFactory;
		this._projectStore = projectStore;
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
			this._projectStore.Delete(project);
			project.Delete();

			return this.Ok();
		}, projectNotFound => this.NotFound());
	}
	
	private record GetCodeElementsResponse(IReadOnlyList<CodeElement> CodeElements) {
		[Required]
		public IReadOnlyList<CodeElement> CodeElements { get; } = CodeElements;
	}
	
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type=typeof(GetCodeElementsResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{projectId}/codeElements")]
	public IActionResult GetCodeElements(string projectId) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);

		return projectResult.Match<IActionResult>(project => {
			var codeElements = project.GetCodeElements();
			return this.Ok(new GetCodeElementsResponse(codeElements));
		}, projectNotFound => this.NotFound());
	}
}