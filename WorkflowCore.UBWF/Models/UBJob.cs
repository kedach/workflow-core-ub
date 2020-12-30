using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.Models
{
    public class UBJob
    {
        public string ProcInstId { get; set; }
        public string ActName { get; set; }
        public string JobId { get; set; }
        public string JobName { get; set; }
        public string JobConfig { get; set; }
        public bool Cycle { get; set; }
        public UBAction Outcome { get; set; }

        public string SkipRule { get; set; }
        public List<UBAction> Actions { get; set; }
    }
}
