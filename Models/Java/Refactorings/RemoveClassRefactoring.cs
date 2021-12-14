using RefactorGuidanceTool.Models.Settings;

namespace RefactorGuidanceTool.Models.Java.Refactorings;

[JavaRefactoring("Remove Class", typeof(RemoveClassRefactoring))]
public class RemoveClassRefactoring : Refactoring {
	public RemoveClassRefactoring(CodeQlBroker codeQlBroker) : base(codeQlBroker) { }

	public override IReadOnlyList<Hazard> GetHazards(Project project, Dictionary<string, string> settings) {
		var className = settings["className"];
		var packageName = settings["packageName"];
		
		var detectorResults = this.CodeQlBroker.RunDetectors(project.Uuid, project.ProjectLanguage, "RemoveClass", s => s
				.Replace("$CLASS_PACKAGE", packageName)
				.Replace("$CLASS", className))
			.Select(result => new Hazard(result))
			.ToList();

		return detectorResults;
	}
	
	public override IReadOnlyList<Setting> GetSettings() {
		return new List<Setting>() {
			new SubjectSetting("Class to remove", "Class"),
		};
	}

	public override Verdict GetVerdict() {
		throw new NotImplementedException();
	}
}