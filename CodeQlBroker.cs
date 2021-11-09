using System.Diagnostics;

namespace RefactorGuidanceTool;

public static class CodeQlBroker {
	private const string DatabaseOutputDirectory = "C:/Users/Laptop-Justin/Documents/RefactorGuidanceTool/Databases";

	public static void CreateDatabase(string projectDirectory, string language) {
		EnsureDatabaseDirectoryExist();

		var databaseName = CreateUniqueDatabaseName(projectDirectory);
		var databasePath = $"{DatabaseOutputDirectory}/{databaseName}";

		RemoveOldDatabase(databasePath);
		
		var arguments = $"database create --language={language} -s \"{projectDirectory}\" --overwrite {databaseName}";

		var cmd = new Process();
		cmd.StartInfo.FileName = "codeql";
		cmd.StartInfo.WorkingDirectory = DatabaseOutputDirectory;
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

	public static void CleanDatabaseDirectory() {
		if (Directory.Exists(DatabaseOutputDirectory))
			Directory.Delete(DatabaseOutputDirectory, true);
	}

	private static void RemoveOldDatabase(string databasePath) {
		if (Directory.Exists(databasePath))
			Directory.Delete(databasePath, true);
	}

	private static string CreateUniqueDatabaseName(string projectDirectory) {
		var databaseName = projectDirectory.Replace(@"\", "_").Replace(@"/", "_");
		return databaseName;
	}

	private static void EnsureDatabaseDirectoryExist() {
		if (!Directory.Exists(DatabaseOutputDirectory))
			Directory.CreateDirectory(DatabaseOutputDirectory);
	}
}