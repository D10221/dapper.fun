using Dapper;
using System;
using System.Collections.Generic;

namespace dapper.fun
{
    ///<summary>
    /// 
    ///</summary>
    public partial class Selects
    {
        public static DbCommand<P, int> Exec<P>(CommandSource query)
        {
            return (connection, transaction) => (param) => SqlMapper.ExecuteAsync(
                connection,
                query.AsCommandDefinition(AutoName(param), transaction)
                );
        }
        public static DbCommand<P, R> Scalar<P, R>(CommandSource query)
        {
            return (connection, transaction) => (param) => SqlMapper.ExecuteScalarAsync<R>(
                connection,
                query.AsCommandDefinition(AutoName(param), transaction)
                );
        }
        public static DbCommand<P, IEnumerable<R>> Query<P, R>(CommandSource query)
        {
            return (connection, transaction) => (param) => SqlMapper.QueryAsync<R>(
                connection, query.AsCommandDefinition(AutoName(param), transaction)
                );
        }
        public static DbCommand<P, R> QueryFirst<P, R>(CommandSource query)
        {
            return (connection, transaction) => (param) => SqlMapper.QueryFirstAsync<R>(
                connection, query.AsCommandDefinition(AutoName(param), transaction)
                );
        }
        public static DbCommand<P, R> QueryFirstOrDefault<P, R>(CommandSource query)
        {
            return (connection, transaction) => (param) => SqlMapper.QueryFirstOrDefaultAsync<R>(
                connection, query.AsCommandDefinition(AutoName(param), transaction)
                );
        }
        public static DbCommand<P, R> QuerySingle<P, R>(CommandSource query)
        {
            return (connection, transaction) => (param) => SqlMapper.QuerySingleAsync<R>(
                connection, query.AsCommandDefinition(AutoName(param), transaction));
        }
        public static DbCommand<P, R> QuerySingleOrDefault<P, R>(CommandSource query)
        {
            return (connection, transaction) => (param) => SqlMapper.QuerySingleOrDefaultAsync<R>(
                connection, query.AsCommandDefinition(AutoName(param), transaction)
                );
        }
        public static object AutoName<P>(P param)
        {
            switch (typeof(P))
            {
                case Type p when p.IsPrimitive: return new { param };
                case Type p when p.IsValueType: return new { param };
                case Type p when p == typeof(string): return new { param };
                case Type p when p == typeof(byte[]): return new { param };
                default: return param;
            }
        }
    }
}