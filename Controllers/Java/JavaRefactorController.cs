using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool.Controllers.Java;

[ApiController]
[Route("Java/Refactor")]
public class JavaRefactorController : ControllerBase {
	private readonly ILogger<JavaRefactorController> _logger;

	public JavaRefactorController(ILogger<JavaRefactorController> logger) {
		this._logger = logger;
	}

	[HttpGet]
	[Route("RemoveClassAdvice")]
	public Advice GetAdviceForRemoveClassRefactoring() {
		return new Advice();
	}
}