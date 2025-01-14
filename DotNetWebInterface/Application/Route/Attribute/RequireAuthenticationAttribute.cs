namespace DotNetWebInterface.Route
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireAuthenticationAttribute : Attribute
    {
        public RequireAuthenticationAttribute()
        {

        }
    }
}