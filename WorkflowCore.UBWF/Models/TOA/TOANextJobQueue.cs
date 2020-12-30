using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.Models.TOA
{
    public class TOANextJobQueue: BaseEntity
    {    
        public string ProcInstId { get; set; }
        public string ActName { get; set; }
        public string JobID { get; set; }
        public string JobName { get; set; }
        public string JobXml { get; set; }
    }
}
