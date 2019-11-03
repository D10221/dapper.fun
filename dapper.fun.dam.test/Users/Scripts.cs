namespace dapper.fun.dam.test.users
{
    public class Scripts
    {
        public class Sqlite
        {
            public const string Query = "select * from user";
            public const string Create = @"
                    create table if not exists User( 
                        id integer primary key autoincrement , 
                        name text not null ,
                        roles text )";
            public const string Drop = "drop table if exists user";
            public const string Find = "select * from user where id = @param limit 1";
            public const string Insert = @"
                    insert into user ( 
                        Name, 
                        Roles 
                    ) values ( 
                        @Name, 
                        @Roles 
                    )";
            public const string Update = "update User set Name = @Name, Roles = @roles where id = @ID";
            public const string Delete = "select * from user";
        }
    }
}
