# dapper.fun is C# (quite) functional Dapper fun

Attempt to use Dapper.net in a bit more declarative and functional way

Usage:  

With static imports

```csharp
using static dapper.fun.Selects;
using static dapper.fun.Transforms;
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
Note: only primitives | valuetypes & string | byte[] are mapped to { param = value }

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

Selectors... Exec, Scalar, Query and derivatives  Take Tuples, string

```csharp
Query("select something from X")
Query((text: "select something from X", token: CancellationToken))
Query((text: "select something from X", commandTimeout: 90))
Query((text: "select something from X", commandType: CommandType.StoredProcedure))
Query((text: "select something from X", buffered: true))
Query((text, commandTimeout, commandType, buffered, cancel));

buffered

```

QueryMap: "functors" Dapper's Query(...types[], map)  amd Query<...T7>

```csharp
var sql = @"
        With user as (
            select  
                1 as userID,
                'bob' as name
        ),
        post as (
            select 
                1 as postID,
                'hello' as text,
                1 as userid
        )
        select 
            u.*, 
            '____' as id,
            p.* 
            from user u 
            join post p 
                on p.userid = u.userid
    ";

var types = new[] { typeof(Pocos2.User), typeof(Pocos2.Post) };

var map = MakeMap(r => new { user = r[0], post = r[1] });

var get = Connect(QueryMap(sql, types, map), connection);

var results = await get();
var x = results.FirstOrDefault();
x.user.Should().BeEquivalentTo(new { UserID = 1, Name = "bob" });
x.post.Should().BeEquivalentTo(new Pocos2.Post { PostID = 1, Text = "hello", UserID = 1 });
```

Typed:

```csharp
var sql = @"
        with T1 as (
            select 1 as id, 't1' as name
        ),
        T2 as (
            select 1 as id, 't2' as name
        ),
        T3 as (
            select 1 as id, 't3' as name
        ),
        T4 as (
            select 1 as id, 't4' as name
        ),
        T5 as (
            select 1 as id, 't5' as name
        ),
        T6 as (
            select 1 as id, 't6' as name
        ),
        T7 as (
            select 1 as id, 't7' as name
        )
        SELECT * from  T1
        JOIN T2 on T2.id = T1.id
        JOIN T3 on T3.id = T2.id
        JOIN T4 on T4.id = T3.id
        JOIN T5 on T5.id = T4.id
        JOIN T6 on T6.id = T5.id
        JOIN T7 on T7.id = T6.id
";
var query = Connect(NoParams(QueryMap<object, T1, T2, T3, T4, T5, T6, T7, Result>(
        sql,
        (t1, t2, t3, t4, t5, t6, t7) => new Result
        {
            T1 = t1,
            T2 = t2,
            T3 = t3,
            T4 = t4,
            T5 = t5,
            T6 = t6,
            T7 = t7,
        }
    )),
    connection
);
var result = await query();
result.Should().BeEquivalentTo(new Result
{
    T1 = new T1 { ID = 1, Name = "t1" },
    T2 = new T2 { ID = 1, Name = "t2" },
    T3 = new T3 { ID = 1, Name = "t3" },
    T4 = new T4 { ID = 1, Name = "t4" },
    T5 = new T5 { ID = 1, Name = "t5" },
    T6 = new T6 { ID = 1, Name = "t6" },
    T7 = new T7 { ID = 1, Name = "t7" },
});
```

Notes:  
    - Async Only  
