namespace dapper.fun
{
    using System;

    public delegate T Change<T>(T i);
    public delegate Change<T> ChangeFty<T>(T i);

    public class Transforms
    {
        public static Func<ChangeFty<string>, Func<string, Func<string, Select<R>>>> ChangeQuery<R>(SelectFty<R> select)
        {
            return transform => query => input => select(transform(input)(query));
        }
        public static Func<ChangeFty<string>, Func<string, Func<string, Select<P, R>>>> ChangeQuery<P, R>(SelectFty<P, R> select)
        {
            return transform => query => input => select(transform(input)(query));
        }
        public static Select<O, R> ChangeParameters<P, R, O>(Select<P, R> select, Func<O, P> transform){
            return (con, tran) => (value) => select(con, tran)(transform(value));
        }
    }
}