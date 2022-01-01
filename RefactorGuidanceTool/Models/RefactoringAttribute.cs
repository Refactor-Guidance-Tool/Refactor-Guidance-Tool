namespace RefactorGuidanceTool.Models;

[AttributeUsage(AttributeTargets.Class)]
public class RefactoringAttribute : Attribute {
	public string Name { get; }
	public string Id { get; }

	protected RefactoringAttribute(string name, Type clazz) {
		this.Name = name;
		this.Id = clazz.ToString();
	}
}