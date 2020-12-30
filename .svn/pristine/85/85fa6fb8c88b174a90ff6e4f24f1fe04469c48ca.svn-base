using Microsoft.Extensions.DependencyInjection;
using System;
using WorkflowCore.Extention.TOA;
using WorkflowCore.Interface;
using WorkflowCore.UBWF.Models;
using WorkflowCore.Users.Models;

namespace WorkflowCore.SampleApprove
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

            UserAction data = new UserAction()
            {
                User = "U3.1",
                OutcomeValue = "Agree"
            };
            host.PublishEvent("03e22e52-293b-4299-bd77-bc441060eec1>6cacba66-d335-462b-b4b3-b8167ad43ce5",
                "03e22e52-293b-4299-bd77-bc441060eec1.6cacba66-d335-462b-b4b3-b8167ad43ce5", data);

            Console.ReadLine();
            host.Stop();
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
