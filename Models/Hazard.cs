namespace RefactorGuidanceTool.Models;

public class Hazard {
	public string Message { get; }
	public string Name { get; }
	public string Source { get; }
	public Position Position { get; }

	public Hazard(CodeQlBroker.DetectorResult detectorResult) {
		this.Message = detectorResult.Message;
		this.Name = detectorResult.DetectorName;
		this.Source = detectorResult.Source;
		this.Position = new Position(
			detectorResult.StartLine,
			detectorResult.StartChar,
			detectorResult.EndLine,
			detectorResult.EndChar
		);
	}
}