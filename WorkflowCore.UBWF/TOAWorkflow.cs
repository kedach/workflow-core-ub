using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;
using WorkflowCore.UBWF.Models;

namespace WorkflowCore.Extention.TOA
{    
    public class TOAWorkflow : IWorkflow<ProcInstContext>
    {
        public string Id => "TOA";
        public int Version => 1;

        public void Build(IWorkflowBuilder<ProcInstContext> builder)
        {
            builder.StartWith(context =>
            {
                context.Step.Name = "Start";
                Console.WriteLine("Workflow started");
                return ExecutionResult.Next();
            })
            .TOA("TOA1"," toaid")
            .Then(context =>
            {
                Console.WriteLine("Workflow end");
                return ExecutionResult.Next();
            });
        }        
    }
}
