namespace DotNetWebInterface.Application.Route
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireAuthenticationAttribute : Attribute
    { 
        public RequireAuthenticationAttribute() 
        {
           
        } 
    }
}