using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Dapper.Conventions.Interfaces
{
    public interface IConventionsLookup<TUsingConventions>
    {
        string GetQuery([CallerMemberName] string methodName = null);
    }    
}
