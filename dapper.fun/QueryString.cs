using System.Data;
using System.Threading;
using Dapper;

namespace dapper.fun
{
    public struct QueryString
    {
        public QueryString(string text, int? commandTimeout = null, CommandType? commandType = null, bool buffered = true, CancellationToken cancel = default(CancellationToken))
        {
            Text = text;
            CommandTimeout = commandTimeout;
            CommandType = commandType;
            CancellationToken = cancel;
            Buffered = buffered;
        }
        public string Text { get; private set; }
        public int? CommandTimeout { get; private set; }
        public CommandType? CommandType { get; private set; }
        public CancellationToken CancellationToken { get; private set; }
        public bool Buffered { get; private set; }
        public void Deconstruct(out string text, out int? commandTimeout, out CommandType? commandType, out bool buffered, out CancellationToken cancel)
        {
            text = Text;
            commandTimeout = CommandTimeout;
            commandType = CommandType;
            buffered = Buffered;
            cancel = CancellationToken;
        }
        public static implicit operator QueryString((string text, int? commandTimeout, CommandType? commandType, bool buffered, CancellationToken cancel) x)
        {
            return new QueryString { Text = x.text, CommandTimeout = x.commandTimeout, CommandType = x.commandType, Buffered = x.buffered, CancellationToken = x.cancel };
        }
        public static implicit operator QueryString((string text, int? commandTimeout, CommandType? commandType, bool buffered) x)
        {
            return new QueryString { Text = x.text, CommandTimeout = x.commandTimeout, CommandType = x.commandType, Buffered = x.buffered };
        }
        public static implicit operator QueryString((string text, int? commandTimeout, CommandType? commandType) x)
        {
            return new QueryString { Text = x.text, CommandTimeout = x.commandTimeout, CommandType = x.commandType };
        }
        public static implicit operator QueryString((string text, int? commandTimeout) x)
        {
            return new QueryString { Text = x.text, CommandTimeout = x.commandTimeout };
        }
        public static implicit operator QueryString((string text, CommandType? commandType) x)
        {
            return new QueryString { Text = x.text, CommandType = x.commandType };
        }
        public static implicit operator QueryString((string text, bool buffered) x)
        {
            return new QueryString { Text = x.text, Buffered = x.buffered };
        }
        public static implicit operator QueryString((string text, CancellationToken cancel) x)
        {
            return new QueryString { Text = x.text, CancellationToken = x.cancel };
        }

        public static implicit operator QueryString(string cmd)
        {
            return new QueryString(cmd);
        }
        public static implicit operator string(QueryString query)
        {
            return query.Text;
        }
        public static CommandDefinition AsCommandDefinition(QueryString query)
        {
            return new CommandDefinition(
                commandText: query.Text, parameters: null,
                transaction: null,
                commandTimeout: null,
                commandType: null,
                flags: CommandFlags.None,
                cancellationToken: default(CancellationToken)
                );
        }
        public static bool isCancellable(QueryString query)
        {
            return query.CancellationToken != default(CancellationToken);
        }
    }
    public static class QueryStringExtensions
    {
        public static CommandDefinition AsCommandDefinition(this QueryString query, object param = null, IDbTransaction transaction = null)
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
        public static bool IsCancellable(this QueryString query)
        {
            return query.CancellationToken != default(CancellationToken);
        }
        public static bool ShouldBeCommand(this QueryString query){
            return  query.IsCancellable() || query.Buffered == true ;
        }
    }
}