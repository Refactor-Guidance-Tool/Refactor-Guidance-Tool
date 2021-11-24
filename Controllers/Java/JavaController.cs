using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool.Controllers.Java;

[ApiController]
[Route("Java")]
public class JavaController : ControllerBase {
	private readonly ILogger<JavaController> _logger;

	public JavaController(ILogger<JavaController> logger) {
		this._logger = logger;
	}

	[HttpGet]
	[Route("PossibleRefactorings")]
	public IEnumerable<Refactoring> GetPossibleRefactorings(string databaseUuid) {
		return Array.Empty<Refactoring>();
	}
}