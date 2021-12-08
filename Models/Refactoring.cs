namespace RefactorGuidanceTool.Models;

public abstract class Refactoring {
	protected CodeQlBroker CodeQlBroker;
	private Guid id;
	
	public Refactoring(CodeQlBroker codeQlBroker) {
		this.CodeQlBroker = codeQlBroker;
		this.id = Guid.NewGuid();
	}
	
	public abstract IReadOnlyList<Hazard> GetHazards(Project project, Dictionary<string, string> settings);
	
	public abstract Verdict GetVerdict();
	
	public Guid GetGuid() {
		return this.id;
	}
}