using System.Runtime.CompilerServices;

namespace Dapper.Conventions.Interfaces
{
    public interface IConventionsLookup<TUsingConventions>
    {
        string GetCommandFor([CallerMemberName] string methodName = null);
    }    
}
