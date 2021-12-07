namespace RefactorGuidanceTool.Models.Java.Refactorings;

[JavaRefactoring("Remove Class", typeof(RemoveClassRefactoring))]
public class RemoveClassRefactoring : Refactoring {
	public RemoveClassRefactoring(CodeQlBroker codeQlBroker, Project project) : base(codeQlBroker, project) { }
	
	public override IReadOnlyList<Hazard> GetHazards() {
		this.CodeQlBroker.RunDetectors(this.project.Uuid, this.project.ProjectLanguage, "RemoveClass", s => {
			
			
			return s;
		});

		return null;
	}

	public override Verdict GetVerdict() {
		throw new NotImplementedException();
	}
}