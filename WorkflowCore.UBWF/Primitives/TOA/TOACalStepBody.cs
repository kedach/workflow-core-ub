﻿using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Extention.TOA;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.UBWF.Models;
using WorkflowCore.UBWF.Models.TOA;
using WorkflowCore.UBWF.SimProvider;

namespace WorkflowCore.Extention.Primitives.TOA
{
    public class TOACalStepBody : UBStepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine(context.Workflow.Id + "  " + this.StageName + " <<<<< Start");
            this.InitUBContext(context);
            //TOA的入口和起始点。
            UBJob lastJob = (UBJob)this.ProcInstContext.GetValue("LastJob");


            SimTOAProvider simTOAProvider = new SimTOAProvider(this.ProcInstContext);
            CalResult calResult = simTOAProvider.Calculate();
            List<UBJob> NextJobQueue = calResult.NextJobQueue;
            //为岗位循环做好准备。
            this.ProcInstContext.SetValue("NextJobQueue", NextJobQueue);

            Console.WriteLine(context.Workflow.Id + "  " + this.StageName + " End >>>>>>>");
            if (NextJobQueue != null && NextJobQueue.Count > 0)
                return ExecutionResult.Outcome(TOAStatus.Continue.ToString());
            //清理上下文信息。
            this.ProcInstContext.ClearScopeContext();
            return ExecutionResult.Outcome(lastJob.Outcome.Id);
        }
    }
}
