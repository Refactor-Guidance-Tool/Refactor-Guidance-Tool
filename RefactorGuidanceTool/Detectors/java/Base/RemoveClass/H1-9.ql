/**
 * @id isa-lab/detectors/java/removeclass/h1-9
 * @kind problem
 * @name H1-9: Type access will become invalid
 * @description Finds all type accesses that will become invalid because the enclosing type is the class to be removed.
 */

import java

from TypeAccess typeAccess
where typeAccess.getType().(RefType).getEnclosingType().getQualifiedName() = "$CLASS"
select typeAccess, "$CLASS used to get access to inner type: " + typeAccess.toString()