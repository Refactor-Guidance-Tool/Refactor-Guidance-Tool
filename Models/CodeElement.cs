namespace RefactorGuidanceTool.Models; 

public class Position {
	public int StartLine { get; }
	public int StartColumn { get; }
	public int EndLine { get; }
	public int EndColumn { get; }
	
	public Position(int startLine, int startColumn, int endLine, int endColumn) {
		this.StartLine = startLine;
		this.StartColumn = startColumn;
		this.EndLine = endLine;
		this.EndColumn = endColumn;
	}
}

public class CodeElement {
	public string Name { get; }
	public Position Position { get; }
	public string Type { get; }
	public string File { get; }
	
	public CodeElement(string name, Position position, string type, string file) {
		this.Name = name;
		this.Position = position;
		this.Type = type;
		this.File = file;
	}
}
