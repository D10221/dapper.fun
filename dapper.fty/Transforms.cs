namespace dapper.fty
{
    using System;

    public delegate T Change<T>(T i);
    public delegate Change<T> ChangeFty<T>(T i);

    public class Transforms
    {
        public static Func<ChangeFty<string>, Func<string, Func<string, Select<R>>>> Change<R>(SelectFty<R> select)
        {
            return transform => query => input => select(transform(input)(query));
        }
        public static Func<ChangeFty<string>, Func<string, Func<string, Select<P, R>>>> Change<P, R>(SelectFty<P, R> select)
        {
            return transform => query => input => select(transform(input)(query));
        }
    }
}