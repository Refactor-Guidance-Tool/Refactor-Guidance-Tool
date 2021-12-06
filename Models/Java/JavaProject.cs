namespace RefactorGuidanceTool.Models.Java;

public class JavaProject : Project {
	private IReadOnlyList<Type> refactorings = new List<Type>();

	public JavaProject(CodeQlBroker codeQlBroker, Guid uuid, string projectPath) : base(codeQlBroker, uuid,
		ProjectLanguage.Java, projectPath) {
		this.PopulateRefactorings(typeof(JavaRefactoringAttribute));
	}

	public override IReadOnlyList<RefactoringDTO> GetAllRefactorings() {
		return this._refactorings.Keys.ToList();
	}
}