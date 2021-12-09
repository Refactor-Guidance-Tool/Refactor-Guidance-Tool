namespace RefactorGuidanceTool.Models.Settings; 

public class StringSetting : Setting {
	public readonly string String;

	public StringSetting(string name, string str, bool required = true) : base("string", name, required) {
		this.String = str;
	}
}