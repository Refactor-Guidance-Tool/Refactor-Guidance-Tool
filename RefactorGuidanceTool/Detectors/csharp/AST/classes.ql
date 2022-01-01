/**
 * @name Print AST classes
 * @description Class
 * @id isa-lab/detectors/csharp/ast/classes
 * @kind problem
 */

import csharp

from Class clazz
select clazz, clazz.getQualifiedName()