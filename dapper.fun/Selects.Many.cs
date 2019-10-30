using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dapper.fun
{
    using static dapper.fun.Transforms;
    public partial class Selects
    {
        /**
         *  // Sample 
            select * from Customers where CustomerId = @id
            select * from Orders    where CustomerId = @id
            select * from Returns   where CustomerId = @id
         */
        public static Select<object, SqlMapper.GridReader> SelectMany(QueryString query)
        {
            return (con, tran) => (p) => SqlMapper.QueryMultipleAsync(
                    cnn: con,
                    sql: query,
                    param: AutoName(p),
                    transaction: tran,
                    commandTimeout: query.CommandTimeout,
                    commandType: query.CommandType);
        }        
        static Func<Task<SqlMapper.GridReader>, Task<R>> WithReader<R>(Func<SqlMapper.GridReader, R> fun)
        {
            return async results => fun((await results));
        }
        public static Select<object, (IEnumerable<T1>, IEnumerable<T2>)> SelectMany<T1, T2>(QueryString query) =>
            ChangeResult(SelectMany(query),
                WithReader(reader => (reader.Read<T1>(), reader.Read<T2>())));
        public static Select<object, (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> SelectMany<T1, T2, T3>(QueryString query) =>
            ChangeResult(SelectMany(query),
                WithReader(reader => (reader.Read<T1>(), reader.Read<T2>(), reader.Read<T3>())));
        public static Select<object, (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>)> SelectMany<T1, T2, T3, T4>(QueryString query) =>
               ChangeResult(SelectMany(query),
                   WithReader(reader => (reader.Read<T1>(), reader.Read<T2>(), reader.Read<T3>(), reader.Read<T4>())));
        public static Select<object, (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>)> SelectMany<T1, T2, T3, T4, T5>(QueryString query) =>
               ChangeResult(SelectMany(query),
                   WithReader(reader => (reader.Read<T1>(), reader.Read<T2>(), reader.Read<T3>(), reader.Read<T4>(), reader.Read<T5>())));
        public static Select<object, (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>)> SelectMany<T1, T2, T3, T4, T5, T6>(QueryString query) =>
               ChangeResult(SelectMany(query),
                   WithReader(reader => (reader.Read<T1>(), reader.Read<T2>(), reader.Read<T3>(), reader.Read<T4>(), reader.Read<T5>(), reader.Read<T6>())));
        public static Select<object, (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>)> SelectMany<T1, T2, T3, T4, T5, T6, T7>(QueryString query) =>
                       ChangeResult(SelectMany(query),
                           WithReader(reader => (reader.Read<T1>(), reader.Read<T2>(), reader.Read<T3>(), reader.Read<T4>(), reader.Read<T5>(), reader.Read<T6>(), reader.Read<T7>())));
    }
}