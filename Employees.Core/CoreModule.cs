using Autofac;
using Employees.Core.Interfaces;

namespace Employees.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register services and dependencies
            builder.RegisterType<CoworkersPreProcessor>();
            builder.RegisterType<AllCoworkersProcessor>().As<IAllCoworkers>();
        }
    }
}
