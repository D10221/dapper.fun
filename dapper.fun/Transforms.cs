namespace dapper.fun
{
    using System;
    using System.Threading.Tasks;
    public class Transforms
    {
        public static DBCommand<R> NoParams<P, R>(DbCommand<P, R> select)
        {
            return (connection, transaction) => () => select(connection, transaction)(default(P));
        }
        public static DbCommand<O, R> ChangeParam<P, R, O>(DbCommand<P, R> select, Func<O, P> transform)
        {
            return (con, tran) => (value) => select(con, tran)(transform(value));
        }
        public static DbCommand<P, X> ChangeResult<P, R, X>(DbCommand<P, R> select, Func<Task<R>, Task<X>> transform)
        {
            return (con, tran) => (p) => transform(select(con, tran)(p));
        }
    }
}