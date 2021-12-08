namespace RefactorGuidanceTool.Models.Settings; 

public class BooleanSetting : Setting {
	public string Type => "boolean";

	public BooleanSetting(string name, bool required = true) : base(name, required) { }
}