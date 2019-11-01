using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.dam.test
{
    using System;
    using Dapper;
    using static dapper.fun.Connects;
    using static dapper.fun.dam.Dam;
    public partial class DapperFunDamTests
    {
        [TestMethod]
        public async Task TestDamBuilt()
        {
            SqlMapper.ResetTypeHandlers();

            // TODO: specify PropertyType Handling fo specific types and/or cases
            SqlMapper.RemoveTypeMap(typeof(Strings));
            var typeHandler = new PropertyTypeHandler<Strings>(
                o => (o as string)?.Split(","),
                param => v => v.Value?.Aggregate((a, b) => a + "," + b)
            );

            SqlMapper.AddTypeHandler(typeHandler);

            // Map interface properties, ignore readonly ? 
            // SqlMapper.SetTypeMap(typeof(User), new InterfaceTypeMap<IUser, User>());       

            using (var connection = Database.Connect())
            {
                var (all, create, drop, find, insert, update) = Connect(Create<User>((
                    all: Users.Scripts.Sqlite.All,
                    create: Users.Scripts.Sqlite.Create,
                    drop: Users.Scripts.Sqlite.Drop,
                    find: Users.Scripts.Sqlite.Find,
                    insert: Users.Scripts.Sqlite.Insert,
                    update: Users.Scripts.Sqlite.Update)
                ), connection)();

                await drop();
                await create();
                connection.Execute(Users.Scripts.Sqlite.Insert, new
                {
                    Name = "bob",
                    Roles = new[] { "admin" }
                });
                var user = (await all()).FirstOrDefault();
                user.Roles.Should().BeEquivalentTo((Strings) new[] { "admin" });
            }

        }
    }
}
