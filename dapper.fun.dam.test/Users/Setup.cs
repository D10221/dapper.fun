namespace dapper.fun.dam.test.users
{
    public class Setup
    {
        public Select<int> Create { get; private set; }
        public Select<int> Drop { get; private set; }

        public void Deconstruct(out Select<int> create, out Select<int> drop)
        {
            create = Create;
            drop = Drop;
        }
        
        public static implicit operator Setup((Select<int> create, Select<int> drop) x)
        {
            var (create, drop) = x;
            return new Setup { Create = create, Drop = drop };
        }
    }
}
