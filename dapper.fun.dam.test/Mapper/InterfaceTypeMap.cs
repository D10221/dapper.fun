namespace dapper.fun.dam.test
{
    using System;
    using System.Reflection;
    using Dapper;

    public class InterfaceTypeMap<TInterface, Timplementation> : SqlMapper.ITypeMap where Timplementation : TInterface
    {
        DefaultTypeMap _interfaceTypeMap  = new DefaultTypeMap(typeof(TInterface));
        DefaultTypeMap _implementationTypeMap  = new DefaultTypeMap(typeof(Timplementation));
        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {            
            return _implementationTypeMap.FindConstructor(names, types);
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            return _implementationTypeMap.FindExplicitConstructor();
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            return _implementationTypeMap.GetConstructorParameter(constructor, columnName);
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            return _interfaceTypeMap.GetMember(columnName);
        }        
    }
}
