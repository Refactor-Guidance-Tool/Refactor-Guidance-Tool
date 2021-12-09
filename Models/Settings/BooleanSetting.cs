namespace RefactorGuidanceTool.Models.Settings; 

public class BooleanSetting : Setting {
	public BooleanSetting(string name, bool required = true) : base("boolean", name, required) { }
}