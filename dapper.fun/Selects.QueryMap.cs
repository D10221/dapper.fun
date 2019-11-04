using Dapper;
using System;
using System.Collections.Generic;

namespace dapper.fun
{
    using static dapper.fun.Transforms;  

    public partial class Selects
    {
        //TODO:
        public static DbCommand<P, IEnumerable<R>> QueryMap<P, R>(
            CommandSource query,
            Type[] types,
            Map<object[], R> map,
            string splitOn = "id") =>
            (con, tran) => (p) => SqlMapper.QueryAsync<R>(
                con, query,
                types: types,
                map: objs => map(objs),
                param: p,
                transaction: tran,
                buffered: query.Buffered,
                splitOn: splitOn
                );
        public static DBCommand<IEnumerable<R>> QueryMap<R>(CommandSource query,
            Type[] types,
            Map<object[], R> map,
            string splitOn = "id")
        {
            return NoParams(QueryMap<object, R>(query, types, map, splitOn));
        }
        public static DBCommand<IEnumerable<object>> QueryMap(CommandSource query,
            Type[] types,
            Map<object[], object> map,
            string splitOn = "id")
        {
            return NoParams(QueryMap<object, object>(query, types, map, splitOn));
        }
        
        ///<summar>
        ///
        ///</summar>
        public static DbCommand<P, IEnumerable<R>> QueryMap<P, T1, T2, R>(
            CommandSource query,
            Func<T1, T2, R> map,
            string splitOn = "id") =>
            (con, transaction) => (param) => SqlMapper.QueryAsync<T1, T2, R>(
                con, query,
                 map,
                param,
                transaction,
                buffered: query.Buffered,
                splitOn: splitOn
                );
        ///<summar>
        ///
        ///</summar>
        public static DbCommand<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, R>(
            CommandSource query,
            Func<T1, T2, T3, R> map,
            string splitOn = "id") =>
            (con, transaction) => (param) => SqlMapper.QueryAsync<T1, T2, T3, R>(
                con, query,
                 map,
                param,
                transaction,
                buffered: query.Buffered,
                splitOn: splitOn
                );
        ///<summar>
        ///
        ///</summar>
        public static DbCommand<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, T4, R>(
           CommandSource query,
           Func<T1, T2, T3, T4, R> map,
           string splitOn = "id") =>
           (con, transaction) => (param) => SqlMapper.QueryAsync<T1, T2, T3, T4, R>(
               con, query,
                map,
               param,
               transaction,
               buffered: query.Buffered,
               splitOn: splitOn
               );
        ///<summar>
        ///
        ///</summar>
        public static DbCommand<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, T4, T5, R>(
            CommandSource query,
            Func<T1, T2, T3, T4, T5, R> map,
            string splitOn = "id") =>
            (con, transaction) => (param) => SqlMapper.QueryAsync<T1, T2, T3, T4, T5, R>(
                con, query,
                 map,
                param,
                transaction,
                buffered: query.Buffered,
                splitOn: splitOn
                );
        ///<summar>
        ///
        ///</summar>
        public static DbCommand<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, T4, T5, T6, R>(
            CommandSource query,
            Func<T1, T2, T3, T4, T5, T6, R> map,
            string splitOn = "id") =>
            (con, transaction) => (param) => SqlMapper.QueryAsync<T1, T2, T3, T4, T5, T6, R>(
                con, query,
                 map,
                param,
                transaction,
                buffered: query.Buffered,
                splitOn: splitOn
                );
        ///<summar>
        ///
        ///</summar>
        public static DbCommand<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, T4, T5, T6, T7, R>(
            CommandSource query,
            Func<T1, T2, T3, T4, T5, T6, T7, R> map,
            string splitOn = "id") =>
            (con, transaction) => (param) => SqlMapper.QueryAsync<T1, T2, T3, T4, T5, T6, T7, R>(
                con, query,
                 map,
                param,
                transaction,
                buffered: query.Buffered,
                splitOn: splitOn
                );
    }
}