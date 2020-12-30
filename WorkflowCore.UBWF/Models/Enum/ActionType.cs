using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowCore.UBWF.Models
{
    public enum ActionType
    {
        普通Action = 0,
        继续TOA = 1,
        结束TOA = 2,
        内部跳转 = 3,
        退回重发起 = 4,
        作废 = 5,
    }
}
