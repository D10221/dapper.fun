using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;
    using static dapper.fun.Connects;
    using static dapper.fun.Transforms;
    [TestClass]
    public class Usage
    {
        class Values
        {
            public int Column1 { get; set; }
            public int Column2 { get; set; }
            public int Column3 { get; set; }
        }
        class Expected { public string Value { get; set; } }
        class A { public int Value { get; set; } }
        class B { public int Value { get; set; } }
        class C { public string StringValue { get; set; } }
        [TestMethod]
        public async Task Demo()
        {
            using (var connection = Database.Connect())
            {
                // Query -with parameters
                {
                    var query = Connect(Query<object, Expected>("select 'hello' as Value where Value = @value"))(connection);
                    var result = await query(new { value = "hello" });
                    result.FirstOrDefault().Value.Should().Be("hello");
                }
                // Query - automapped parameters '@param'
                // Note: only primitives / valuetypes & string or byte[] are mapped to { param = value }
                {
                    var query = Connect(Query<string, Expected>("select 'hello' as Value where Value = @param"))(connection);
                    var result = await query("hello");
                    result.FirstOrDefault().Value.Should().Be("hello");
                }
                // Query - without parameters
                {
                    var query = Connect(Query<Expected>("select 'hello' as Value"))(connection);
                    var result = await query();
                    result.FirstOrDefault().Value.Should().Be("hello");
                }
                // Query Single - without parameters
                {
                    var query = Connect(QuerySingle<int>("select 1"))(connection);
                    var result = await query();
                    result.Should().Be(1);
                }
                // Scalar - without parameters
                {
                    var query = Connect(Scalar<int>("select 1"))(connection);
                    var result = await query();
                    result.Should().Be(1);
                }
                // Select Many, use reader
                {
                    var select = Connect((SelectMany(@"
                    select 1 as Value where @value = @value;
                    select 2 as Value where @value = @value;
                    select 'x' as StringValue where @value = @value;
                    ", typeof(A), typeof(B), typeof(C)
                    )), connection);
                    
                    var reader = await select(new { value = "x" });
                    
                    var a = reader.Read<A>();
                    a.Should().BeEquivalentTo(new A { Value = 1 });

                    var b = reader.Read<B>();
                    b.Should().BeEquivalentTo(new B { Value = 2 });
                    
                    var c = reader.Read<C>();
                    c.Should().BeEquivalentTo(new C { StringValue = "x" });
                }
                // Select Many , Query multiple
                {
                    var select = Connect((SelectMany<A, B, C>(@"
                    select 1 as Value where @value = @value;
                    select 2 as Value where @value = @value;
                    select 'x' as StringValue where @value = @value;
                ")), connection);
                    // nice little tuple :)
                    var (a, b, c) = await select(new { value = "x" });

                    a.Should().BeEquivalentTo(new A { Value = 1 });
                    b.Should().BeEquivalentTo(new B { Value = 2 });
                    c.Should().BeEquivalentTo(new C { StringValue = "x" });
                }
                // Select Many , Query multiple, without pramaters
                {
                    // NoParams removes the need to to pass null as parameter
                    var select = Connect(NoParams(SelectMany<A, B, C>(@"
                    select 1 as Value;
                    select 2 as Value;
                    select 'x' as StringValue
                ")), connection);

                    var (a, b, c) = await select();

                    a.Should().BeEquivalentTo(new A { Value = 1 });
                    b.Should().BeEquivalentTo(new B { Value = 2 });
                    c.Should().BeEquivalentTo(new C { StringValue = "x" });
                }
            }
        }
    }
}