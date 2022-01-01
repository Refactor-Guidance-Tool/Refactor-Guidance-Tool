/**
 * @id isa-lab/detectors/java/removeclass/h1-3
 * @kind problem
 * @name H1-3: Field access to class will become invalid.
 * @description Finds object references to the to be removed class.
 */

import java

from FieldAccess fieldAccess
where /*not fieldAccess.isOwnFieldAccess() and*/ fieldAccess.getField().getDeclaringType().getQualifiedName() = "$CLASS"
select fieldAccess, "Field access of class $CLASS: " + fieldAccess.toString()