using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkflowCore.Extention.Primitives.TOA;
using WorkflowCore.Extention.TOA;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;
using WorkflowCore.UBWF.Models;
using WorkflowCore.Services;

namespace WorkflowCore.Interface
{
    public static class UBStepBuilderExtensions
    {
        public static IStepBuilder<TData, TOAJobsScheduleStepBody> TOA<TData, TStepBody>(this IStepBuilder<TData, TStepBody> builder, string name, string id)
            where TStepBody : IStepBody
        {
            //TOA cal
            var newStep = new WorkflowStep<TOACalStepBody>();
            builder.WorkflowBuilder.AddStep(newStep);
            builder.Step.Outcomes.Add(new ValueOutcome()
            {
                NextStep = newStep.Id
            });

            AddInputParams(newStep, "TOACal", id, name);

            //TOA JOB foreach
            var jobScheduleStep = new WorkflowStep<TOAJobsScheduleStepBody>();
            builder.WorkflowBuilder.AddStep(jobScheduleStep);

            Expression<Func<object, object>> continueExpr = x => TOAStatus.Continue.ToString();
            newStep.Outcomes.Add(new ValueOutcome()
            {
                NextStep = jobScheduleStep.Id,
                Value = continueExpr
            });

            //结束时输出
            //Expression<Func<object, object>> endExpr = x => TOAStatus.End;
            //newStep.Outcomes.Add(new ValueOutcome()
            //{
            //    NextStep = newStep.Id,
            //    Value = endExpr
            //});
            AddInputParams(jobScheduleStep, "JobSchedule", id, name);
            //Expression<Func<TOAJobForeachStepBody, IEnumerable>> inputExpr = (x => x.Collection);
            //Expression<Func<ProcInstContext, IEnumerable>> collection = (data => (IList<UBJob>)data.GetValue("nextJobList"));
            //jforStep.Inputs.Add(new MemberMapParameter(collection, inputExpr));

            //jobStepBody 取人的逻辑
            var jobStep = new WorkflowStep<JobStepBody>();
            builder.WorkflowBuilder.AddStep(jobStep);
            AddInputParams(jobStep, "Job", id, name);
            //Expression<Func<JobStepBody, UBJob>> jobExpr = (x => x.Job);
            //Expression<Func<ProcInstContext, IStepExecutionContext, UBJob>> job = ((data, context) => (UBJob)context.Item);
            //jobStep.Inputs.Add(new MemberMapParameter(job, jobExpr));
            jobScheduleStep.Children.Add(jobStep.Id);

            //分配任务
            var destStep = new WorkflowStep<DestStepBody>();
            builder.WorkflowBuilder.AddStep(destStep);
            AddInputParams(destStep, "Dest", id, name);
            jobStep.Children.Add(destStep.Id);

            //job完成后，重新回到TOA Cal
            ValueOutcome result = new ValueOutcome
            {
                NextStep = newStep.Id
            };
            jobStep.Outcomes.Add(result);

            var stepBuilder = new StepBuilder<TData, TOAJobsScheduleStepBody>(builder.WorkflowBuilder, jobScheduleStep);
          
            return stepBuilder;
        }

        private static void AddInputParams(WorkflowStep step, string prefix, string TOAId, string TOAName)
        {
            //input表达式必须要带有输入参数。
            string stepName = prefix + "_" + TOAName;
            step.Name = stepName;
            Expression<Func<object, string>> nameValueExpr = (x => stepName);
            Expression<Func<UBStepBody, string>> nameExpr = (x => x.StageName);
            step.Inputs.Add(new MemberMapParameter(nameValueExpr, nameExpr));

            Expression<Func<object, string>> actValueExpr = (x => TOAName);
            Expression<Func<UBStepBody, string>> actExpr = (x => x.ActName);
            step.Inputs.Add(new MemberMapParameter(actValueExpr, actExpr));

            Expression<Func<object, string>> IdValueExpr = (x => TOAId);
            Expression<Func<UBStepBody, string>> IdExpr = (x => x.ActID);
            step.Inputs.Add(new MemberMapParameter(IdValueExpr, IdExpr));

            string actType = ActType.TOA.ToString();
            Expression<Func<object, string>> actTypeValueExpr = (x => actType) ;
            Expression<Func<UBStepBody, string>> actTypeExpr = (x => x.SActType);
            step.Inputs.Add(new MemberMapParameter(actTypeValueExpr, actTypeExpr));
        }
    }
}
