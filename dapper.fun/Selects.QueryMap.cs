using Dapper;
using System;
using System.Collections.Generic;

namespace dapper.fun
{
    using static dapper.fun.Transforms;
    public delegate R Map<X, R>(X x);

    public partial class Selects
    {
        //TODO:
        public static Select<P, IEnumerable<R>> QueryMap<P, R>(
            QueryString query,
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
        public static Select<IEnumerable<R>> QueryMap<R>(QueryString query,
            Type[] types,
            Map<object[], R> map,
            string splitOn = "id")
        {
            return NoParams(QueryMap<object, R>(query, types, map, splitOn));
        }
        public static Select<IEnumerable<object>> QueryMap(QueryString query,
            Type[] types,
            Map<object[], object> map,
            string splitOn = "id")
        {
            return NoParams(QueryMap<object, object>(query, types, map, splitOn));
        }
        public static Map<X, R> MakeMap<X, R>(Map<X, R> map)
        {
            return map;
        }
        public static Map<object[], R> MakeMap<R>(Map<object[], R> map)
        {
            return map;
        }
        public static Map<X, R2> MakeMap<X, R1, R2>(Map<X, R1> map1, Map<R1, R2> map2) => x => map2(map1(x));
        public static Map<object[], R2> MakeMap<R1, R2>(Map<object[], R1> map1, Map<R1, R2> map2) => MakeMap<object[], R1, R2>(map1, map2);
        ///<sumary>
        ///
        ///</sumary>
        public static Map<object[], R3> MakeMap<R1, R2, R3>(
            Map<object[], R1> map1, Map<R1, R2> map2, Map<R2, R3> map3) =>
                MakeMap(MakeMap<object[], R1, R2>(map1, map2), map3);
        ///<sumary>
        ///
        ///</sumary>
        public static Map<object[], R4> MakeMap<R1, R2, R3, R4>(
            Map<object[], R1> map1, Map<R1, R2> map2, Map<R2, R3> map3, Map<R3, R4> map4) =>
                MakeMap(MakeMap(map1, map2, map3), map4);
        ///<sumary>
        ///
        ///</sumary>
        public static Map<object[], R5> MakeMap<R1, R2, R3, R4, R5>(
            Map<object[], R1> map1, Map<R1, R2> map2, Map<R2, R3> map3, Map<R3, R4> map4, Map<R4, R5> map5) =>
                MakeMap(MakeMap<R1, R2, R3, R4>(map1, map2, map3, map4), map5);
        ///<sumary>
        ///
        ///</sumary>
        public static Map<object[], R6> MakeMap<R1, R2, R3, R4, R5, R6>(
            Map<object[], R1> map1, Map<R1, R2> map2, Map<R2, R3> map3, Map<R3, R4> map4, Map<R4, R5> map5, Map<R5, R6> map6) =>
               MakeMap(MakeMap<R1, R2, R3, R4, R5>(map1, map2, map3, map4, map5), map6);
        public static Map<object[], R7> MakeMap<R1, R2, R3, R4, R5, R6, R7>(
            Map<object[], R1> map1, Map<R1, R2> map2, Map<R2, R3> map3, Map<R3, R4> map4, Map<R4, R5> map5, Map<R5, R6> map6, Map<R6, R7> map7) =>
               MakeMap(MakeMap<R1, R2, R3, R4, R5, R6>(map1, map2, map3, map4, map5, map6), map7);
        ///<summar>
        ///
        ///</summar>
        public static Select<P, IEnumerable<R>> QueryMap<P, T1, T2, R>(
            QueryString query,
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
        public static Select<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, R>(
            QueryString query,
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
        public static Select<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, T4, R>(
           QueryString query,
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
        public static Select<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, T4, T5, R>(
            QueryString query,
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
        public static Select<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, T4, T5, T6, R>(
            QueryString query,
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
        public static Select<P, IEnumerable<R>> QueryMap<P, T1, T2, T3, T4, T5, T6, T7, R>(
            QueryString query,
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