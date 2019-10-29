namespace dapper.fun
{
    using System;
    using System.Threading.Tasks;
    public class Transforms
    {
        public static Select<R> NoParams<P, R>(Select<P, R> select)
        {
            return (connection, transaction) => () => select(connection, transaction)(default(P));
        }
        public static Select<O, R> ChangeParam<P, R, O>(Select<P, R> select, Func<O, P> transform)
        {
            return (con, tran) => (value) => select(con, tran)(transform(value));
        }
        public static Select<P, X> ChangeResult<P, R, X>(Select<P, R> select, Func<Task<R>, Task<X>> transform)
        {
            return (con, tran) => (p) => transform(select(con, tran)(p));
        }
    }
}