using System.Collections.Generic;

namespace dapper.fun.dam.test.users
{
    public class Queries
        {
            public DbCommand<object, IEnumerable<User>> Get { get; private set; }
            public DbCommand<int, User> Find { get; private set; }
            public DbCommand<User, int> Insert { get; private set; }
            public DbCommand<User, int> Update { get; private set; }
            public DbCommand<int, int> Delete { get; private set; }
            public void Deconstruct(
                out DbCommand<object, IEnumerable<User>> get,
                out DbCommand<int, User> find,
                out DbCommand<User, int> insert,
                out DbCommand<User, int> update,
                out DbCommand<int, int> delete)
            {
                get = Get;
                find = Find;
                insert = Insert;
                update = Update;
                delete = Delete;
            }
            public static implicit operator Queries((
                DbCommand<object, IEnumerable<User>> get,
                DbCommand<int, User> find,
                DbCommand<User, int> insert,
                DbCommand<User, int> update,
                DbCommand<int, int> delete
                ) x)
            {
                var (get, find, insert, update, delete) = x;
                return new Queries
                {
                    Get = get,
                    Find = find,
                    Insert = insert,
                    Update = update,
                    Delete = delete
                };
            }
        }
}
