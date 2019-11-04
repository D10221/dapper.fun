using System.Threading.Tasks;

namespace dapper.fun.dam.test
{
    using System.Data;

    public static class Extensions
    {
        public static Task<R> Run<P, R>(this DbCommand<P, R> select, P p, IDbConnection connection, IDbTransaction transaction = null)
        {
            return select(connection, transaction)(p);
        }
        public static Task<R> Run<R>(this DBCommand<R> select, IDbConnection connection, IDbTransaction transaction = null)
        {
            return select(connection, transaction)();
        }
    }
}
