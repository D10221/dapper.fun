using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Map;
    
    [TestClass]
    public class MapTest
    {
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
    }
}