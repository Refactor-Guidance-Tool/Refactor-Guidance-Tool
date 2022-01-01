/**
 * @id isa-lab/detectors/java/removeclass/h2
 * @kind problem
 * @name H2: Superclass is removed from the inheritance tree.
 * @description Finds classes of which the superclass is the to be removed class.
 */

import java

from RefType refType
where refType.getASupertype+().getQualifiedName() = "$CLASS"
select refType, "$CLASS is used as superclass"