﻿namespace dapper.fun.dam.test
{
    using System;
    using System.Data;
    using Dapper;
    public static class PropertyTypeHandler {
        public static PropertyTypeHandler<T> From<T>( Func<object, T> get,Func<IDbDataParameter, Func<T, object>> set){
            return new PropertyTypeHandler<T>(get, set);
        }
    }
    public class PropertyTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        private Func<object, T> _get;
        private Func<IDbDataParameter, Func<T, object>> _set;

        DefaultTypeMap _defaultTypeMap;

        public PropertyTypeHandler(
            Func<object, T> get,
            Func<IDbDataParameter, Func<T, object>> set
            )
        {
            _get = get;
            _set = set;           
        }
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            if (_defaultTypeMap != null)
            {
                return;
            }
            parameter.Value = _set(parameter)(value);
        }

        public override T Parse(object value)
        {
            return _get(value);
        }
    }
}
