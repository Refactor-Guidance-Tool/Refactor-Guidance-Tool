using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool.Controllers;

[ApiController]
[Route("Project")]
public class ProjectController : ControllerBase {
	private readonly ILogger<ProjectController> _logger;

	private readonly ProjectFactory _projectFactory;
	private readonly ProjectStore _projectStore;

	public ProjectController(ILogger<ProjectController> logger, ProjectFactory projectFactory,
		ProjectStore projectStore) {
		this._logger = logger;

		this._projectFactory = projectFactory;
		this._projectStore = projectStore;
	}

	private record RegisterProjectResponse(string ProjectUuid) {
		[Required]
		public string ProjectUuid { get; } = ProjectUuid;
	}

	[HttpPost]
	[Route("Project")]
	public IActionResult PostProject(ProjectLanguage projectLanguage, string projectPath) {
		var project = this._projectFactory.CreateProject(projectLanguage, projectPath);
		this._projectStore.Insert(project);

		return this.Ok(new RegisterProjectResponse(project.Uuid.ToString()));
	}

	public record GetProjectsResponse(IReadOnlyList<string> ProjectUuids) {
		[Required]
		public IReadOnlyList<string> ProjectUuids { get; } = ProjectUuids;
	}

	[HttpGet]
	[Route("Projects")]
	public GetProjectsResponse GetProjects() {
		var projectUuids = this._projectStore.GetProjects()
			.Select(p => p.Uuid.ToString())
			.ToList();

		return new GetProjectsResponse(projectUuids);
	}

	private record GetAllRefactoringsResponse(IReadOnlyList<Refactoring> Refactorings) {
		[Required]
		public IReadOnlyList<Refactoring> Refactorings { get; } = Refactorings;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllRefactoringsResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("AllRefactorings")]
	public IActionResult GetAllRefactorings(string uuid) {
		var projectResult = this._projectStore.GetProjectByUuid(uuid);

		return projectResult.Match<IActionResult>(
			project => {
				var refactorings = project.GetAllRefactorings();

				return this.Ok(new GetAllRefactoringsResponse(refactorings));
			},
			projectNotFound => this.NotFound());
	}

	[HttpDelete]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("Project")]
	public IActionResult DeleteProject(string uuid) {
		var projectResult = this._projectStore.GetProjectByUuid(uuid);

		return projectResult.Match<IActionResult>(project => {
			this._projectStore.Delete(project);
			project.Delete();

			return this.Ok();
		}, projectNotFound => this.NotFound());
	}

	public record RemoveAllDatabasesResponse {
		public int DatabasesDeletedCount { get; set; }
	}
}