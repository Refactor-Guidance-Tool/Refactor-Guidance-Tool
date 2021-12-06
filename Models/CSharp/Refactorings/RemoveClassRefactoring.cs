﻿namespace RefactorGuidanceTool.Models.CSharp.Refactorings;

[CSharpRefactoring("Remove Class", typeof(RemoveClassRefactoring))]
public class RemoveClassRefactoring : Refactoring {
	public RemoveClassRefactoring(CodeQlBroker codeQlBroker, Project project) : base(codeQlBroker, project) { }
	
	public override IReadOnlyList<Hazard> GetHazards() {
		throw new NotImplementedException();
	}

	public override Verdict GetVerdict() {
		throw new NotImplementedException();
	}
}