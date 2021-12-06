namespace RefactorGuidanceTool.Models;

public abstract class Refactoring {
	protected CodeQlBroker CodeQlBroker;

	public Refactoring(CodeQlBroker codeQlBroker) {
		this.CodeQlBroker = codeQlBroker;
	}

	public abstract IReadOnlyList<Hazard> GetHazards();
	public abstract Verdict GetVerdict();
}