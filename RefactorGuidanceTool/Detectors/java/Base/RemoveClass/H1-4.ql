/**
 * @id isa-lab/detectors/java/removeclass/h1-4
 * @kind problem
 * @name H1-4: Instanceof expression will become invalid.
 * @description Finds instanceof expressions where the checked type is the to be removed class.
 */

import java

from InstanceOfExpr instanceOfExpr
where instanceOfExpr.getCheckedType().getQualifiedName() = "$CLASS"
select instanceOfExpr, "Instanceof expression using the $CLASS class."