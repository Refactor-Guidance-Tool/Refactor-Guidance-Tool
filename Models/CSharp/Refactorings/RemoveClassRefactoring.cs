namespace RefactorGuidanceTool.Models.CSharp.Refactorings;

public class RemoveClassRefactoring : Refactoring {
	public RemoveClassRefactoring(CodeQlBroker codeQlBroker) : base(codeQlBroker) { }
	
	public override IReadOnlyList<Hazard> GetHazards() {
		throw new NotImplementedException();
	}

	public override Verdict GetVerdict() {
		throw new NotImplementedException();
	}
}