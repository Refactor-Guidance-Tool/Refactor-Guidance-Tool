/**
 * @id isa-lab/detectors/java/removeclass/h1-5
 * @kind problem
 * @name H1-5: Return type is invalid
 * @description Finds methods of which the return type is the class to be removed.
 */

import java

from Callable callable
where callable.getReturnType().(RefType).getQualifiedName() = "$CLASS"
select callable, "$CLASS is used as return type in method: " + callable.getStringSignature()