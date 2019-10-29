using System.Collections.Generic;

namespace dapper.fun
{
    using static Transforms;
    public partial class Selects
    {
        public static Select<IEnumerable<dynamic>> Query(QueryString query)=> NoParams(Query<object,dynamic>(query));
        public static Select<R> QueryFirstOrDefault<R>(QueryString query) => NoParams(QueryFirstOrDefault<object, R>(query));
        public static Select<int> Exec(QueryString query) => NoParams(Exec<object>(query));
        public static Select<R> Scalar<R>(QueryString query) => NoParams(Scalar<object, R>(query));
        public static Select<IEnumerable<R>> Query<R>(QueryString query) => NoParams(Query<object, R>(query));
        public static Select<R> QueryFirst<R>(QueryString query) => NoParams(QueryFirst<object, R>(query));
        public static Select<R> QuerySingle<R>(QueryString query) => NoParams(QuerySingle<object, R>(query));
        public static Select<R> QuerySingleOrDefault<R>(QueryString query) => NoParams(QuerySingleOrDefault<object, R>(query));
    }
}