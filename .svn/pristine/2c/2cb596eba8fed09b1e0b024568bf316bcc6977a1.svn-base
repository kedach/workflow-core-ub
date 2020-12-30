using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Extention.TOA;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Extention.Primitives.TOA
{
    /// <summary>
    /// 开始节点
    /// </summary>
    public class UBStartStep : WorkflowStep
    {
        public override Type BodyType => null;

        public override ExecutionPipelineDirective InitForExecution(
            WorkflowExecutorResult executorResult,
            WorkflowDefinition defintion,
            WorkflowInstance workflow,
            ExecutionPointer executionPointer)
        {
            this.Name = "Start";
            return ExecutionPipelineDirective.Next;
        }
    }
}
