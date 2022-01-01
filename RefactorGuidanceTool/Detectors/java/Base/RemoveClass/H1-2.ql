/**
 * @id isa-lab/detectors/java/removeclass/h1-2
 * @kind problem
 * @name H1-2: Method access to class will become invalid.
 * @description Finds object references to the to be removed class.
 */

import java

from MethodAccess methodAccess
where /*not methodAccess.isOwnMethodAccess() and*/ methodAccess.getReceiverType().getQualifiedName() = "$CLASS"
select methodAccess, "Method access of class $CLASS: " + methodAccess.printAccess()