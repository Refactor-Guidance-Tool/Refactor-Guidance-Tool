using Microsoft.AspNetCore.Mvc;

namespace RefactorGuidanceTool.Controllers; 

[ApiController]
[Route("Main")]
public class MainController : ControllerBase{
	private readonly ILogger<MainController> _logger;
	private readonly CodeQlBroker _codeQlBroker;

	public MainController(ILogger<MainController> logger, CodeQlBroker codeQlBroker) {
		this._logger = logger;
		this._codeQlBroker = codeQlBroker;
	}
	
	[HttpGet]
	[Route("CleanDatabaseDirectory")]
	public void GetPossibleRefactorings() {
		this._codeQlBroker.CleanDatabaseDirectory();
	}
}