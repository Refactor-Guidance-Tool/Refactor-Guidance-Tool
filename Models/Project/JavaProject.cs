namespace RefactorGuidanceTool.Models.Project; 

public class JavaProject : Project {
	public JavaProject(CodeQlBroker codeQlBroker, Guid uuid, string projectPath) : base(codeQlBroker, uuid, ProjectLanguage.Java, projectPath) { }
}