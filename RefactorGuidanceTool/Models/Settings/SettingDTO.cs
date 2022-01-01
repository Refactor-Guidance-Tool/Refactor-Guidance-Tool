using Newtonsoft.Json;

namespace RefactorGuidanceTool.Models.Settings;

public class SettingDto {
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public BooleanSetting BooleanSetting { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public ChoiceSetting ChoiceSetting { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public StringSetting StringSetting { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public SubjectSetting SubjectSetting { get; set; }
}