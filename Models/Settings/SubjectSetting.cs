namespace RefactorGuidanceTool.Models.Settings; 

public class SubjectSetting : Setting {
	public string CodeElementType { get; }

	public SubjectSetting(string identifier, string name, string codeElementType, bool required = true) : base("subject", name, identifier, required) {
		this.CodeElementType = codeElementType;
	}

	public override void FillDTO(SettingDto dto) {
		dto.SubjectSetting = this;
	}
}