using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Services;
using WorkflowCore.UBWF.Models;
using WorkflowCore.UBWF.ServiceExtensions;
using WorkflowCore.Users.Models;
using WorkflowCore.Users.Primitives;

namespace WorkflowCore.Interface
{
    public static class UBWorkflowHostExtensions
    {
        public static async Task Approve(this IWorkflowHost host, ActionRight actionRight)
        {

            await host.PublishEvent(actionRight.WorklistDescr, actionRight.WorklistId, actionRight);
        }

        public static List<ActionRight> GetOpenActionRight(this IWorkflowHost host, string workflowId)
        {
            var workflow = host.PersistenceStore.GetWorkflowInstance(workflowId).Result;
            return workflow.GetOpenActionRight();
        }
    }
}
