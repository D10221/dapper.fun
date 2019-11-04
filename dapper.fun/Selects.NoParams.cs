using System.Collections.Generic;

namespace dapper.fun
{
    using static Transforms;
    public partial class Selects
    {
        public static DBCommand<IEnumerable<dynamic>> Query(CommandSource query)=> NoParams(Query<object,dynamic>(query));
        public static DBCommand<R> QueryFirstOrDefault<R>(CommandSource query) => NoParams(QueryFirstOrDefault<object, R>(query));
        public static DBCommand<int> Exec(CommandSource query) => NoParams(Exec<object>(query));
        public static DBCommand<R> Scalar<R>(CommandSource query) => NoParams(Scalar<object, R>(query));
        public static DBCommand<IEnumerable<R>> Query<R>(CommandSource query) => NoParams(Query<object, R>(query));
        public static DBCommand<R> QueryFirst<R>(CommandSource query) => NoParams(QueryFirst<object, R>(query));
        public static DBCommand<R> QuerySingle<R>(CommandSource query) => NoParams(QuerySingle<object, R>(query));
        public static DBCommand<R> QuerySingleOrDefault<R>(CommandSource query) => NoParams(QuerySingleOrDefault<object, R>(query));
    }
}