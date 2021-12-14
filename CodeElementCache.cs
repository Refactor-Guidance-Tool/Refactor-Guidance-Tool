using RefactorGuidanceTool.Models;

namespace RefactorGuidanceTool; 

public class CodeElementCache {
	private readonly Dictionary<Guid, IReadOnlyList<CodeElement>> _cachedCodeElements;
	
	public CodeElementCache() {
		this._cachedCodeElements = new Dictionary<Guid, IReadOnlyList<CodeElement>>();
	}

	public void Register(Guid id, IReadOnlyList<CodeElement> codeElements) {
		this._cachedCodeElements.Add(id, codeElements);
	}

	public void Invalidate(Guid id) {
		this._cachedCodeElements.Remove(id);
	}

	public bool TryGet(Guid id, out IReadOnlyList<CodeElement> codeElements) {
		return this._cachedCodeElements.TryGetValue(id, out codeElements!);
	}
}