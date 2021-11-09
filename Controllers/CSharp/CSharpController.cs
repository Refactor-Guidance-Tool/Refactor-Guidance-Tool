using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool.Controllers.CSharp;

[ApiController]
[Route("CSharp")]
public class CSharpController : ControllerBase {
	private readonly ILogger<CSharpController> _logger;

	public CSharpController(ILogger<CSharpController> logger) {
		this._logger = logger;
	}

	[HttpGet]
	[Route("PossibleRefactorings")]
	public IEnumerable<Refactoring> GetPossibleRefactorings(int id) {
		return Array.Empty<Refactoring>();
	}

	[HttpGet]
	[Route("CreateDatabase")]
	public void CreateDatabase(string projectDirectory) {
		CodeQlBroker.CreateDatabase(projectDirectory, "csharp");
	}
}