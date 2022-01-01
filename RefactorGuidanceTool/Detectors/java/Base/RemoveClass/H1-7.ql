/**
 * @id isa-lab/detectors/java/removeclass/h1-7
 * @kind problem
 * @name H1-7: Field type is invalid.
 * @description Finds fields that have the to be removed class as type.
 */

import java

from Field field
where field.getType().(RefType).getQualifiedName() = "$CLASS"
select field, "Field is of type $CLASS: " + field.pp()