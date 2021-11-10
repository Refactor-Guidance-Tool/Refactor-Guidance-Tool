using System.Diagnostics.CodeAnalysis;
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

	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
	public record GetPossibleRefactoringsResponse {
		public int DatabasesDeletedCount { get; set; } 
	}
	
	[HttpGet]
	[Route("CleanDatabaseDirectory")]
	public GetPossibleRefactoringsResponse GetPossibleRefactorings() {
		var databasesDeletedCount = this._codeQlBroker.CleanDatabaseDirectory();

		var response = new GetPossibleRefactoringsResponse() {
			DatabasesDeletedCount = databasesDeletedCount
		};
		
		return response;
	}
}