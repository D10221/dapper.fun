using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public static Map<X, R2> MakeMap<X, R1, R2>(Map<X, R1> map1, Map<R1, R2> map2)
        {
            return x => map2(map1(x));
        }
        public static Map<object[], R2> MakeMap<R1, R2>(Map<object[], R1> map1, Map<R1, R2> map2)=>  MakeMap<object[], R1, R2>(map1, map2);

    }
}