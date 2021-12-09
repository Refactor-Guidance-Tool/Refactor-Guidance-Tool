namespace RefactorGuidanceTool.Models.Settings; 

public class SubjectSetting : Setting {
	public SubjectSetting(string name, bool required = true) : base("subject", name, required) { }
}