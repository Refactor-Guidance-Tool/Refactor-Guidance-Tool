namespace RefactorGuidanceTool.Models.CSharp;

public class CSharpRefactoringProvider : RefactoringProvider {
	public CSharpRefactoringProvider(CodeQlBroker codeQlBroker) :
		base(codeQlBroker, typeof(CSharpRefactoringAttribute)) { }
}