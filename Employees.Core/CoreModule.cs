using Autofac;
using Employees.Core.Coworking;
using Employees.Core.Coworking.Interfaces;

namespace Employees.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register services and dependencies
            builder.RegisterType<TopCoworkersProcessor>().As<ITopCoworkers>().SingleInstance();
            builder.RegisterType<AllCoworkersProcessor>().As<IAllCoworkers>().SingleInstance();
            builder.RegisterType<CoworkersPreProcessor>().As<ICoworkersPreProcessor>().SingleInstance();
        }
    }
}
