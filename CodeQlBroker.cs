using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace RefactorGuidanceTool;

public class CodeQlBroker {
	private readonly string _databaseOutputDirectory;
	private readonly string _resultsDirectory;
	private readonly string _detectorsDirectory;

	public CodeQlBroker(string outputDirectory, string detectorsDirectory) {
		this._databaseOutputDirectory = $"{outputDirectory}/Databases";
		this._resultsDirectory = $"{outputDirectory}/Results";
		this._detectorsDirectory = detectorsDirectory;
	}

	public CodeQlBroker(string databaseOutputDirectory, string resultsDirectory, string detectorsDirectory) {
		this._databaseOutputDirectory = databaseOutputDirectory;
		this._resultsDirectory = resultsDirectory;
		this._detectorsDirectory = detectorsDirectory;
	}

	public void CreateDatabase(Guid uuid, string projectDirectory, ProjectLanguage language) {
		Utils.EnsureDirectoryExists(this._databaseOutputDirectory);

		var databaseName = uuid.ToString("D");
		var databasePath = $"{this._databaseOutputDirectory}/{databaseName}";

		Utils.SafeDeleteDirectory(databasePath);

		var languageParam = language switch {
			ProjectLanguage.Java => "java",
			ProjectLanguage.CSharp => "csharp",
			_ => throw new Exception()
		};

		var arguments = $"database create --language={languageParam} -s \"{projectDirectory}\" --overwrite {databaseName}";
		RunCodeQl(arguments, this._databaseOutputDirectory);
	}

	// ReSharper disable once ClassNeverInstantiated.Global
	public record DetectorResult {
		[Index(0)] public string? DetectorName { get; set; }
		[Index(1)] public string? DetectorDescription { get; set; }
		[Index(2)] public string? DetectorType { get; set; }
		[Index(3)] public string? Message { get; set; }
		[Index(4)] public string? Source { get; set; }
		[Index(5)] public string? StartLine { get; set; }
		[Index(6)] public string? StartChar { get; set; }
		[Index(7)] public string? EndLine { get; set; }
		[Index(8)] public string? EndChar { get; set; }
	}

	private static string RunCodeQl(string arguments, string workingDirectory) {
		Utils.EnsureDirectoryExists(workingDirectory);
		
		var cmd = new Process();
		cmd.StartInfo.FileName = "codeql";
		cmd.StartInfo.WorkingDirectory = workingDirectory;
		cmd.StartInfo.Arguments = arguments;
		cmd.StartInfo.RedirectStandardInput = true;
		cmd.StartInfo.RedirectStandardOutput = true;
		cmd.StartInfo.CreateNoWindow = false;
		cmd.StartInfo.UseShellExecute = false;
		cmd.Start();

		//var output = cmd.StandardOutput.ReadToEnd();

		cmd.WaitForExit();
		
		return string.Empty;
	}

	public void DeleteDatabase(Guid uuid) {
		var databaseName = uuid.ToString("D");
		var databasePath = $"{this._databaseOutputDirectory}/{databaseName}";

		Utils.SafeDeleteDirectory(databasePath);
	}
}