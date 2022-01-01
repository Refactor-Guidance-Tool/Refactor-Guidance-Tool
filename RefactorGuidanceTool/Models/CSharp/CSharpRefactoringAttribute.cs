namespace RefactorGuidanceTool.Models.CSharp;

[AttributeUsage(AttributeTargets.Class)]
public class CSharpRefactoringAttribute : RefactoringAttribute {
	public CSharpRefactoringAttribute(string name, Type clazz) : base(name, clazz) { }
}