using Autofac;
using CsvHelper;
using Employees.Core.Interfaces;

namespace Employees.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register services and dependencies
            builder.RegisterType<CSVDataProcessor>().As<IDataProcessor>();
        }
    }
}
