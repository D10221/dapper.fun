using System;
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
    public class QueryMapTests
    {
        class Pocos
        {
            public class User
            {
                public int ID { get; set; }
                public string Name { get; set; }
            }
            public class Post
            {
                public int ID { get; set; }
                public int UserID { get; set; }
                public string Text { get; set; }
            }
        }
        class Pocos2
        {
            public class User
            {
                public int UserID { get; set; }
                public string Name { get; set; }
            }
            public class Post
            {
                public int PostID { get; set; }
                public int UserID { get; set; }
                public string Text { get; set; }
            }
        }
        [TestMethod]

        public async Task SelectsMultiTest()
        {
            using (var con = Database.Connect())
            {
                var sql = @"
With user as (
    select  
        1 as id,
        'bob' as name
),
post as (
    select 
        1 as id,
        'hello' as text,
        1 as userid
    union all select 
        2 as id,
        'hello2' as text,
        1 as userid
    
)
select 
    u.*, 
    p.* 
    from user u 
    join post p 
        on p.id = u.id
                ";
                var types = new[] { typeof(Pocos.User), typeof(Pocos.Post) };
                var get = Connect(QueryMap(sql, types, (r) => new { user = r[0], post = r[1] }), con);

                var results = await get();
                var x = results.FirstOrDefault();
                x.user.Should().BeEquivalentTo(new { ID = 1, Name = "bob" });
                x.post.Should().BeEquivalentTo(new Pocos.Post { ID = 1, Text = "hello", UserID = 1 });

            }
        }
        [TestMethod]

        public async Task SelectsMultiSplitsTest()
        {
            using (var con = Database.Connect())
            {
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

                var get = Connect(QueryMap(sql, types, map), con);

                var results = await get();
                var x = results.FirstOrDefault();
                x.user.Should().BeEquivalentTo(new { UserID = 1, Name = "bob" });
                x.post.Should().BeEquivalentTo(new Pocos2.Post { PostID = 1, Text = "hello", UserID = 1 });

            }
        }
        [TestMethod]
        public void MakeMapTest()
        {
            var map = MakeMap(
                objs => new { x = objs[0] },
                x => x.x);
            var r = map(new[] { "x" });
            r.Should().Be("x");
        }
        [TestMethod]
        public void MakeMapsTest()
        {
            var map = MakeMap(
                objs => new { x = (int)objs[0] + 1 },
                x => x.x + 1,
                x => x + 1,
                x => x + 1,
                x => x + 1,
                x => x + 1,
                x => x + 1
                );
            var r = map(new object[] { 0 });
            r.Should().Be(7);
        }

        [TestMethod]
        public async Task QueryMap7()
        {
            using (var con = Database.Connect())
            {
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
                    con
                );
                var result = await query();
                result.Should().BeEquivalentTo(new Result{
                    T1 = new T1 { ID = 1, Name = "t1"},
                    T2 = new T2 { ID = 1, Name = "t2"},
                    T3 = new T3 { ID = 1, Name = "t3"},
                    T4 = new T4 { ID = 1, Name = "t4"},
                    T5 = new T5 { ID = 1, Name = "t5"},
                    T6 = new T6 { ID = 1, Name = "t6"},
                    T7 = new T7 { ID = 1, Name = "t7"},
                });
            }
        }
    }
    class T1
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    class T2
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    class T3
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    class T4
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    class T5
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    class T6
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    class T7
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    class Result
    {
        public T1 T1 { get; internal set; }
        public T2 T2 { get; internal set; }
        public T3 T3 { get; internal set; }
        public T4 T4 { get; internal set; }
        public T5 T5 { get; internal set; }
        public T6 T6 { get; internal set; }
        public T7 T7 { get; internal set; }
    }
}