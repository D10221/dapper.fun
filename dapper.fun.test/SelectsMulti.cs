using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;
    using static dapper.fun.Connects;

    [TestClass]
    public class SelectsMulti
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
    }
}