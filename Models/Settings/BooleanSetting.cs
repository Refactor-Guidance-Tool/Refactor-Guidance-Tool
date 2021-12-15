namespace RefactorGuidanceTool.Models.Settings; 

public class BooleanSetting : Setting {
	public BooleanSetting(string identifier, string name, bool required = true) : base("boolean", name, identifier, required) { }
	
	public override void FillDTO(SettingDto dto) {
		dto.BooleanSetting = this;
	}
}