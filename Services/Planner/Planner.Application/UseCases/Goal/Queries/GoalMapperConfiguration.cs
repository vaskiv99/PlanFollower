using Mapster;
using Planner.Application.Common.Interfaces;
using Planner.Domain.AggregatesModel.GoalAggregate.ValueObjects;

namespace Planner.Application.UseCases.Goal.Queries
{
    public class GoalMapperConfiguration : IMapperConfiguration
    {
        public void ConfigureMappings()
        {
            TypeAdapterConfig<Domain.AggregatesModel.GoalAggregate.Entities.Goal, GoalView>.NewConfig()
                .Map(dest => dest.Frequency, src => new Frequency(src.Frequency));

            TypeAdapterConfig<Domain.AggregatesModel.GoalAggregate.Entities.Goal, ExtendedGoalView>.NewConfig()
                .Map(dest => dest.Frequency, src => new Frequency(src.Frequency));
        }
    }
}