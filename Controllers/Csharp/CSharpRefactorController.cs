using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool.Controllers.CSharp;

[ApiController]
[Route("CSharp/Refactor")]
public class CSharpRefactorController : ControllerBase {
	private readonly ILogger<CSharpRefactorController> _logger;

	public CSharpRefactorController(ILogger<CSharpRefactorController> logger) {
		this._logger = logger;
	}

	[HttpGet]
	[Route("RemoveClassAdvice")]
	public Advice GetAdviceForRemoveClassRefactoring() {
		return new Advice();
	}
}