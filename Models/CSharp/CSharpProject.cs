using RefactorGuidanceTool.Models.CSharp.Refactorings;

namespace RefactorGuidanceTool.Models.CSharp;

public class CSharpProject : Project {
	public CSharpProject(CodeQlBroker codeQlBroker, Guid uuid, string projectPath) : base(codeQlBroker, uuid,
		ProjectLanguage.CSharp, projectPath) { }

	public override IReadOnlyList<Refactoring> GetAllRefactorings() {
		return new List<Refactoring>() {
			new RemoveClassRefactoring(this.CodeQlBroker),
		};
	}
}