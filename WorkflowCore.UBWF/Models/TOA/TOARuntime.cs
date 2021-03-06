﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.Models.TOA
{
    public class TOARuntime : BaseEntity
    {
        public string ProcInstId { get; set; }
        public string ActName { get; set; }
        public string TOAName { get; set; }
        public string LastApprJob { get; set; }
        public string LastApprJobId { get; set; }
        public string LastApprJobAction { get; set; }
        public string LastApprJobIsCycle { get; set; }
        public string NextJobCount { get; set; }
        public string NextAllocatedJobCount { get; set; }
        public TOARuntimeStatus Status { get; set; }
    }
}
