# dapper.fun is C# (quite) functional Dapper fun

Attempt to use Dapper.net in a bit more declarative and functional way

Usage:  

Optimized for static imports

```csharp
using static dapper.fun.Selects;
using static dapper.fun.Transforms;
using static dapper.fun.Queries;
using static dapper.fun.Connects;
```

Query -with parameters  

```csharp
var query = Connect(Query<object, Expected>(
                "select 'hello' as Value where Value = @value"
                ))(connection);
var result = await query(new { value = "hello" });
result.FirstOrDefault().Value.Should().Be("hello");
```

Query - automapped parameters '@param'  
Note: only primitives / valuetypes & string or byte[] are mapped to { param = value }

```csharp
var query = Connect(Query<string, Expected>(
    "select 'hello' as Value where Value = @param"
))(connection);

var result = await query("hello");
result.FirstOrDefault().Value.Should().Be("hello");
```

Query - without parameters

```csharp
var query = Connect(Query<Expected>("select 'hello' as Value"))(connection);
var result = await query();
result.FirstOrDefault().Value.Should().Be("hello");
```

Query Single - without parameters

```csharp
var query = Connect(QuerySingle<int>("select 1"))(connection);
var result = await query();
result.Should().Be(1);
```

Scalar - without parameters

```csharp
var query = Connect(Scalar<int>("select 1"))(connection);
var result = await query();
result.Should().Be(1);
```

Select Many, use reader

```csharp
var select = Connect(
(SelectMany(@"
        select 1 as Value where @value = @value;
        select 2 as Value where @value = @value;
        select 'x' as StringValue where @value = @value;
        ",
/*types:*/ typeof(A), typeof(B), typeof(C))),
connection /*, transaction */);

var reader = await select(new { value = "x" });
var a = reader.Read<A>();
a.Should().BeEquivalentTo(new A { Value = 1 });

var b = reader.Read<B>();
b.Should().BeEquivalentTo(new B { Value = 2 });

var c = reader.Read<C>();
c.Should().BeEquivalentTo(new C { StringValue = "x" });
```

Select Many , Query multiple

```csharp
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
```

Select Many , Query multiple, without parameters  
Note: NoParams removes the need to to pass null as parameter

```csharp
var select = Connect(NoParams(SelectMany<A, B, C>(@"
select 1 as Value;
select 2 as Value;
select 'x' as StringValue
")), connection);

var (a, b, c) = await select();
a.Should().BeEquivalentTo(new A { Value = 1 });
b.Should().BeEquivalentTo(new B { Value = 2 });
c.Should().BeEquivalentTo(new C { StringValue = "x" });
```

Notes:  
    - Async Only
    - todo?: multi query mapping (use external mapper?)
