using Autofac;
using Dapper.Conventions.Domain;
using Dapper.Conventions.Interfaces;
using Dapper.Conventions.Samples.Interfaces;
using Dapper.Conventions.Samples.Services.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Conventions.Samples
{
    class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<ConfigService>().AsImplementedInterfaces().SingleInstance();
            //builder.RegisterType<MappingService>().AsImplementedInterfaces();
            //builder.RegisterType<CsvProcessorService<CsvEntry>>().As<ICsvProcessorService<CsvEntry>>()
            //    .WithParameter(
            //        (pi, ctx) => pi.Name == "filePath" && pi.ParameterType == typeof(string),
            //        (pi, ctx) => ctx.Resolve<IConfigService>().FilePath
            //    );


            builder.RegisterType<Application>().AsImplementedInterfaces();
            builder.RegisterType<OrderQueriesWithConventions>().AsImplementedInterfaces();
            builder.RegisterType<SqlFileExtension>().AsImplementedInterfaces();

            //builder
            //    .RegisterType<ConventionsLookup<OrderQueriesWithConventions>>().As<IConventionsLookup<OrderQueriesWithConventions>>()
            //        .WithParameter(
            //            (pi, ctx) => pi.Name == "baseDirectory",
            //            (pi, ctx) => "SQLScripts"
            //        )
            //    .SingleInstance();

            builder.RegisterGeneric(typeof(ConventionsCachedLookup<>)).AsImplementedInterfaces()
                 .WithParameter(
                        (pi, ctx) => pi.Name == "baseDirectory",
                        (pi, ctx) => "SQLScripts"
                    )
                 .InstancePerDependency();
        }

    }
}
