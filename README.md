# Dapper.Conventions
Instead of having hardcoded SQL into C# code, Conventions uses Type and Method naming for lookup and discovers typically "sql" files on the disk and caches them. A project with sample is included how to use it.


Example usage:

```csharp

[UseConventions("Query/Orders")]
    public class OrderQueriesWithConventions : IOrderQueries
    {
        private IQueryExecutor<OrderQueriesWithConventions> conventions;

        public OrderQueriesWithConventions(IQueryExecutor<OrderQueriesWithConventions> conventions)
        {
            this.conventions = conventions;
        }


        public IEnumerable<OrderDetails> GetAll() =>    //a
            conventions._(sql =>
            {
                using (var conn = new SqlConnection())
                {
                    return conn.Query<OrderDetails>(sql);
                }
            }); 


        [OverrideConventions("GetHigherThan200")]
        public IEnumerable<OrderDetails> GetComplexFiltered(string partOfDetails) => //b
            conventions._((sql, conn) => conn.Query<OrderDetails>(sql));

        public OrderDetails GetSingle(int id) => 
            conventions._((sql, conn) => conn.QuerySingle<OrderDetails>(sql, new { Id = id })); //c
    }
```

This will lookup in the Query\Orders folder for a file for each method that queries. 

At a) it will get the default name of Get all -> a file with the contents on: Query\Orders\Getall.sql will be loaded
At b) it will get the overwritten name of GetComplexFiltered which is GetHigherThan200 -> a file with the contents on: Query\Orders\GetHigherThan200.sql will be loaded
At c) it will get the default name of GetSingle -> a file with the contents on: Query\Orders\GetSingle.sql will be loaded

When you inject the parameter into the class is when the mapping is done and is only done once because each IQueryExecutor<T> is a Singleton.


To Register the following Services if you are using AutoFac you can register with the following calls:

```csharp
class AutofacModule : Module
{
    const string ConnectionString = "your_CS";

    protected override void Load(ContainerBuilder builder)
    {
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

```
