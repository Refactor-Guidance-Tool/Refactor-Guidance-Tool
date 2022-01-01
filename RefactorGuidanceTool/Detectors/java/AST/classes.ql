/**
 * @name Print AST classes
 * @description Class
 * @id isa-lab/detectors/java/ast/classes
 * @kind problem
 */

import java

from Class clazz
select clazz, clazz.getQualifiedName()