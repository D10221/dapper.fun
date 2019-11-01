using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.dam.test
{
    using Dapper;
    using static dapper.fun.Connects;

    [TestClass]
    public partial class DapperFunDamTests
    {
        [TestMethod]
        public async Task TestDamTyped()
        {
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

                var users = await all();
                users.FirstOrDefault().Name.Should().Be("bob");

                await update(new User { Name = "Tom", ID = users.FirstOrDefault().ID });

                (await find((await all()).FirstOrDefault().ID)).Name.Should().Be("Tom");
            }

        }        
        [TestMethod]
        public async Task TestDamBuilt2()
        {
            var GetUsers = Dam.Create<User>((
                all: "select * from user",
                create: "create table if not exists User( id integer primary key autoincrement , name text not null )",
                drop: "drop table if exists user",
                find: "select * from user where id = @param",
                insert: "insert into user ( Name ) values ( @Name )",
                update: "update User set Name = @Name where id = @ID")
            );

            using (var connection = Database.Connect())
            {
                var users = Dam.Connect(GetUsers, connection).Invoke();
                await users.all();
            }

        }
    }
}
