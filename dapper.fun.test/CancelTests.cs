using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dapper.fun.test
{
    using static dapper.fun.Selects;
    using static dapper.fun.Connects;

    [TestClass]
    public class CancelTests
    {
        [TestMethod]
        public async Task CancelsExec()
        {
            using (var con = Database.Connect())
            {
                using (var tokenSource = new CancellationTokenSource())
                {
                    var cancel = tokenSource.Token;
                    var wait = Connect(Exec((text: "sqlilte3_sleep(100); select 1;", cancel)), con);
                    tokenSource.Cancel();

                    Exception err = null;
                    try
                    {
                        await wait();
                    }
                    catch (Exception ex)
                    {
                        err = ex;
                    }

                    err.Should().NotBeNull();
                    err.Should().BeOfType<TaskCanceledException>();
                }

            }
        }
        [TestMethod]
        public async Task CancelsScalar()
        {
            using (var con = Database.Connect())
            {
                using (var tokenSource = new CancellationTokenSource())
                {
                    var cancel = tokenSource.Token;
                    var wait = Connect(Scalar<int>((text: "sqlilte3_sleep(100); select 1;", cancel)), con);
                    tokenSource.Cancel();

                    Exception err = null;
                    try
                    {
                        await wait();
                    }
                    catch (Exception ex)
                    {
                        err = ex;
                    }

                    err.Should().NotBeNull();
                    err.Should().BeOfType<TaskCanceledException>();
                }

            }
        }
        [TestMethod]
        public async Task CancelsQuery()
        {
            using (var con = Database.Connect())
            {
                using (var tokenSource = new CancellationTokenSource())
                {
                    var cancel = tokenSource.Token;
                    var wait = Connect(Query<int>((text: "sqlilte3_sleep(100); select 1;", cancel)), con);
                    tokenSource.Cancel();

                    Exception err = null;
                    try
                    {
                        await wait();
                    }
                    catch (Exception ex)
                    {
                        err = ex;
                    }

                    err.Should().NotBeNull();
                    err.Should().BeOfType<TaskCanceledException>();
                }

            }
        }
    }
}
