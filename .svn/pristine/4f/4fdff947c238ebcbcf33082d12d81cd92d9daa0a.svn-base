using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Extention.TOA;
using WorkflowCore.UBWF.Context;
using WorkflowCore.UBWF.Models;

namespace WorkflowCore.UBWF.Rules
{
    public class SucceedingRule : IUBEventRule
    {
        public void Run(ProcInstContext PIContext)
        {
            UBJob job = (UBJob)PIContext.GetValue("Job");
            PIContext.SetValue("Outcome", new UBAction()
            {
                Id = "Agree",
                Name = "同意",
                ActionJob = job.JobId,
                ActionAct = "",
                ActionType = ActionType.继续TOA,
                MultiRule = ""
            });
            Console.WriteLine("SucceedingRule");
        }
    }
}
