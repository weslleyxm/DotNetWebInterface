namespace DotNetWebInterface.Controllers.Authentication 
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequireAuthenticationAttribute : Attribute
    {
        public RequireAuthenticationAttribute()
        {

        }
    }
}