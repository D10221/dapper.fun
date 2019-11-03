using System.Collections.Generic;
using System.Data;

namespace dapper.fun.dam.test.users
{
    using static dapper.fun.Selects;
    using static dapper.fun.Connects;
    public class Users
    {
        public static Setup Setup()
        {
            return (create: Exec(Scripts.Sqlite.Create), drop: Exec(Scripts.Sqlite.Drop));
        }
        public static (Selector<int> create, Selector<int> drop) Connected(Setup s, IDbConnection connection, IDbTransaction transaction = null)
        {
            var (create, drop) = s;
            return (
                create: Connect(create, connection, transaction),
                drop: Connect(drop, connection, transaction)
            );
        }
   
        public static Queries Queries()
        {
            return (
                get: Query<object, User>(Scripts.Sqlite.Query),
                find: QuerySingleOrDefault<int, User>(Scripts.Sqlite.Find),
                insert: Exec<User>(Scripts.Sqlite.Insert),
                update: Exec<User>(Scripts.Sqlite.Update),
                delete: Exec<int>(Scripts.Sqlite.Delete)
            );
        }
        public static (Selector<object, IEnumerable<User>> get, Selector<int, User> find, Selector<User, int> insert, Selector<User, int> update, Selector<int, int> delete) Connected(
            Queries q, 
            IDbConnection connection, IDbTransaction transaction = null)
        {
            var (get, find, insert, update, delete) = q;
            return (
                get: Connect(get, connection, transaction),
                find: Connect(find, connection, transaction),
                insert: Connect(insert, connection, transaction),
                update: Connect(update, connection, transaction),
                delete: Connect(delete, connection, transaction)
                );
        }

    }
}
