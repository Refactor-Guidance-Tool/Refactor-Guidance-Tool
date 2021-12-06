using RefactorGuidanceTool.Models.Java;

namespace RefactorGuidanceTool.Models.Java.Refactorings;

[JavaRefactoring("Remove Class", typeof(RemoveClassRefactoring))]
public class RemoveClassRefactoring : Refactoring {
	public RemoveClassRefactoring(CodeQlBroker codeQlBroker, Project project) : base(codeQlBroker, project) { }
	
	public override IReadOnlyList<Hazard> GetHazards() {
		this.CodeQlBroker.RunDetectors(project.Uuid, project.ProjectLanguage, "RemoveClass", s => {
			
			
			return s;
		});

		return null;
	}

	public override Verdict GetVerdict() {
		throw new NotImplementedException();
	}
}