using RefactorGuidanceTool.Models.CSharp;

namespace RefactorGuidanceTool.Models.Java; 

public class JavaRefactoringAttribute : RefactoringAttribute {
	public JavaRefactoringAttribute(string name, Type clazz) : base(name, clazz) { }
}