using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using RefactorGuidanceTool.Models;

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
		[Index(0)] public string DetectorName { get; set; }
		[Index(1)] public string DetectorDescription { get; set; }
		[Index(2)] public string DetectorType { get; set; }
		[Index(3)] public string Message { get; set; }
		[Index(4)] public string Source { get; set; }
		[Index(5)] public int StartLine { get; set; }
		[Index(6)] public int StartChar { get; set; }
		[Index(7)] public int EndLine { get; set; }
		[Index(8)] public int EndChar { get; set; }
	}
	
	public IReadOnlyList<DetectorResult> RunDetectors(Guid uuid, ProjectLanguage language, string detectorsName, Func<string, string> argumentFiller)
	{
		var databaseName = uuid.ToString("D");
		var databasePath = $"{this._databaseOutputDirectory}/{databaseName}";
		
		var languageParam = language switch {
			ProjectLanguage.Java => "java",
			ProjectLanguage.CSharp => "csharp",
			_ => throw new Exception()
		};
		
		var baseDetectorsDirectory = $"{this._detectorsDirectory}/{languageParam}/Base/{detectorsName}";
		var concreteDetectorsDirectory = $"{this._detectorsDirectory}/{languageParam}/Concrete/{detectorsName}";
		
		Utils.EnsureDirectoryExists(concreteDetectorsDirectory);
		Utils.EnsureDirectoryExists(this._resultsDirectory);
		
		foreach (var baseDetectorPath in Directory.EnumerateFiles(baseDetectorsDirectory)) {
			var baseDetectorSourceCode = File.ReadAllText(baseDetectorPath);
			var concreteDetectorSourceCode = argumentFiller(baseDetectorSourceCode);
			
			var detectorFileName = Path.GetFileName(baseDetectorPath);
			var concreteDetectorPath = $"{concreteDetectorsDirectory}/{detectorFileName}";

			File.WriteAllText(concreteDetectorPath, concreteDetectorSourceCode);
		}
		
		var arguments = $"database analyze --format=csv --output=removeClass.csv --rerun {databasePath} {concreteDetectorsDirectory}";
		RunCodeQl(arguments, this._resultsDirectory);

		var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
			HasHeaderRecord = false,
		};
		using var reader = new StreamReader($"{this._resultsDirectory}/removeClass.csv");
		using var csv = new CsvReader(reader, config);

		var detectorResults = csv.GetRecords<DetectorResult>();
		
		return detectorResults.ToList();
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

		var output = cmd.StandardOutput.ReadToEnd();

		cmd.WaitForExit();

		Console.WriteLine(output);
		
		return output;
	}

	public void DeleteDatabase(Guid uuid) {
		var databaseName = uuid.ToString("D");
		var databasePath = $"{this._databaseOutputDirectory}/{databaseName}";

		Utils.SafeDeleteDirectory(databasePath);
	}

	public IReadOnlyList<CodeElement> GetCodeElements(Guid uuid, ProjectLanguage language) {
		var databaseName = uuid.ToString("D");
		var databasePath = $"{this._databaseOutputDirectory}/{databaseName}";
		
		var languageParam = language switch {
			ProjectLanguage.Java => "java",
			ProjectLanguage.CSharp => "csharp",
			_ => throw new Exception()
		};
		
		Utils.EnsureDirectoryExists(this._resultsDirectory);
		
		var astDetectorsDirectory = $"{this._detectorsDirectory}/{languageParam}/AST";
		
		var arguments = $"database analyze --format=csv --output=ast.csv --rerun {databasePath} {astDetectorsDirectory}";
		RunCodeQl(arguments, this._resultsDirectory);

		var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
			HasHeaderRecord = false,
		};
		using var reader = new StreamReader($"{this._resultsDirectory}/ast.csv");
		using var csv = new CsvReader(reader, config);

		var detectorResults = csv.GetRecords<DetectorResult>();

		var codeElements = detectorResults.Select(result => new CodeElement(
			result.Message,
			new Position(result.StartLine, result.StartChar, result.EndLine, result.EndChar),
			result.DetectorDescription,
			result.Source
		)).ToList();


		return codeElements;
	}
}