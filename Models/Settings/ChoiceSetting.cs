namespace RefactorGuidanceTool.Models.Settings; 

public class ChoiceSetting : Setting {
	public readonly IReadOnlyList<string> Choices;

	public ChoiceSetting(string name, IReadOnlyList<string> choices, bool required = true) : base("choice", name, required) {
		this.Choices = choices;
	}
}