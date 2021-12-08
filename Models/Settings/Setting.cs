namespace RefactorGuidanceTool.Models.Settings; 

public abstract class Setting {
	public string Name { get; }
	public bool Required { get; }
	
	protected Setting(string name, bool required = true) {
		this.Name = name;
		this.Required = required;
	}
}