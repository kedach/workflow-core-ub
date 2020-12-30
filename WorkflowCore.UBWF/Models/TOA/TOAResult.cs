using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using WorkflowCore.UBWF.Models;
using WorkflowCore.UBWF.Models.TOA;

namespace WorkflowCore.UBWF.SimProvider
{
    public struct CalResult
    {
        /// <summary>
        /// 整个TOA的状态
        /// </summary>
        public TOAStatus TOAStatus;

        /// <summary>
        /// 上一审批岗位是否循环岗位
        /// </summary>
        public UBJob LastJob;
        /// <summary>
        /// TOA选人规则XML  Root\Rules\Rule[EType='TOA']
        /// </summary>
        public string RuleConfig;

        public List<UBJob> NextJobQueue { get; set; }
    }
}
