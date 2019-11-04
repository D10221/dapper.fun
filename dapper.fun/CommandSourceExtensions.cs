using System.Data;
using System.Threading;
using Dapper;

namespace dapper.fun
{
    public static class CommandSourceExtensions
    {
        public static CommandDefinition AsCommandDefinition(this CommandSource query, object param = null, IDbTransaction transaction = null)
        {
            return new CommandDefinition(
                commandText: query.Text,
                parameters: param,
                transaction: transaction,
                commandTimeout: query.CommandTimeout,
                commandType: query.CommandType,
                // this is what dapper does on 'QueryAsync'
                flags: query.Buffered ? CommandFlags.Buffered : CommandFlags.None,
                cancellationToken: query.CancellationToken
                );
        }
        public static bool IsCancellable(this CommandSource query)
        {
            return query.CancellationToken != default(CancellationToken);
        }
        public static bool ShouldBeCommand(this CommandSource query){
            return  query.IsCancellable() || query.Buffered == true ;
        }
    }
}