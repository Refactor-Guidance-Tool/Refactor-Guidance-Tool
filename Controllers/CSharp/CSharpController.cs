using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
	public IEnumerable<Refactoring> GetPossibleRefactorings(string databaseUuid) {
		return Array.Empty<Refactoring>();
	}
	
	public record CreateDatabaseResponse {
		public string DatabaseUuid { get; set; } 
	}

	[HttpGet]
	[Route("CreateDatabase")]
	public CreateDatabaseResponse CreateDatabase(string projectDirectory) {
		var databaseUuid = this._codeQlBroker.CreateDatabase(projectDirectory, "csharp");

		var response = new CreateDatabaseResponse() {
			DatabaseUuid = databaseUuid
		};

		return response;
	}
}