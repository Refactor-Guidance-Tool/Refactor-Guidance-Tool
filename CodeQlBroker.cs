using System.Diagnostics;

namespace RefactorGuidanceTool;

public class CodeQlBroker {
	private readonly string _databaseOutputDirectory;

	public CodeQlBroker(string databaseOutputDirectory) {
		this._databaseOutputDirectory = databaseOutputDirectory;
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

	public void CleanDatabaseDirectory() {
		if (Directory.Exists(this._databaseOutputDirectory))
			Directory.Delete(this._databaseOutputDirectory, true);
	}
	private void EnsureDatabaseDirectoryExist() {
		if (!Directory.Exists(this._databaseOutputDirectory))
			Directory.CreateDirectory(this._databaseOutputDirectory);
	}

	private static void RemoveOldDatabase(string databasePath) {
		if (Directory.Exists(databasePath))
			Directory.Delete(databasePath, true);
	}

	private static string CreateUniqueDatabaseName(string projectDirectory) {
		var databaseName = projectDirectory.Replace(@"\", "_").Replace(@"/", "_");
		return databaseName;
	}
}