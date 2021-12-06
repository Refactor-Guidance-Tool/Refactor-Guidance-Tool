namespace RefactorGuidanceTool.Models;

public abstract class Refactoring {
	protected CodeQlBroker CodeQlBroker;
	protected Project project;
	private Guid id;
	
	public Refactoring(CodeQlBroker codeQlBroker, Project project) {
		this.CodeQlBroker = codeQlBroker;
		this.project = project;
		this.id = Guid.NewGuid();
	}

	public abstract IReadOnlyList<Hazard> GetHazards();
	public abstract Verdict GetVerdict();

	public Guid GetGuid() {
		return this.id;
	}
}