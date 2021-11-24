namespace RefactorGuidanceTool;

internal static class Utils {
	public static void EnsureDirectoryExists(string directory) {
		if (Directory.Exists(directory))
			return;

		Directory.CreateDirectory(directory);
	}

	public static void SafeDeleteDirectory(string directory, bool recursive = false) {
		if (!Directory.Exists(directory))
			return;
		
		Directory.Delete(directory, recursive);
	}
}