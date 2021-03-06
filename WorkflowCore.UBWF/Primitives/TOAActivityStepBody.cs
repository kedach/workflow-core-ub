﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;
using WorkflowCore.UBWF.Context;
using WorkflowCore.UBWF.Rules;

namespace WorkflowCore.Extention.TOA
{
    public class TOABranch
    { 
        public string JobId { get; set; }
        public string Destination { get; set; }
        /// <summary>
        /// 对应clientevet的id
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// TOA逻辑
    /// </summary>
    public class TOAActivityStepBody : ContainerStepBody
    {
        IDictionary<string, IDictionary<string, string>> PIContext { get; }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("TOAActivityStep run...");
            UBTOAContext uBTOAContext = new UBTOAContext()
            {
                WorkflowId = context.Workflow.Id,
                LastJobID = ""
            };
            if (context.PersistenceData != null)
            {
                uBTOAContext = context.PersistenceData as UBTOAContext;
            }
            try
            {
                if (context.ExecutionPointer.ContextItem != null)
                {
                    var branch = context.ExecutionPointer.ContextItem as TOABranch;
                    uBTOAContext.LastJobID = branch.JobId;
                }


                //TOA 计算  需要传入context
                IList<string> calRst = new List<string>();
                calRst.Add("job1");
                calRst.Add("job2");
                Console.WriteLine("符合条件的岗位count：" + calRst.Count);
                //循环
                if (calRst.Count == 0)
                {
                    Console.WriteLine("符合条件的岗位count：" + calRst.Count);
                    ExecutionResult er = ExecutionResult.Next();
                    er.OutcomeValue = "End";

                    return er;
                }

                //List<object> branchs = new List<object>();
                //foreach (var job in calRst)
                //{
                //    DestinationRule dstinationRule = new DestinationRule();
                //    dstinationRule.Run(uBTOAContext);
                //    foreach (string dest in uBTOAContext.Destinations)
                //    {
                //        uBTOAContext.Destination = dest;
                //        ClientEventRule eventRule = new ClientEventRule();
                //        eventRule.Run(uBTOAContext);
                //        branchs.Add(new TOABranch()
                //        {
                //            JobId = job,
                //            Destination = dest,
                //            Id = Guid.NewGuid().ToString()
                //        });
                //    }
                //}

                //return ExecutionResult.Persist(context.PersistenceData);
                //return ExecutionResult.Branch(branchs, uBTOAContext);
                return ExecutionResult.Next();
            }
            catch (Exception ex)
            {
                Console.WriteLine("TOAActivityStep error:" + ex.ToString());
                return ExecutionResult.Persist(new
                {
                    Error = ex.ToString(),
                    LastJobId = uBTOAContext.LastJobID
                });
            }


            //EventSucceedingRule eventSucceedingRule = new EventSucceedingRule();
            //eventSucceedingRule.Run(uBTOAContext);

            //uBTOAContext.Destination = "";
            //SucceedingRule succeedingRule = new SucceedingRule();
            //succeedingRule.Run(uBTOAContext);
        }
    }
}
