using System;
using System.Data;
using System.Threading.Tasks;

namespace dapper.fun
{    
    public class Middleware
    {
        public static DbCommand<P,R> Create<P, R>(Func<IDbConnection, IDbTransaction,P, Task<R>> run) 
                => (c,t) => p => run(c,t,p);
    }
}