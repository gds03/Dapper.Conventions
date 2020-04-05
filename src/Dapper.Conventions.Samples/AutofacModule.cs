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
           
            //builder.RegisterType<OrderQueriesWithConventionsAnother>().AsSelf();
            builder.RegisterType<Application>().AsImplementedInterfaces();
            builder.RegisterType<OrderQueriesWithConventions>().AsImplementedInterfaces();
            builder.RegisterType<SqlFileExtension>().AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(ConventionsCachedLookup<>)).AsImplementedInterfaces()
                 .WithParameter(
                        (pi, ctx) => pi.Name == "baseDirectory",
                        (pi, ctx) => "SQLScripts"
                    )
                 .SingleInstance();
        }

    }
}
