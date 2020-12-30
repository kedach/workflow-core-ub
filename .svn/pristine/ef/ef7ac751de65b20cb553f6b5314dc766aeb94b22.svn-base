using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.UBWF.Context;
using WorkflowCore.UBWF.Rules;

namespace WorkflowCore.Extention.TOA
{
    public class TOAActivityStep : WorkflowStep<TOAActivityStepBody>
    {
        public TOAActivityStep(string name)
        {
            this.Name = name;
        }
        public Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();


        public override IStepBody ConstructBody(IServiceProvider serviceProvider)
        {
            return new TOAActivityStepBody();
        }
    }
}
