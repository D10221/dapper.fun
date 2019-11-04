namespace dapper.fun.dam
{
    public struct Queries
    {
        public Queries(CommandSource all, CommandSource create, CommandSource drop, CommandSource find, CommandSource insert, CommandSource update){
            All = all;
            Create = create;
            Drop = drop;
            Find = find;
            Insert = insert;
            Update = update;
        }
        CommandSource All;
        CommandSource Create;
        CommandSource Drop; 
        CommandSource Find; 
        CommandSource Insert; 
        CommandSource Update;
        public void Deconstruct(out CommandSource all, out CommandSource create, out CommandSource drop, out CommandSource find, out CommandSource insert, out CommandSource update)
        {
            all = All;
            create= Create;
            drop = Drop;
            find = Find;
            insert = Insert;
            update =Update;
        }
        public static implicit operator Queries ((CommandSource all, CommandSource create, CommandSource drop, CommandSource find, CommandSource insert, CommandSource update) queries) {
            var (all, create, drop, find, insert, update) = queries;
            return new Queries(all, create, drop, find, insert, update);
        }
    }
}