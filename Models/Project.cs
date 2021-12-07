using RefactorGuidanceTool.Models.CSharp;

namespace RefactorGuidanceTool.Models;

public abstract class Project {
	protected readonly CodeQlBroker CodeQlBroker;

	protected Dictionary<RefactoringDTO, Func<Refactoring>> _refactorings;
	
	public Guid Uuid { get; }
	public ProjectLanguage ProjectLanguage { get; }
	public string ProjectPath { get; }

	public Project(CodeQlBroker codeQlBroker, Guid uuid, ProjectLanguage projectLanguage, string projectPath) {
		this.CodeQlBroker = codeQlBroker;

		this.Uuid = uuid;
		this.ProjectLanguage = projectLanguage;
		this.ProjectPath = projectPath;
	}

	protected void PopulateRefactorings(Type attributeType) {
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
					.Invoke(new object[] {this.CodeQlBroker});
			});
		}
	}

	public IReadOnlyList<CodeElement> GetCodeElements() {
		return this.CodeQlBroker.GetCodeElements(this.Uuid, this.ProjectLanguage);
	}

	public abstract IReadOnlyList<RefactoringDTO> GetAllRefactorings();

	public void UpdateDatabase() {
		this.CodeQlBroker.DeleteDatabase(this.Uuid);
		this.CodeQlBroker.CreateDatabase(this.Uuid, this.ProjectPath, this.ProjectLanguage);
	}

	public void Delete() {
		this.CodeQlBroker.DeleteDatabase(this.Uuid);
	}
}