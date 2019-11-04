namespace dapper.fun.dam.test.users
{
    public class Setup
    {
        public DBCommand<int> Create { get; private set; }
        public DBCommand<int> Drop { get; private set; }

        public void Deconstruct(out DBCommand<int> create, out DBCommand<int> drop)
        {
            create = Create;
            drop = Drop;
        }
        
        public static implicit operator Setup((DBCommand<int> create, DBCommand<int> drop) x)
        {
            var (create, drop) = x;
            return new Setup { Create = create, Drop = drop };
        }
    }
}
