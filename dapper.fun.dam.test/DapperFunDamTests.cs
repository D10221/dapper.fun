using System.Linq;
using System.Threading.Tasks;
using dapper.fun.dam.test.users;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using valueof;

namespace dapper.fun.dam.test
{
    using static dapper.fun.Connects;

    [TestClass]
    public partial class DapperFunDamTests
    {
        [TestMethod]
        public async Task TestDamTyped()
        {
            Database.Drop();

            var typeHandler = PropertyTypeHandler.From<ValueOf<string[]>>(
                o => o == null ? null : ((string)o)?.Split(","),
                param => v => v.Value?.Aggregate((a, b) => a + "," + b)
            );

            using (var connection = Database.Connect())
            using (var d = DisposableTypeHandler.Use(typeHandler))
            {
                var (create, drop) = Users.Connected(Users.Setup(), connection);

                await drop();
                await create();

                var (get, find, insert, update, delete) = Users.Connected(Users.Queries(), connection);

                await insert(new User { Name = "bob" });

                var users = await get(null);

                users.FirstOrDefault().Name.Should().Be("bob");

                await update(new User { Name = "Tom", ID = users.FirstOrDefault().ID });

                (await find((await get(null)).FirstOrDefault().ID)).Name.Should().Be("Tom");
            }

        }
        [TestMethod]
        public async Task TestDamBuilt2()
        {
            var GetUsers = Dam.Create<User>((
                all: users.Scripts.Sqlite.Query,
                create: users.Scripts.Sqlite.Create,
                drop: users.Scripts.Sqlite.Drop,
                find: users.Scripts.Sqlite.Query,
                insert: users.Scripts.Sqlite.Insert,
                update: users.Scripts.Sqlite.Update)
            );

            using (var connection = Database.Connect())
            {
                var (create, drop) = Users.Connected(Users.Setup(), connection);
                await drop();
                await create();

                var users = Dam.Connect(GetUsers, connection).Invoke();
                await users.all();
            }

        }
    }
}
