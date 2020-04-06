using System.Runtime.CompilerServices;

namespace Dapper.Conventions.Interfaces
{
    public interface IConventionsLookup<TUsingConventions>
    {
        string GetQuery([CallerMemberName] string methodName = null);
    }    
}
