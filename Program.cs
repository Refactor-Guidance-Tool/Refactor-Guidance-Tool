using RefactorGuidanceTool;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

{
	var outputDirectory = args[0];
	var detectorsDirectory = args[1];

	var codeQlBroker = new CodeQlBroker(outputDirectory, detectorsDirectory);

	builder.Services.AddSingleton(codeQlBroker);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWebSockets();

app.MapControllers();

app.Run();