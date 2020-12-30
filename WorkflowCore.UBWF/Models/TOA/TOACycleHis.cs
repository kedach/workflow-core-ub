using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.Models.TOA
{
    /// <summary>
    /// 循环岗位的队列历史
    /// </summary>
    public class TOACycleHis: BaseEntity
    {
        public string ProcInstId { get; set; }
        public string ActInstID { get; set; }
        public string JobID { get; set; }
        public string JobName { get; set; }
        public string Destination { get; set; }
    }
}
