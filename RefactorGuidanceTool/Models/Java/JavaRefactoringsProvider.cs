namespace RefactorGuidanceTool.Models.Java;

public class JavaRefactoringProvider : RefactoringProvider {
	public JavaRefactoringProvider(CodeQlBroker codeQlBroker) :
		base(codeQlBroker, typeof(JavaRefactoringAttribute)) { }
}