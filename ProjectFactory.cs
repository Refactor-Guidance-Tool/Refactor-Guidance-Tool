using RefactorGuidanceTool.Models.Project;

namespace RefactorGuidanceTool; 

public class ProjectFactory {
	private readonly CodeQlBroker _codeQlBroker;

	public ProjectFactory(CodeQlBroker codeQlBroker) {
		this._codeQlBroker = codeQlBroker;
	}
	
	public Project CreateProject(ProjectLanguage projectLanguage, string projectPath) {
		var uuid = Guid.NewGuid();
		return this.CreateProject(uuid, projectLanguage, projectPath);
	}

	public Project CreateProject(Guid uuid, ProjectLanguage projectLanguage, string projectPath) {
		return projectLanguage switch {
			ProjectLanguage.Java => new JavaProject(this._codeQlBroker, uuid, projectPath),
			ProjectLanguage.CSharp => new CSharpProject(this._codeQlBroker, uuid, projectPath),
			_ => throw new Exception()
		};
	}
}