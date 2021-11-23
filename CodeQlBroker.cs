using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace RefactorGuidanceTool;

public class CodeQlBroker {
	private readonly string _databaseOutputDirectory;
	private readonly string _resultsDirectory;
	private readonly string _detectorsDirectory;

	private readonly DatabaseDataStore _databaseDataStore;

	public CodeQlBroker(string outputDirectory, string detectorsDirectory) {
		this._databaseOutputDirectory = $"{outputDirectory}/Databases";
		this._resultsDirectory = $"{outputDirectory}/Results";
		this._detectorsDirectory = detectorsDirectory;

		this._databaseDataStore = new DatabaseDataStore(this._databaseOutputDirectory);
	}

	public CodeQlBroker(string databaseOutputDirectory, string resultsDirectory, string detectorsDirectory) {
		this._databaseOutputDirectory = databaseOutputDirectory;
		this._resultsDirectory = resultsDirectory;
		this._detectorsDirectory = detectorsDirectory;

		this._databaseDataStore = new DatabaseDataStore(this._databaseOutputDirectory);
	}

	public void CreateDatabase(string projectDirectory, string language) {
		Utils.EnsureDirectoryExists(this._databaseOutputDirectory);

		var databaseName = CreateUniqueDatabaseName(projectDirectory);
		var databasePath = $"{this._databaseOutputDirectory}/{databaseName}";

		this.RemoveDatabase(databasePath);

		var arguments = $"database create --language={language} -s \"{projectDirectory}\" --overwrite {databaseName}";
		this.RunCodeQl(arguments, this._databaseOutputDirectory);

		this._databaseDataStore.Insert(databasePath);
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

	private string RunCodeQl(string arguments, string workingDirectory) {
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

		return output;
	}

	public IEnumerable<DetectorResult> DetectHazardsRemoveClass(string databasePath, string language, string className) {
		var baseDetectorsDirectory = $"{this._detectorsDirectory}/{language}/Base/RC";
		var concreteDetectorsDirectory = $"{this._detectorsDirectory}/{language}/Concrete/RC";

		Utils.EnsureDirectoryExists(concreteDetectorsDirectory);

		foreach (var baseDetectorPath in Directory.EnumerateFiles(baseDetectorsDirectory)) {
			var baseDetectorSourceCode = File.ReadAllText(baseDetectorPath);
			var concreteDetectorSourceCode = baseDetectorSourceCode.Replace("$CLASS_TO_BE_REMOVED", className);

			var detectorFileName = Path.GetFileName(baseDetectorPath);
			var concreteDetectorPath = $"{concreteDetectorsDirectory}/{detectorFileName}";

			File.WriteAllText(concreteDetectorPath, concreteDetectorSourceCode);
		}

		var arguments =
			$"database analyze --format=csv --output=removeClass.csv --rerun {databasePath} {concreteDetectorsDirectory}";
		this.RunCodeQl(arguments, this._resultsDirectory);

		var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
			HasHeaderRecord = false,
		};
		using var reader = new StreamReader($"{this._resultsDirectory}/removeClass.csv");
		using var csv = new CsvReader(reader, config);

		var detectorResults = csv.GetRecords<DetectorResult>();

		return detectorResults.ToList();
	}

	public int RemoveAllDatabases() {
		return this._databaseDataStore.RemoveAll();
	}

	private void RemoveDatabase(string databasePath) {
		this._databaseDataStore.Delete(databasePath);
	}

	private void RemoveDatabase(DatabaseDataStore.DatabaseData databaseData) {
		this._databaseDataStore.Delete(databaseData);
	}

	private static string CreateUniqueDatabaseName(string projectDirectory) {
		var databaseName = projectDirectory.Replace(@"\", "_").Replace(@"/", "_").Replace(@":", "");
		return databaseName;
	}
}