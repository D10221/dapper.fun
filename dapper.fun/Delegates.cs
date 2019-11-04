using System.Data;
using System.Threading.Tasks;

namespace dapper.fun
{
    public delegate Task<R> Command<P, R>(P param);
    public delegate Task<R> Command<R>();
    public delegate Command<P, R> DbCommand<P, R>(IDbConnection connection, IDbTransaction transaction);
    public delegate Command<R> DBCommand<R>(IDbConnection connection, IDbTransaction transaction);
    public delegate DbCommand<P, R> DbCommandFty<P, R>(CommandSource query);
    public delegate DBCommand<R> DbCommandFty<R>(CommandSource query);
}