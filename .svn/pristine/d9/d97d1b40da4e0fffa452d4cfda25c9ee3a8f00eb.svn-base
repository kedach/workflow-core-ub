using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Extention.TOA;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Extention.Primitives.TOA
{
    /// <summary>
    /// 计算下一审批岗位
    /// </summary>
    public class UBEndStepBody : UBStepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            this.InitUBContext(context);
            Console.WriteLine("UBEndStepBody");
            return ExecutionResult.Next();
        }
    }
}
