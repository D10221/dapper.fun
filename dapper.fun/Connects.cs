using System.Data;

namespace dapper.fun
{
    using System;
    using System.Threading.Tasks;

    public class Connects
    {
        public static Func<IDbConnection, Command<P, R>> Connect<P, R>(DbCommand<P, R> select)
        {
            return (connection) => select(connection, null);
        }
        public static Func<IDbConnection, Command<R>> Connect<R>(DBCommand<R> select)
        {
            return (connection) => select(connection, null);
        }
        public static Command<P, R> Connect<P, R>(DbCommand<P, R> select, IDbConnection connection, IDbTransaction transaction = null)
        {
            return select(connection, transaction);
        }
        public static Command<R> Connect<R>(DBCommand<R> select, IDbConnection connection, IDbTransaction transaction = null)
        {
            return select(connection, transaction);
        }        
    }
}