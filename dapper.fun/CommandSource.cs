using System.Data;
using System.Threading;
using Dapper;

namespace dapper.fun
{
    public struct CommandSource
    {
        public CommandSource(string text, int? commandTimeout = null, CommandType? commandType = null, bool buffered = true, CancellationToken cancel = default(CancellationToken))
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
        public static implicit operator CommandSource((string text, int? commandTimeout, CommandType? commandType, bool buffered, CancellationToken cancel) x)
        {
            return new CommandSource { Text = x.text, CommandTimeout = x.commandTimeout, CommandType = x.commandType, Buffered = x.buffered, CancellationToken = x.cancel };
        }
        public static implicit operator CommandSource((string text, int? commandTimeout, CommandType? commandType, bool buffered) x)
        {
            return new CommandSource { Text = x.text, CommandTimeout = x.commandTimeout, CommandType = x.commandType, Buffered = x.buffered };
        }
        public static implicit operator CommandSource((string text, int? commandTimeout, CommandType? commandType) x)
        {
            return new CommandSource { Text = x.text, CommandTimeout = x.commandTimeout, CommandType = x.commandType };
        }
        public static implicit operator CommandSource((string text, int? commandTimeout) x)
        {
            return new CommandSource { Text = x.text, CommandTimeout = x.commandTimeout };
        }
        public static implicit operator CommandSource((string text, CommandType? commandType) x)
        {
            return new CommandSource { Text = x.text, CommandType = x.commandType };
        }
        public static implicit operator CommandSource((string text, bool buffered) x)
        {
            return new CommandSource { Text = x.text, Buffered = x.buffered };
        }
        public static implicit operator CommandSource((string text, CancellationToken cancel) x)
        {
            return new CommandSource { Text = x.text, CancellationToken = x.cancel };
        }

        public static implicit operator CommandSource(string cmd)
        {
            return new CommandSource(cmd);
        }
        public static implicit operator string(CommandSource query)
        {
            return query.Text;
        }
        public static CommandDefinition AsCommandDefinition(CommandSource query)
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
        public static bool isCancellable(CommandSource query)
        {
            return query.CancellationToken != default(CancellationToken);
        }
    }
}