using System.Collections;
using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool.Controllers.CSharp;

[ApiController]
[Route("CSharp/Refactor")]
public class CSharpRefactorController : ControllerBase {
	private readonly ILogger<CSharpRefactorController> _logger;
	private readonly CodeQlBroker _codeQlBroker;

	public CSharpRefactorController(ILogger<CSharpRefactorController> logger, CodeQlBroker codeQlBroker) {
		this._logger = logger;
		this._codeQlBroker = codeQlBroker;
	}

	[HttpGet]
	[Route("RemoveClassAdvice")]
	public IEnumerable<CodeQlBroker.DetectorResult> GetAdviceForRemoveClassRefactoring(string databaseUuid, string className) {
		var detectorResults = this._codeQlBroker.DetectHazardsRemoveClass(databaseUuid, "csharp", className);

		return detectorResults;
	}
}