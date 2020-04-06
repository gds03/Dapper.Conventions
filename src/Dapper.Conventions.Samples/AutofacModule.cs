using Autofac;
using Dapper.Conventions.Samples.Services.Queries;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Dapper.Conventions.Samples
{
    class AutofacModule : Module
    {
        const string ConnectionString = "Data Source=localhost,1433;Initial Catalog=Test;User Id=sa;Password=Summer2020";

        protected override void Load(ContainerBuilder builder)
        {
           
            //builder.RegisterType<OrderQueriesWithConventionsAnother>().AsSelf();
            builder.RegisterType<Application>().AsImplementedInterfaces();
            builder.RegisterType<OrderQueriesWithConventions>().AsImplementedInterfaces();            

            builder.RegisterGeneric(typeof(ConventionsCachedLookup<>)).AsImplementedInterfaces()
                 .WithParameter(
                        (pi, ctx) => pi.Name == "baseDirectory",
                        (pi, ctx) => "SQLScripts"
                    ) 
                .WithParameter(
                        (pi, ctx) => pi.Name == "fileExtensions",
                        (pi, ctx) => "sql"
                    )

                 .SingleInstance();

            builder.RegisterGeneric(typeof(QueryExecutor<>)).AsImplementedInterfaces()
                 .WithParameter(
                        (pi, ctx) => pi.ParameterType == typeof(Func<IDbConnection>),
                        (pi, ctx) => { Func<IDbConnection> factory = () => new SqlConnection(ConnectionString); return factory; }
                    )
                 .SingleInstance();

        }

    }
}
