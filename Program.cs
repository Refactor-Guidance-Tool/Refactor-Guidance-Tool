using RefactorGuidanceTool;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

{
	var databaseOutputDirectory = args[0];
	var codeQlBroker = new CodeQlBroker(databaseOutputDirectory);

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