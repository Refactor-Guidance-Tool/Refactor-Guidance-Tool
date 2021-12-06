namespace RefactorGuidanceTool.Models;

public abstract class Project {
	protected readonly CodeQlBroker CodeQlBroker;

	public Guid Uuid { get; }
	public ProjectLanguage ProjectLanguage { get; }
	public string ProjectPath { get; }

	public Project(CodeQlBroker codeQlBroker, Guid uuid, ProjectLanguage projectLanguage, string projectPath) {
		this.CodeQlBroker = codeQlBroker;

		this.Uuid = uuid;
		this.ProjectLanguage = projectLanguage;
		this.ProjectPath = projectPath;
	}

	public abstract IReadOnlyList<Refactoring> GetAllRefactorings();

		public void UpdateDatabase() {
		this.CodeQlBroker.DeleteDatabase(this.Uuid);
		this.CodeQlBroker.CreateDatabase(this.Uuid, this.ProjectPath, this.ProjectLanguage);
	}

	public void Delete() {
		this.CodeQlBroker.DeleteDatabase(this.Uuid);
	}
}