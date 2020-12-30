using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.Models
{
    public class UBAction
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ActionAct { get; set; }
        public string ActionJob { get; set; }
        public ActionType ActionType { get; set; }
        public string MultiRule { get; set; }
    }


}
