﻿namespace DotNetWebInterface.Route
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireRoleAttribute : Attribute
    {
        public string RequiredRole { get; set; }

        public RequireRoleAttribute(string requireRole)
        {
            RequiredRole = requireRole;
        }
    }
}