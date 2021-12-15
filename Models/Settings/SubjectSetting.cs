namespace RefactorGuidanceTool.Models.Settings; 

public class SubjectSetting : Setting {
	public string CodeElementType { get; }

	public SubjectSetting(string name, string codeElementType, bool required = true) : base(name, required) {
		this.CodeElementType = codeElementType;
	}

	public override void FillDTO(SettingDto dto) {
		dto.SubjectSetting = this;
	}
}