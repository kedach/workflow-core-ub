using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Extention.TOA;
using WorkflowCore.UBWF.Context;
using WorkflowCore.UBWF.Models;

namespace WorkflowCore.UBWF.Rules
{
    public class ServerEventRule : IUBEventRule
    {
        public void Run(ProcInstContext PIContext)
        {
            Console.WriteLine("ServerEventRule");
        }
    }
}
