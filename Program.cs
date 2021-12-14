using RefactorGuidanceTool;
using RefactorGuidanceTool.Models;
using RefactorGuidanceTool.Models.CSharp;
using RefactorGuidanceTool.Models.Java;

var builder = WebApplication.CreateBuilder(args);

string _policyName = "CorsPolicy";

builder.Services.AddCors(opt =>
{
	opt.AddPolicy(name: _policyName, builder =>
	{
		builder.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

builder.Services.AddControllers();

{
	var outputDirectory = args[0];
	var detectorsDirectory = args[1];

	var codeQlBroker = new CodeQlBroker(outputDirectory, detectorsDirectory);
	builder.Services.AddSingleton(codeQlBroker);

	var refactoringProviders = new Dictionary<ProjectLanguage, RefactoringProvider>() {
		{ProjectLanguage.CSharp, new CSharpRefactoringProvider(codeQlBroker)},
		{ProjectLanguage.Java, new JavaRefactoringProvider(codeQlBroker)},
	};
	builder.Services.AddSingleton(refactoringProviders);
	
	var projectFactory = new ProjectFactory(codeQlBroker, refactoringProviders);
	builder.Services.AddSingleton(projectFactory);

	var projectStore = new ProjectStore(projectFactory, outputDirectory);
	builder.Services.AddSingleton(projectStore);

	var codeElementCache = new CodeElementCache();
	builder.Services.AddSingleton(codeElementCache);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseCors(_policyName);

app.UseAuthorization();

app.UseWebSockets();

app.MapControllers();

app.Run();