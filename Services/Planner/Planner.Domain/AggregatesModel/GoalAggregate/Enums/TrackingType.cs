namespace Planner.Domain.AggregatesModel.GoalAggregate.Enums
{
    /// <summary>
    /// Represent progress tracking type
    /// </summary>
    public enum TrackingType
    {
        /// <summary>
        /// Mean that progress of achieving the goal, will calculating from entered reports in percentage 
        /// </summary>
        Percentage,

        /// <summary>
        /// Mean that progress of achieving the goal, will calculating from entered reports in 'Abstract Goal Value' 
        /// </summary>
        AbstractGoalValue
    }
}