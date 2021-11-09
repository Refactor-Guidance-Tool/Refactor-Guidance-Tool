using Microsoft.AspNetCore.Mvc;
using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool.Controllers;

[ApiController]
[Route("[controller]")]
public class RefactorController : ControllerBase {
	private readonly ILogger<RefactorController> _logger;

	public RefactorController(ILogger<RefactorController> logger) {
		this._logger = logger;
	}

	[HttpGet]
	[Route("AdviceForRemoveClassRefactoring")]
	public Advice GetAdviceForRemoveClassRefactoring() {
		return new Advice();
	}

	[HttpGet]
	[Route("PossibleRefactorings")]
	public IEnumerable<Refactoring> GetPossibleRefactorings(int id) {
		return Array.Empty<Refactoring>();
	}
}