namespace RefactorGuidanceTool.Models.Settings; 

public class ChoiceSetting : Setting {
	public IReadOnlyList<string> Choices { get; }

	public ChoiceSetting(string identifier, string label, IReadOnlyList<string> choices, bool required = true) : base("choice", label, identifier, required) {
		this.Choices = choices;
	}

	public override void FillDTO(SettingDto dto) {
		dto.ChoiceSetting = this;
	}
}