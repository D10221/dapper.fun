using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;
    using static dapper.fun.Transforms;
    using static dapper.fun.Connects;
    [TestClass]
    public class TransformTests
    {
        class User
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Roles { get; set; }
        }
        [TestMethod]
        public async Task TransformsParameters()
        {
            Database.Drop();
            using (var connection = Database.Connect())
            {
                DbCommand<int, User> Find = ChangeParam(
                                QuerySingle<object, User>(@"
                -- SQLite
                WITH x AS (values(1,'bob','password', 'admin')) 
                select 
                    x.Column1 as ID,
                    x.COlumn2 as Name,
                    x.Column3 as Password,
                    x.COlumn4 as Roles
                from x
                WHERE id = @ID
                "),
                  transform: (int id) => new { ID = id }
                );
                var find = Connect(Find, connection);
                var found = await find(1);
                var subject = found.Should();
                subject.NotBeNull();
                subject.BeOfType<User>();
                found.Name.Should().Be("bob");
            }

        }
    }
}