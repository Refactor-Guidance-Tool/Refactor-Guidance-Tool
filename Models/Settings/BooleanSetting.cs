namespace RefactorGuidanceTool.Models.Settings; 

public class BooleanSetting : Setting {
	public BooleanSetting(string identifier, string label, bool required = true) : base("boolean", label, identifier, required) { }
	
	public override void FillDTO(SettingDto dto) {
		dto.BooleanSetting = this;
	}
}