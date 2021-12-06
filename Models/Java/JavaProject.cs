namespace RefactorGuidanceTool.Models.Java;

public class JavaProject : Project {
	public JavaProject(CodeQlBroker codeQlBroker, Guid uuid, string projectPath) : base(codeQlBroker, uuid,
		ProjectLanguage.Java, projectPath) { }

	public override IReadOnlyList<Refactoring> GetAllRefactorings() {
		throw new NotImplementedException();
	}
}