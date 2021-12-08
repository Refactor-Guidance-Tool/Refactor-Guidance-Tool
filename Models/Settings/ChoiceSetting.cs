namespace RefactorGuidanceTool.Models.Settings; 

public class ChoiceSetting : Setting {
	public string Type => "choice";
	public readonly IReadOnlyList<string> Choices;

	public ChoiceSetting(string name, IReadOnlyList<string> choices, bool required = true) : base(name, required) {
		this.Choices = choices;
	}
}