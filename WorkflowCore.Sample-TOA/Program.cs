using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WorkflowCore.Extention.TOA;
using WorkflowCore.Interface;
using WorkflowCore.UBWF.Models;
using WorkflowCore.Users.Models;

namespace WorkflowCore.Sample_TOA
{
    class Program
    {
        static void Main(string[] args)
        {
            //表 UBI_TOA_Runtime
            //TOA逻辑: 
            //TOA计算  
            //      ActionTypeEnum.结束TOA || ActionTypeEnum.退回重发起   return: TOAStatus.End
            //      ActionTypeEnum.继续TOA(默认值) 下一审批岗位>0 （计算过程中找到的岗位插入到UBI_TOA_NextJobQueue）        return: TOAStatus.Continue  
            //                                     下一审批岗位=0         return: TOAStatus.End    TOA节点没有审批岗位  LastJobArrpOutcome='NoJob'
            //TOA分配: 更新UBI_TOA_Runtime的字段 NextAllocatedJobCount ++;
            //         根据 NextJobCount和NextAllocatedJobCount确定是否需要循环。
            //TOA      从UBI_TOA_NextJobQueue提取岗位 获取岗位

            var serviceProvider = ConfigureServices();

            //start the workflow host
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<TOAWorkflow, ProcInstContext>();
            host.Start();

            Console.WriteLine("Starting workflow...");
            var workflowId = host.StartWorkflow("TOA", new ProcInstContext() {
                Scope = "123"
            }).Result;


        Showlist:
            Thread.Sleep(5000);
            Console.WriteLine("waiting workflow persist... 点击回车获取任务信息");
            Console.ReadLine();
            var openWorklist = host.GetOpenActionRight(workflowId);
            while (openWorklist.Count() == 0)
            {
                Thread.Sleep(1000);
                openWorklist = host.GetOpenActionRight(workflowId);
            }
            Console.WriteLine("workflow persisted...");

            foreach (var item in openWorklist)
            {
                Console.WriteLine("   Task:" + item.WorklistId);
                Console.WriteLine("   user:" + item.AsignedApprover + ".Options are ");
                foreach (var option in item.Actions)
                {
                    Console.WriteLine(" - " + option.Key + " : " + option.Value + ", ");
                }
            }

            Console.WriteLine("点击回车开始录入审批信息！！");
            ActionRight actionRight = null;
            while (actionRight == null)
            {
                actionRight = GetActionRight(openWorklist, host);
                if(actionRight == null)
                    Console.WriteLine("任务审批信息录入不准确！");
            }

            //host.PublishUserAction(key, item.Key, value).Wait();

            host.Approve(actionRight).Wait();
            Console.WriteLine("任务开始处理！");

            goto Showlist;

            //host.Stop();
        }

        static ActionRight GetActionRight(List<ActionRight> actionRights, IWorkflowHost host)
        {
            Console.WriteLine("请输入任务号：");
            var sn = Console.ReadLine();
            Console.WriteLine("请输入审批人：");
            var approver = Console.ReadLine();
            Console.WriteLine("请输入审批Action：");
            var action = Console.ReadLine();
            ActionRight actionRight1 = null;
            //还需要输入一个用户。
            foreach (var actionRight in actionRights)
            {
                if (actionRight.WorklistId == sn)
                {
                    if (actionRight.Actions.ContainsKey(action))
                    {
                        actionRight.Action = action;
                        actionRight1 = actionRight;
                    }
                }
            }
            return actionRight1;
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            //services.AddWorkflow();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            services.AddWorkflow(x => x.UseSqlServer(@"Server=.;Database=WorkflowCore;Trusted_Connection=True;", true, true));
            //services.AddWorkflow(x => x.UsePostgreSQL(@"Server=127.0.0.1;Port=5432;Database=workflow;User Id=postgres;", true, true));

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
