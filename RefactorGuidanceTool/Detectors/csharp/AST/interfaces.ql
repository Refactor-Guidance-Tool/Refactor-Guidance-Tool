/**
 * @name Print AST interfaces
 * @description Interface
 * @id isa-lab/detectors/csharp/ast/interfaces
 * @kind problem
 */

import csharp

from Interface interface
select interface, interface.getQualifiedName()