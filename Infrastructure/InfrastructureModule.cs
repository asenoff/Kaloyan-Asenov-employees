using Autofac;
using Employees.Core.Interfaces;

namespace Employees.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register services and dependencies
            builder.RegisterType<MockDataProcessor>().As<IDataProcessor>();
        }
    }
}
