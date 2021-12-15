namespace RefactorGuidanceTool.Models.Settings; 

public class ChoiceSetting : Setting {
	public IReadOnlyList<string> Choices { get; }

	public ChoiceSetting(string name, IReadOnlyList<string> choices, bool required = true) : base(name, required) {
		this.Choices = choices;
	}

	public override void FillDTO(SettingDto dto) {
		dto.ChoiceSetting = this;
	}
}