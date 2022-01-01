/**
 * @id isa-lab/detectors/java/removeclass/h1-1
 * @kind problem
 * @name H1-1: Instantiations (keyword new) of a class will become invalid.
 * @description Finds instantations of the to be removed class.
 */

import java

from ConstructorCall constructorCall
where not constructorCall.callsSuper() and constructorCall.getConstructedType().getQualifiedName() = "$CLASS"
select constructorCall, "Creation of new instance of type $CLASS: " + constructorCall.toString()