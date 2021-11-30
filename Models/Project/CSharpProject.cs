namespace RefactorGuidanceTool.Models.Project; 

public class CSharpProject : Project {
	public CSharpProject(CodeQlBroker codeQlBroker, Guid uuid, string projectPath) : base(codeQlBroker, uuid, ProjectLanguage.CSharp, projectPath) {
		
	}
}