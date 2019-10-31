namespace dapper.fun.dam
{
    public struct Queries
    {
        public Queries(QueryString all, QueryString create, QueryString drop, QueryString find, QueryString insert, QueryString update){
            All = all;
            Create = create;
            Drop = drop;
            Find = find;
            Insert = insert;
            Update = update;
        }
        QueryString All;
        QueryString Create;
        QueryString Drop; 
        QueryString Find; 
        QueryString Insert; 
        QueryString Update;
        public void Deconstruct(out QueryString all, out QueryString create, out QueryString drop, out QueryString find, out QueryString insert, out QueryString update)
        {
            all = All;
            create= Create;
            drop = Drop;
            find = Find;
            insert = Insert;
            update =Update;
        }
        public static implicit operator Queries ((QueryString all, QueryString create, QueryString drop, QueryString find, QueryString insert, QueryString update) queries) {
            var (all, create, drop, find, insert, update) = queries;
            return new Queries(all, create, drop, find, insert, update);
        }
    }
}