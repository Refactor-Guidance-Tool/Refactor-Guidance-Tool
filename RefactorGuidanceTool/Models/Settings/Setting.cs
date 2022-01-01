namespace RefactorGuidanceTool.Models.Settings; 

public abstract class Setting {
	public string Type { get; }
	public string Label { get; }
	public string Identifier { get; }
	public bool Required { get; }
	
	protected Setting(string type, string label, string identifier, bool required = true) {
		this.Type = type;
		this.Label = label;
		this.Identifier = identifier;
		this.Required = required;
	}

	public abstract void FillDTO(SettingDto dto);
}