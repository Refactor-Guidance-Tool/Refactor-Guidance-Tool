using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;
using RefactorGuidanceTool.Models.Settings;

namespace RefactorGuidanceTool.Controllers;

[ApiController]
[Route("Refactorings")]
public class RefactoringsController : ControllerBase {
	private readonly ILogger<ProjectsController> _logger;

	private readonly Dictionary<ProjectLanguage, RefactoringProvider> _refactoringProviders;
	private readonly ProjectFactory _projectFactory;
	private readonly ProjectStore _projectStore;

	public RefactoringsController(ILogger<ProjectsController> logger,
		Dictionary<ProjectLanguage, RefactoringProvider> refactoringProviders, ProjectFactory projectFactory,
		ProjectStore projectStore) {
		this._logger = logger;

		this._refactoringProviders = refactoringProviders;
		this._projectFactory = projectFactory;
		this._projectStore = projectStore;
	}

	private record GetHazardsResponse(IReadOnlyList<Hazard> Hazards) {
		[Required] public IReadOnlyList<Hazard> Hazards { get; } = Hazards;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetHazardsResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Route("{refactoringId}/hazards")]
	public IActionResult GetHazards(string refactoringId, [Required] string projectId, string settings) {
		var projectResult = this._projectStore.GetProjectByUuid(projectId);

		return projectResult.Match<IActionResult>(project => {
			var convertedSettings = new Dictionary<string, string>();
			settings
				.Split(',')
				.Select(setting => setting.Split('='))
				.ToList()
				.ForEach(setting => convertedSettings.Add(setting[0], setting[1]));

			var refactoringProvider = this._refactoringProviders[project.ProjectLanguage];
			var refactoring = refactoringProvider.GetRefactoring(refactoringId);
			var hazards = refactoring.GetHazards(project, convertedSettings);

			return this.Ok(hazards);
		}, projectNotFound => this.NotFound());

		return null;
	}

	private record GetSettingsResponse(IReadOnlyList<SettingDto> Settings) {
		[Required] public IReadOnlyList<SettingDto> Settings { get; } = Settings;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetSettingsResponse))]
	[Route("{refactoringId}/settings")]
	public IActionResult GetSettings(string refactoringId, [Required] ProjectLanguage projectLanguage) {
		var refactoringProvider = this._refactoringProviders[projectLanguage];
		var refactoring = refactoringProvider.GetRefactoring(refactoringId);
		var settings = refactoring.GetSettings().Select((setting, i) => {
			var dto = new SettingDto();
			setting.FillDTO(dto);

			return dto;
		});

		return this.Ok(settings);
	}
}