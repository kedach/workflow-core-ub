﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkflowCore.Exceptions;
using WorkflowCore.Extention.TOA;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.UBWF.Models;
using WorkflowCore.UBWF.Models.Errors;
using WorkflowCore.UBWF.Models.TOA;
using WorkflowCore.UBWF.Rules;

namespace WorkflowCore.Extention.Primitives.TOA
{
    public class JobStepBody : UBStepBody
    {
        public UBJob Job { get; set; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            this.InitUBContext(context);

            Job = (UBJob)context.ExecutionPointer.ContextItem;

            this.ProcInstContext.SetValue("Job", Job);

            if (context.PersistenceData == null)
            {
                Console.WriteLine(context.Workflow.Id + "  " + this.StageName + "Job" + Job.JobId + " <<<<< Start");

                DestinationRule destinationRule = new DestinationRule();
                destinationRule.Run(this.ProcInstContext);

                DestinationResult destinationResult = (DestinationResult)this.ProcInstContext.GetValue("DestinationResult");

                if (!string.IsNullOrEmpty(destinationResult.Error))
                {
                    throw new Exception("Job: " + Job.JobId + " JobName:" + Job.JobName + " error >" + destinationResult.Error);
                }
                if (destinationResult.Destinations.Count == 0)
                {
                    throw new Exception("Job: " + Job.JobId + " JobName:" + Job.JobName + " 未找到审批人！ >");
                }
                var values = destinationResult.Destinations.Cast<object>();

                return ExecutionResult.Branch(new List<object>(values), new ControlPersistenceData() { ChildrenActive = true });
            }

            bool succeedingRuleExe = false;
            object perdata = null;

            if (context.PersistenceData is SucceedingRuleError succeedingRuleError)
            {
                succeedingRuleExe = true;
                perdata = succeedingRuleError;
            }
            else if (context.PersistenceData is ControlPersistenceData persistenceData && persistenceData?.ChildrenActive == true)
            {
                perdata = persistenceData;
                if (context.Workflow.IsBranchComplete(context.ExecutionPointer.Id))
                {
                    Console.WriteLine(context.Workflow.Id + "  " + this.StageName + "Job" + Job.JobId + " End >>>>>>>");
                    succeedingRuleExe = true;
                }
                else 
                {
                    succeedingRuleExe = false;  
                }
            }

            if (succeedingRuleExe)
            {
                try
                {
                    SucceedingRule succeedingRule = new SucceedingRule();
                    succeedingRule.Run(ProcInstContext);

                    Console.WriteLine("JobStepBody:" + Job.JobId + " end...");
                    //找到所有的子exepointer的outcome 与job的action的outcome的结果
                    Job.Outcome = (UBAction)ProcInstContext.GetValue("Outcome");
                    string jobOutcome = Job.Outcome.Id;
                    //为下一次循环计算做准备。
                    this.ProcInstContext.SetValue("lastJob", Job);
                    return ExecutionResult.Outcome(jobOutcome);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("节点:" + this.ActName + " job:" + Job.JobName + " SucceedingRule error >" + ex.ToString());
                    context.ExecutionPointer.PersistenceData = new SucceedingRuleError();
                    succeedingRuleExe = true;
                }
            }

            return ExecutionResult.Persist(perdata);
        }
    }
}
