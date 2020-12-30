using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowCore.Models;
using WorkflowCore.UBWF.Models;
using WorkflowCore.Users.Models;
using WorkflowCore.Users.Primitives;

namespace WorkflowCore.UBWF.ServiceExtensions
{
    public static class UBWorkflowInstanceExtensions
    {
        public static List<ActionRight> GetOpenActionRight(this WorkflowInstance workflow)
        {
            List<ActionRight> actionRights = new List<ActionRight>();
            var pointers = workflow.ExecutionPointers.Where(x => !x.EventPublished && x.Status == PointerStatus.WaitingForEvent ).ToList();
            foreach (var pointer in pointers)
            {
                var item = new ActionRight()
                {
                    WorklistId = pointer.EventKey,
                    WorklistDescr = pointer.EventName,
                    AsignedApprover = Convert.ToString(pointer.ExtensionAttributes[WorklistExtConst.ExtApprover]),
                    Actions = (pointer.ExtensionAttributes[WorklistExtConst.ExtActions] as Dictionary<string, string>)
                };

                actionRights.Add(item);
            }

            return actionRights;
        }
    }
}
