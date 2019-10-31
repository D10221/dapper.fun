using System.Collections.Generic;
using System.Data;

namespace dapper.fun.dam
{
    using static dapper.fun.Selects;

    public delegate (
            Select<IEnumerable<T>> all,
            Select<int> create,
            Select<int> drop,
            Select<int, T> find,
            Select<T, int> insert,
            Select<T, int> update) GetDam<T>();


    public delegate (
                Selector<IEnumerable<T>> all,
                Selector<int> create,
                Selector<int> drop,
                Selector<int, T> find,
                Selector<T, int> insert,
                Selector<T, int> update)
                GetConnected<T>();
    public class Dam
    {
        public static GetDam<T> Create<T>(Queries queries) => () =>
        {
            var (all, create, drop, find, insert, update) = queries;
            return (
                all: Query<T>(all),
                create: Exec(create),
                drop: Exec(drop),
                find: QuerySingle<int, T>(find),
                insert: Exec<T>(insert),
                update: Exec<T>(update)
            );
        };

        // public static ConnectDam<T> ConnectDam<T>(ConnectDam<T> connectDam) => connectDam;
        public static GetConnected<T> Connect<T>(
                GetDam<T> getDam,
                IDbConnection connection,
                IDbTransaction transaction = null) => () =>
        {
            var (all, create, drop, find, insert, update) = getDam();
            return (
            all: Connects.Connect(all, connection, transaction),
            create: Connects.Connect(create, connection, transaction),
            drop: Connects.Connect(drop, connection, transaction),
            find: Connects.Connect(find, connection, transaction),
            insert: Connects.Connect(insert, connection, transaction),
            update: Connects.Connect(update, connection, transaction)
            );
        };
    }
}