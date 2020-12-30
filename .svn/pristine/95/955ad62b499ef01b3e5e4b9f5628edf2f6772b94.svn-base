using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    public interface IStepOutcome
    {
        string ExternalNextStepId { get; set; }
        string Label { get; set; }
        int NextStep { get; set; }

        bool Matches(object data);
        /// <summary>
        /// ExecutionResult.OutcomeValue 与data的表达式返回值的比对
        /// </summary>
        /// <param name="executionResult"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Matches(ExecutionResult executionResult, object data);
    }
}