namespace RefactorGuidanceTool.Models.CSharp.Refactorings;

[CSharpRefactoring("Remove Class", typeof(RemoveClassRefactoring))]
public class RemoveClassRefactoring : Refactoring {
	public RemoveClassRefactoring(CodeQlBroker codeQlBroker) : base(codeQlBroker) { }

	public override IReadOnlyList<Hazard> GetHazards(Project project, Dictionary<string, string> settings) {
		var className = settings["className"];
		
		var detectorResults = this.CodeQlBroker.RunDetectors(project.Uuid, project.ProjectLanguage, "RemoveClass", s => s
				.Replace("$CLASS_TO_BE_REMOVED", className))
			.Select(result => new Hazard(result))
			.ToList();

		return detectorResults;
	}

	public override Verdict GetVerdict() {
		throw new NotImplementedException();
	}
}