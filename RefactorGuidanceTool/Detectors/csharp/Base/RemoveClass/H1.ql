/**
 * @id isa-lab/detectors/csharp/removeclass/h1
 * @kind problem
 * @name Class extends detector
 * @description Finds classes that extend the class to be removed.
 */

import csharp

from RefType type
where type.getABaseType+().hasQualifiedName("$CLASS_TO_BE_REMOVED")
select type, type.getQualifiedName() + " extends $CLASS_TO_BE_REMOVED"