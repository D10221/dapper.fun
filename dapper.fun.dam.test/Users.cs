using System.Collections.Generic;
using System.Data;

namespace dapper.fun.dam.test
{
    using static dapper.fun.Selects;
    using static dapper.fun.Connects;
    public class Users
    {
        public static (Select<IEnumerable<User>> all, Select<int> create, Select<int> drop, Select<int, User> find, Select<User, int> insert, Select<User, int> update) Dac()
        {
            return (
                all: Query<User>("select * from user"),
                create: Exec("create table if not exists User( id integer primary key autoincrement , name text not null )"),
                drop: Exec("drop table if exists user"),
                find: QuerySingle<int, User>("select * from user where id = @param"),
                insert: Exec<User>("insert into user ( Name ) values ( @Name )"),
                update: Exec<User>("update User set Name = @Name where id = @ID")
            );
        }
        public static (Selector<IEnumerable<User>> all, Selector<int> create, Selector<int> drop, Selector<int, User> find, Selector<User, int> insert, Selector<User, int> update) Connected(IDbConnection connection, IDbTransaction transaction = null)
        {
            var (all, create, drop, find, insert, update) = Dac();
            return (
                all: Connect(all, connection, transaction),
                create: Connect(create, connection, transaction),
                drop: Connect(drop, connection, transaction),
                find: Connect(find, connection, transaction),
                insert: Connect(insert, connection, transaction),
                update: Connect(update, connection, transaction)
            );
        }
    }
}
