namespace RefactorGuidanceTool.Models.Settings; 

public class StringSetting : Setting {
	public string Type => "string";
	public readonly string String;

	public StringSetting(string name, string str, bool required = true) : base(name, required) {
		this.String = str;
	}
}