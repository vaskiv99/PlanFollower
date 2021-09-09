using System.Collections.Generic;

namespace Planner.Application.UseCases.Goal.Queries
{
    public class ExtendedGoalView : GoalView
    {
        public List<GoalStatusItemView> Items { get; set; }

        public List<ReportView> Reports { get; set; }
    }
}