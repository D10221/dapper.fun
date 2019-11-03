using System.Collections.Generic;

namespace dapper.fun.dam.test.users
{
    public class Queries
        {
            public Select<object, IEnumerable<User>> Get { get; private set; }
            public Select<int, User> Find { get; private set; }
            public Select<User, int> Insert { get; private set; }
            public Select<User, int> Update { get; private set; }
            public Select<int, int> Delete { get; private set; }
            public void Deconstruct(
                out Select<object, IEnumerable<User>> get,
                out Select<int, User> find,
                out Select<User, int> insert,
                out Select<User, int> update,
                out Select<int, int> delete)
            {
                get = Get;
                find = Find;
                insert = Insert;
                update = Update;
                delete = Delete;
            }
            public static implicit operator Queries((
                Select<object, IEnumerable<User>> get,
                Select<int, User> find,
                Select<User, int> insert,
                Select<User, int> update,
                Select<int, int> delete
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
