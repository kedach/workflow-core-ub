using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Extention.TOA;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.UBWF.Models;
using WorkflowCore.UBWF.Rules;
using WorkflowCore.Users.Models;

namespace WorkflowCore.Extention.Primitives.TOA
{
    public class DestStepBody : UBStepBody
    {
        //private readonly Dictionary<string, string> _options;

        //public DestStepBody(Dictionary<string, string> options)
        //{
        //    _options = options;
        //}
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            this.InitUBContext(context);
            UBDestination dest = (UBDestination)context.ExecutionPointer.ContextItem;
            //持久化并等待事件 异常后会触发流程异常事件。retry后会重新进入此段逻辑。
            if (!context.ExecutionPointer.EventPublished)
            {
                this.ProcInstContext.SetValue("Destination", dest);
                //需要根据岗位配置来
                Dictionary<string, string> _options = new Dictionary<string, string>();
                _options.Add("Agree", "同意");
                _options.Add("DisAgree", "退回");

                //审批人
                context.ExecutionPointer.ExtensionAttributes[WorklistExtConst.ExtApprover] = dest.UserId;
                //审批意见列表
                context.ExecutionPointer.ExtensionAttributes[WorklistExtConst.ExtActions] = _options;

                ClientEventRule cr = new ClientEventRule();
                cr.Run(this.GetContext());
                Console.WriteLine(context.Workflow.Id + "  " + this.StageName + "dest:" + dest.UserId + "  job:" + dest.Job.JobId);
                var effectiveDate = DateTime.Now.ToUniversalTime();
                var eventKey = context.ExecutionPointer.Id+ ".evt";
                //岗位加人员
                string eventName = dest.Job.JobName + "." + dest.UserName;
;
                return ExecutionResult.WaitForEvent(eventName, eventKey, effectiveDate);
            }

            try
            {
                if (!(context.ExecutionPointer.EventData is ActionRight))
                    throw new ArgumentException();
                //时间发布后的执行 即doaction的操作
                var action = ((ActionRight)context.ExecutionPointer.EventData);

                this.ProcInstContext.SetValue("LastJob", dest.Job);
                EventSucceedingRule eventSucceedingRule = new EventSucceedingRule();
                eventSucceedingRule.Run(ProcInstContext);

                return ExecutionResult.Outcome(action.Action);
            }
            catch (Exception ex)
            {
                Console.WriteLine(context.Workflow.Id + "  " + this.StageName + "EventSucceedingRule error >" + ex.ToString());
                throw;
            }
        }
    }
}
