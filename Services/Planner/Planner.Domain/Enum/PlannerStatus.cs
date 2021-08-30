namespace Planner.Domain.Enum
{
    /// <summary>
    /// Represent statuses for planner & goal
    /// </summary>
    public enum PlannerStatus
    {
        PendingStart,
        InProgress,
        Postponed,
        Stopped,
        Completed
    }
}