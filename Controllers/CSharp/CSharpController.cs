using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool.Controllers.CSharp;

[ApiController]
[Route("CSharp")]
public class CSharpController : ControllerBase {
	private readonly ILogger<CSharpController> _logger;
	private readonly CodeQlBroker _codeQlBroker;

	public CSharpController(ILogger<CSharpController> logger, CodeQlBroker codeQlBroker) {
		this._logger = logger;
		this._codeQlBroker = codeQlBroker;
	}

	[HttpGet]
	[Route("PossibleRefactorings")]
	public IEnumerable<Refactoring> GetPossibleRefactorings(int id) {
		return Array.Empty<Refactoring>();
	}

	[HttpGet]
	[Route("CreateDatabase")]
	public void CreateDatabase(string projectDirectory) {
		this._codeQlBroker.CreateDatabase(projectDirectory, "csharp");
	}
}