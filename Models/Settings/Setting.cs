namespace RefactorGuidanceTool.Models.Settings; 

public abstract class Setting {
	public string Type { get; }
	public string Name { get; }
	public string Identifier { get; }
	public bool Required { get; }
	
	protected Setting(string type, string name, string identifier, bool required = true) {
		this.Type = type;
		this.Name = name;
		this.Identifier = identifier;
		this.Required = required;
	}

	public abstract void FillDTO(SettingDto dto);
}