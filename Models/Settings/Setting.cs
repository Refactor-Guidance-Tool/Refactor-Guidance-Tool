namespace RefactorGuidanceTool.Models.Settings; 

public abstract class Setting {
	public string Type { get; }
	public string Name { get; }
	public bool Required { get; }
	
	protected Setting(string type, string name, bool required = true) {
		this.Type = type;
		this.Name = name;
		this.Required = required;
	}

	public abstract void FillDTO(SettingDto dto);
}