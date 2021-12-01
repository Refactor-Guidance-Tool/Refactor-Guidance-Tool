namespace RefactorGuidanceTool.Models.Project; 

public abstract class Project {
	private readonly CodeQlBroker _codeQlBroker;
	
	public Guid Uuid { get; }
	public ProjectLanguage ProjectLanguage { get; }
	public string ProjectPath { get;  }
	
	public Project(CodeQlBroker codeQlBroker, Guid uuid, ProjectLanguage projectLanguage, string projectPath) {
		this._codeQlBroker = codeQlBroker;
		
		this.Uuid = uuid;
		this.ProjectLanguage = projectLanguage;
		this.ProjectPath = projectPath;
	}

	public IReadOnlyList<Refactoring> GetAllRefactorings() {
		return new List<Refactoring>();
	}

	public void UpdateDatabase() {
		this._codeQlBroker.CreateDatabase(this.Uuid, this.ProjectPath, this.ProjectLanguage);
	}
	
	public void Delete() {
		this._codeQlBroker.DeleteDatabase(this.Uuid);
	}
}