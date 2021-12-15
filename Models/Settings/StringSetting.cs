namespace RefactorGuidanceTool.Models.Settings; 

public class StringSetting : Setting {
	public string String { get; }

	public StringSetting(string name, string str, bool required = true) : base("string", name, required) {
		this.String = str;
	}

	public override void FillDTO(SettingDto dto) {
		dto.StringSetting = this;
	}
}