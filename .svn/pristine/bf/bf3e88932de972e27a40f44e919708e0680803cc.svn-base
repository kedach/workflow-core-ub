using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Extention.TOA;
using WorkflowCore.UBWF.Context;
using WorkflowCore.UBWF.Models;

namespace WorkflowCore.UBWF.Rules
{
    public class EventSucceedingRule : IUBEventRule
    {
        public void Run(ProcInstContext PIContext)
        {
            Console.WriteLine("EventSucceedingRule dest:" + PIContext.GetValue("Destination"));
        }
    }
}
