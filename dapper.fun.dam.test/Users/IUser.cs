using valueof;

namespace dapper.fun.dam.test
{
    public interface IUser
    {
        int ID { get; }
        string Name { get; }        
        ValueOf<string[]> Roles {get; set; }
    }
}
