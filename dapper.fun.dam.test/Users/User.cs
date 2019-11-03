using valueof;

namespace dapper.fun.dam.test
{
    public class User: IUser 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ValueOf<string[]> Roles {get; set; }
    }    
}
