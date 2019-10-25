namespace dapper.fun
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Operations;
    using static Transforms;

    public class Queries
    {
        public static ChangeFty<string> WithWhere = w => query => $"{query} WHERE {w}";

        public static Func<string, Select<IEnumerable<R>>> Where<R>(string sql)
        {
            return ChangeQuery(Query<R>)(WithWhere)(sql);
        }
        public static Func<string, Select<P, IEnumerable<R>>> Where<P, R>(string sql)
        {
            return ChangeQuery(Query<P, R>)(WithWhere)(sql);
        }
        public static Select<P, R> QuerySingle<P, R>(string query)
        {
            return (con, tran) => async p => {
                var r = ( await Query<P, R>(query)(con, tran)(p));
                if(r.Count() > 1) {
                    // TODO: hides querie intention ? 
                    throw new Exception("You are expectin 1 but got many");
                }
                return r.FirstOrDefault();
            };
        }
        public static Select<R> QuerySingle<R>(string query)
        {
            return (con, tran) => async () => {
                var r = ( await Query<R>(query)(con, tran)());
                if(r.Count() > 1) {
                    // TODO: hides querie intention ? 
                    throw new Exception("You are expectin 1 but got many");
                }
                return r.FirstOrDefault();
            };
        }
    }
}