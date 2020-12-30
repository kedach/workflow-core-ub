﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Models.LifeCycleEvents;

namespace WorkflowCore.Services.ErrorHandlers
{
    public class CompensateHandler : IWorkflowErrorHandler
    {
        private readonly ILifeCycleEventPublisher _eventPublisher;
        private readonly IExecutionPointerFactory _pointerFactory;
        private readonly IDateTimeProvider _datetimeProvider;
        private readonly WorkflowOptions _options;

        public WorkflowErrorHandling Type => WorkflowErrorHandling.Compensate;

        public CompensateHandler(IExecutionPointerFactory pointerFactory, ILifeCycleEventPublisher eventPublisher, IDateTimeProvider datetimeProvider, WorkflowOptions options)
        {
            _pointerFactory = pointerFactory;
            _eventPublisher = eventPublisher;
            _datetimeProvider = datetimeProvider;
            _options = options;
        }

        public void Handle(WorkflowInstance workflow, WorkflowDefinition def, ExecutionPointer exceptionPointer, WorkflowStep exceptionStep, Exception exception, Queue<ExecutionPointer> bubbleUpQueue)
        {
            var scope = new Stack<string>(exceptionPointer.Scope.Reverse());
            scope.Push(exceptionPointer.Id);
            
            while (scope.Any())
            {
                var pointerId = scope.Pop();
                var scopePointer = workflow.ExecutionPointers.FindById(pointerId);
                var scopeStep = def.Steps.FindById(scopePointer.StepId);

                var resume = true;
                var revert = false;
                
                var txnStack = new Stack<string>(scope.Reverse());
                while (txnStack.Count > 0)
                {
                    var parentId = txnStack.Pop();
                    var parentPointer = workflow.ExecutionPointers.FindById(parentId);
                    var parentStep = def.Steps.FindById(parentPointer.StepId);
                    if ((!parentStep.ResumeChildrenAfterCompensation) || (parentStep.RevertChildrenAfterCompensation))
                    {
                        resume = parentStep.ResumeChildrenAfterCompensation;
                        revert = parentStep.RevertChildrenAfterCompensation;
                        break;
                    }
                }

                if ((scopeStep.ErrorBehavior ?? WorkflowErrorHandling.Compensate) != WorkflowErrorHandling.Compensate)
                {
                    bubbleUpQueue.Enqueue(scopePointer);
                    continue;
                }

                scopePointer.Active = false;
                scopePointer.EndTime = _datetimeProvider.UtcNow;
                scopePointer.Status = PointerStatus.Failed;

                if (scopeStep.CompensationStepId.HasValue)
                {
                    scopePointer.Status = PointerStatus.Compensated;

                    var compensationPointer = _pointerFactory.BuildCompensationPointer(def, scopePointer, exceptionPointer, scopeStep.CompensationStepId.Value);
                    workflow.ExecutionPointers.Add(compensationPointer);

                    if (resume)
                    {
                        foreach (var outcomeTarget in scopeStep.Outcomes.Where(x => x.Matches(workflow.Data)))
                            workflow.ExecutionPointers.Add(_pointerFactory.BuildNextPointer(def, scopePointer, outcomeTarget));
                    }
                }
                //一般逻辑 补偿用来做回滚，所以在发生错误时，回滚相同范围内兄弟节点的逻辑
                if (revert)
                {
                    //已经执行完成的兄弟节点
                    var prevSiblings = workflow.ExecutionPointers
                        .Where(x => scopePointer.Scope.SequenceEqual(x.Scope) && x.Id != scopePointer.Id && x.Status == PointerStatus.Complete)
                        .OrderByDescending(x => x.EndTime)
                        .ToList();
                    //出发兄弟节点的补偿
                    foreach (var siblingPointer in prevSiblings)
                    {
                        var siblingStep = def.Steps.FindById(siblingPointer.StepId);
                        if (siblingStep.CompensationStepId.HasValue)
                        {
                            var compensationPointer = _pointerFactory.BuildCompensationPointer(def, siblingPointer, exceptionPointer, siblingStep.CompensationStepId.Value);
                            workflow.ExecutionPointers.Add(compensationPointer);
                            siblingPointer.Status = PointerStatus.Compensated;
                        }
                    }
                }
            }
        }
    }
}