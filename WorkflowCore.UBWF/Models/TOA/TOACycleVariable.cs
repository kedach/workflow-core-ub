﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.Models.TOA
{
    public class TOACycleVariable: BaseEntity
    {
        public string ProcInstId { get; set; }
        public string JobID { get; set; }
        public string JobName { get; set; }
        public string Variable { get; set; }
        public string Value { get; set; }
    }
}
