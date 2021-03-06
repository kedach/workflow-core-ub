﻿using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Extention.TOA;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.UBWF.Rules;

namespace WorkflowCore.Extention.Primitives.TOA
{
    public class TOAJobStepBody : UBStepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            DestinationRule destinationRule = new DestinationRule();

            return ExecutionResult.Next();
        }
    }
}
