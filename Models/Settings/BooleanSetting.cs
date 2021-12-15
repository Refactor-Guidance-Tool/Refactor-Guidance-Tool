namespace RefactorGuidanceTool.Models.Settings; 

public class BooleanSetting : Setting {
	public BooleanSetting(string name, bool required = true) : base(name, required) { }
	
	public override void FillDTO(SettingDto dto) {
		dto.BooleanSetting = this;
	}
}