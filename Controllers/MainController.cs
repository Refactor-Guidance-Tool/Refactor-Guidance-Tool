using Microsoft.AspNetCore.Mvc;

namespace RefactorGuidanceTool.Controllers; 

[ApiController]
[Route("Main")]
public class MainController : ControllerBase{
	private readonly ILogger<MainController> _logger;

	public MainController(ILogger<MainController> logger) {
		this._logger = logger;
	}
	
	[HttpGet]
	[Route("CleanDatabaseDirectory")]
	public void GetPossibleRefactorings() {
		CodeQlBroker.CleanDatabaseDirectory();
	}
}