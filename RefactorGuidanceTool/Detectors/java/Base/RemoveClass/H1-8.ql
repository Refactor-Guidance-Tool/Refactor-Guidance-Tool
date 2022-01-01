/**
 * @id isa-lab/detectors/java/removeclass/h1-8
 * @kind problem
 * @name H1-8: Variable type is invalid.
 * @description Finds variables that have the to be removed class as type.
 */

import java

from LocalVariableDecl localVariableDecl
where localVariableDecl.getType().(RefType).getQualifiedName() = "$CLASS"
select localVariableDecl, "Variable is of type $CLASS: " + localVariableDecl.pp()