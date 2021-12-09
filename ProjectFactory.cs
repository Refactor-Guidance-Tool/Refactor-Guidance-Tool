using RefactorGuidanceTool.Models;
using RefactorGuidanceTool.Models.CSharp;
using RefactorGuidanceTool.Models.Java;

namespace RefactorGuidanceTool; 

public class ProjectFactory {
	protected readonly CodeQlBroker _codeQlBroker;
	protected readonly Dictionary<ProjectLanguage, RefactoringProvider> _refactoringProviders;

	public ProjectFactory(CodeQlBroker codeQlBroker, Dictionary<ProjectLanguage, RefactoringProvider> _refactoringProviders) {
		this._codeQlBroker = codeQlBroker;
		this._refactoringProviders = _refactoringProviders;
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