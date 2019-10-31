namespace dapper.fun.dam.test
{
    public interface IUser
    {
        int ID { get; set; }
        string Name { get; set; }
    }
    public class User: IUser 
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
