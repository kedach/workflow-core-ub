﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.Models.TOA
{
    public class BaseEntity
    {
        public string UUID { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; }
    }
}
