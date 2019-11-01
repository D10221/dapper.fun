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
                all: Query<User>(Scripts.Sqlite.All),
                create: Exec(Scripts.Sqlite.Create),
                drop: Exec(Scripts.Sqlite.Drop),
                find: QuerySingle<int, User>(Scripts.Sqlite.Find),
                insert: Exec<User>(Scripts.Sqlite.Insert),
                update: Exec<User>(Scripts.Sqlite.Update)
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
        public class Scripts
        {
            public class Sqlite
            {
                public const string All = "select * from user";
                public const string Create = @"
                    create table if not exists User( 
                        id integer primary key autoincrement , 
                        name text not null ,
                        roles text )";
                public const string Drop = "drop table if exists user";
                public const string Find = "select * from user where id = @param";
                public const string Insert = @"
                    insert into user ( 
                        Name, 
                        Roles 
                    ) values ( 
                        @Name, 
                        @Roles 
                    )";
                public const string Update = "update User set Name = @Name, Roles = @roles where id = @ID";
            }
        }
    }
}
