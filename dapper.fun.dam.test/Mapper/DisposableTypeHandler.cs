using System;
using Dapper;

namespace dapper.fun.dam.test
{
    public class DisposableTypeHandler {

        class Disposable : IDisposable
        {
            readonly Action _dispose;
            public Disposable(Action dispose){
                _dispose = dispose;
            }
            public void Dispose()
            {
                _dispose?.Invoke();
            }
        }
        public static IDisposable Use<T>(SqlMapper.TypeHandler<T> mapper) {
            
            SqlMapper.RemoveTypeMap(typeof(T));            

            SqlMapper.AddTypeHandler<T>(mapper);

            Action dispose = ()=> SqlMapper.RemoveTypeMap(typeof(T));;

            return new Disposable(dispose);
        }
    }
}


