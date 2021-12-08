using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;
using RefactorGuidanceTool.Models.Settings;

namespace RefactorGuidanceTool.Controllers; 

[ApiController]
[Route("Refactorings")]
public class RefactoringsController : ControllerBase {
	private readonly ILogger<ProjectsController> _logger;

	private readonly ProjectFactory _projectFactory;
	private readonly ProjectStore _projectStore;

	public RefactoringsController(ILogger<ProjectsController> logger, ProjectFactory projectFactory,
		ProjectStore projectStore) {
		this._logger = logger;

		this._projectFactory = projectFactory;
		this._projectStore = projectStore;
	}
	
	private record GetHazardsResponse(IReadOnlyList<Hazard> Hazards) {
		[Required]
		public IReadOnlyList<Hazard> Hazards { get; } = Hazards;
	}
	
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetHazardsResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{refactoringId}/hazards")]
	public IActionResult GetHazards(string refactoringId, [Required]string projectId, Dictionary<string, string> settings) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);
		
		return projectResult.Match<IActionResult>(project => {
			var refactoring = project.GetRefactoring(refactoringId);
			var hazards = refactoring.GetHazards(project, settings);
			
			return this.Ok(hazards);
		}, projectNotFound => this.NotFound());
		
		return null;
	}
	
	private record GetSettingsResponse(IReadOnlyList<Setting> Settings) {
		[Required]
		public IReadOnlyList<Setting> Settings { get; } = Settings;
	}
	
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetSettingsResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{refactoringId}/settings")]
	public IActionResult GetSettings(string refactoringId, [Required]string projectId) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);
		
		return projectResult.Match<IActionResult>(project => {
			var refactoring = project.GetRefactoring(refactoringId);
			var settings = refactoring.GetSettings();
			
			return this.Ok(settings);
		}, projectNotFound => this.NotFound());
		
		return null;
	}
}