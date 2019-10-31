using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;
    using static dapper.fun.Connects;
    [TestClass]
    public class SelectsTests
    {
        class User
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        class Users
        {
            public static (Select<IEnumerable<User>> all, Select<int> create, Select<int> drop, Select<int, User> find, Select<User, int> insert, Select<User, int> update) Dac()
            {
                return (
                    all: Query<User>("select * from user"),
                    create: Exec("create table if not exists User( id int primary key , name text not null )"),
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
        [TestMethod]
        public async Task MiniDac()
        {
            IEnumerable<User> users;
            Database.Drop();
            using (var connection = Database.Connect())
            {
                var dac = Users.Dac();
                
                var connected = new
                {
                    create = Connect(dac.create, connection),
                    drop = Connect(dac.drop, connection),
                };

                await connected.drop();
                await connected.create();

                var (all, _, _, find, insert, update) = Users.Connected(connection);

                await insert(new User { Name = "bob" });
                users = await all();
                users.FirstOrDefault().Name.Should().Be("bob");

                await update(new User { Name = "Tom", ID = users.FirstOrDefault().ID });                                
                
                (await find((await all()).FirstOrDefault().ID)).Name.Should().Be("Tom");
            }

        }
        [TestMethod]
        public async Task QueryDefualtReturnType()
        {
            using (var con = Database.Connect())
            {
                var get = Connect(Query("select 1 as x"), con);
                var x = await get();
                ((object)(x).FirstOrDefault().x)
                             .Should()
                             .BeEquivalentTo(1);
            }
        }
    }
}
