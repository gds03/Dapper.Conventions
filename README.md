# Dapper.Conventions
Instead of having hardcoded SQL into C# code, Conventions uses Type and Method naming for lookup and discovers typically "sql" files on the disk and caches them. A project with sample is included how to use it.


Example usage:

```csharp

[UseConventions("Query/Orders")]
public class OrderQueriesWithConventions : IOrderQueries
{
    private IQueryExecutor<OrderQueriesWithConventions> conventionsQuery;

    public OrderQueriesWithConventions(IQueryExecutor<OrderQueriesWithConventions> conventionsQuery)
    {
        this.conventionsQuery = conventionsQuery;
    }


    public IEnumerable<OrderDetails> GetAll() => 
        conventionsQuery.Run((sql, conn) => conn.Query<OrderDetails>(sql)
    );


    [OverrideConventions("GetHigherThan200")]
    public IEnumerable<OrderDetails> GetComplexFiltered(string partOfDetails) =>
        conventionsQuery.Run((sql, conn) => conn.Query<OrderDetails>(sql)
    );

    public OrderDetails GetSingle(int id) => 
        conventionsQuery.Run((sql, conn) => conn.QuerySingle<OrderDetails>(sql, new { Id = id })
    );
}
