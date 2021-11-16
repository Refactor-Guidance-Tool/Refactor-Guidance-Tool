using System.Collections;
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

	public void CreateDatabase(string projectDirectory, string language) {
		this.EnsureDatabaseDirectoryExist();

		var databaseName = CreateUniqueDatabaseName(projectDirectory);
		var databasePath = $"{this._databaseOutputDirectory}/{databaseName}";

		RemoveOldDatabase(databasePath);

		var arguments = $"database create --language={language} -s \"{projectDirectory}\" --overwrite {databaseName}";

		var cmd = new Process();
		cmd.StartInfo.FileName = "codeql";
		cmd.StartInfo.WorkingDirectory = this._databaseOutputDirectory;
		cmd.StartInfo.Arguments = arguments;
		cmd.StartInfo.RedirectStandardInput = true;
		cmd.StartInfo.RedirectStandardOutput = true;
		cmd.StartInfo.CreateNoWindow = false;
		cmd.StartInfo.UseShellExecute = false;
		cmd.Start();

		// var output = cmd.StandardOutput.ReadToEnd();
		cmd.StandardOutput.ReadToEnd();

		cmd.WaitForExit();
	}

	// ReSharper disable once ClassNeverInstantiated.Global
	public record DetectorResult {
		[Index(0)] public string DetectorName { get; set; }
		[Index(1)] public string DetectorDescription { get; set; }
		[Index(2)] public string DetectorType { get; set; }
		[Index(3)] public string Message { get; set; }
		[Index(4)] public string Source { get; set; }
		[Index(5)] public string StartLine { get; set; }
		[Index(6)] public string StartChar { get; set; }
		[Index(7)] public string EndLine { get; set; }
		[Index(8)] public string EndChar { get; set; }
	}

	public IEnumerable<DetectorResult> DetectHazardsRemoveClass(string databasePath, string language, string className) {
		var baseDetectorsDirectory = $"{this._detectorsDirectory}/{language}/Base/RC";
		var concreteDetectorsDirectory = $"{this._detectorsDirectory}/{language}/Concrete/RC";

		if (!Directory.Exists(concreteDetectorsDirectory))
			Directory.CreateDirectory(concreteDetectorsDirectory);

		foreach (var baseDetectorPath in Directory.EnumerateFiles(baseDetectorsDirectory)) {
			var baseDetectorSourceCode = File.ReadAllText(baseDetectorPath);
			var concreteDetectorSourceCode = baseDetectorSourceCode.Replace("$CLASS_TO_BE_REMOVED", className);

			var detectorFileName = Path.GetFileName(baseDetectorPath);
			var concreteDetectorPath = $"{concreteDetectorsDirectory}/{detectorFileName}";

			File.WriteAllText(concreteDetectorPath, concreteDetectorSourceCode);
		}

		var arguments =
			$"database analyze --format=csv --output=removeClass.csv --rerun {databasePath} {concreteDetectorsDirectory}";

		var cmd = new Process();
		cmd.StartInfo.FileName = "codeql";
		cmd.StartInfo.WorkingDirectory = this._resultsDirectory;
		cmd.StartInfo.Arguments = arguments;
		cmd.StartInfo.RedirectStandardInput = true;
		cmd.StartInfo.RedirectStandardOutput = true;
		cmd.StartInfo.CreateNoWindow = false;
		cmd.StartInfo.UseShellExecute = false;
		cmd.Start();

		// var output = cmd.StandardOutput.ReadToEnd();
		cmd.StandardOutput.ReadToEnd();

		cmd.WaitForExit();

		var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
			HasHeaderRecord = false,
		};
		using var reader = new StreamReader($"{this._resultsDirectory}/removeClass.csv");
		using var csv = new CsvReader(reader, config);

		var detectorResults = csv.GetRecords<DetectorResult>();

		return detectorResults.ToList();
	}

	public int CleanDatabaseDirectory() {
		if (!Directory.Exists(this._databaseOutputDirectory))
			return 0;

		var databaseCount = Directory.GetDirectories(this._databaseOutputDirectory).Length;

		Directory.Delete(this._databaseOutputDirectory, true);

		return databaseCount;
	}

	private void EnsureDatabaseDirectoryExist() {
		if (Directory.Exists(this._databaseOutputDirectory))
			return;

		Directory.CreateDirectory(this._databaseOutputDirectory);
	}

	private static void RemoveOldDatabase(string databasePath) {
		if (!Directory.Exists(databasePath))
			return;

		Directory.Delete(databasePath, true);
	}

	private static string CreateUniqueDatabaseName(string projectDirectory) {
		var databaseName = projectDirectory.Replace(@"\", "_").Replace(@"/", "_");
		return databaseName;
	}
}