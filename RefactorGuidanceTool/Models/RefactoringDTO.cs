namespace RefactorGuidanceTool.Models; 

public class RefactoringDTO {
	public string Name { get; }
	public string Id { get; }
	
	public RefactoringDTO(string name, string id) {
		this.Name = name;
		this.Id = id;
	}
}