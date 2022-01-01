using System.Data.Common;

namespace RefactorGuidanceTool.Models.Settings; 

public class StringSetting : Setting {
	public string String { get; }

	public StringSetting(string identifier, string label, string str, bool required = true) : base("string", label, identifier, required) {
		this.String = str;
	}

	public override void FillDTO(SettingDto dto) {
		dto.StringSetting = this;
	}
}