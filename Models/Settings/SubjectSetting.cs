namespace RefactorGuidanceTool.Models.Settings; 

public class SubjectSetting : Setting {
	public string Type => "subject";
	
	public SubjectSetting(string name, bool required = true) : base(name, required) { }
}