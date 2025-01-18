
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;

namespace DotNetWebInterface.Controllers
{
    internal static class MethodExecutorCache
    {
        private static readonly ConcurrentDictionary<MethodInfo, Delegate> _delegateCache = new();

        public static Delegate GetDelegate(MethodInfo methodInfo)
        {
            return _delegateCache.GetOrAdd(methodInfo, CreateDelegate);
        }

        private static Delegate CreateDelegate(MethodInfo methodInfo)
        {
            var targetParameter = Expression.Parameter(typeof(object), "target");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            var parameters = methodInfo.GetParameters()
                .Select((p, i) =>
                    Expression.Convert(
                        Expression.ArrayIndex(parametersParameter, Expression.Constant(i)),
                        p.ParameterType))
                .ToArray();

            var instance = Expression.Convert(targetParameter, methodInfo.DeclaringType!);
            var call = Expression.Call(instance, methodInfo, parameters);

            Expression body;

            if (methodInfo.ReturnType == typeof(void))
            { 
                body = Expression.Block(call, Expression.Constant(null));
            }
            else
            { 
                body = Expression.Convert(call, typeof(object));
            }

            var lambda = Expression.Lambda(body, targetParameter, parametersParameter); 
            return lambda.Compile(); 
        } 
    } 
}
