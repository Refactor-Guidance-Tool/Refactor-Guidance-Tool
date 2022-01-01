/**
 * @id isa-lab/detectors/java/removeclass/h1-6
 * @kind problem
 * @name H1-6: Parameter type is invalid
 * @description Finds paremeters of methods of which the to be removed class is being used as type.
 */

import java

from Parameter parameter
where parameter.getType().(RefType).getQualifiedName() = "$CLASS"
select parameter, "Parameter is of type $CLASS: " + parameter.pp()