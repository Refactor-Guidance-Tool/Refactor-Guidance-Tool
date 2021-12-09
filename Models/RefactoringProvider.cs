namespace RefactorGuidanceTool.Models; 

public abstract class RefactoringProvider {
	private readonly Dictionary<RefactoringDTO, Func<Refactoring>> _refactorings;

	public RefactoringProvider(CodeQlBroker codeQlBroker, Type attributeType) {
		this._refactorings = new Dictionary<RefactoringDTO, Func<Refactoring>>();

		var assembly = System.Reflection.Assembly.GetExecutingAssembly();
		var types = assembly.GetTypes();

		foreach (var type in types) {
			var attribute = type.GetCustomAttributes(attributeType, true).FirstOrDefault();

			if (attribute == null) continue;
			
			var refactoringAttribute = (RefactoringAttribute) attribute;

			var refactoringDto = new RefactoringDTO(refactoringAttribute.Name, refactoringAttribute.Id);

			this._refactorings.Add(refactoringDto, () => {
				return (Refactoring) type
					.GetConstructor(new Type[] {typeof(CodeQlBroker)})!
					.Invoke(new object[] {codeQlBroker});
			});
		}
	}

	public List<RefactoringDTO> GetRefactorings() {
		return this._refactorings.Keys.ToList();
	}
	
	public Refactoring GetRefactoring(string refactoringId) {
		var refactoringDto = this._refactorings.Keys.FirstOrDefault(dto => dto.Id == refactoringId)!;
		var creator = this._refactorings[refactoringDto];
		var refactoring = creator();

		return refactoring;
	}
}