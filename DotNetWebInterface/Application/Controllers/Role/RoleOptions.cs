
using System.Linq;

namespace DotNetWebInterface
{
    /// <summary>
    /// Represents options for managing roles and their levels
    /// </summary>
    public class RoleOptions
    {
        /// <summary>
        /// A dictionary that maps role names to their respective levels
        /// </summary>
        internal Dictionary<string, int> RoleLevels { get; set; } = new();   

        /// <summary>
        /// The name of the field in the JWT token that stores the user's role
        /// </summary>
        internal string RoleFieldName { get; set; } = "roles";

        /// <summary>
        /// Gets the level of the specified role
        /// </summary>
        /// <param name="roleName">The name of the role</param>
        /// <returns>The level of the role if found; otherwise, 0</returns>
        public int GetRoleLevel(string roleName)
        {
            return RoleLevels.TryGetValue(roleName, out var level) ? level : 0;
        }

        /// <summary>
        /// Adds a role to the RoleLevels dictionary if it does not already exist
        /// </summary>
        /// <param name="role">The name of the role to add</param>
        /// <returns>The current instance of RoleOptions</returns>
        public RoleOptions WithRole(string role)
        {
            if (!string.IsNullOrWhiteSpace(role) && !RoleLevels.ContainsKey(role))
            {
                RoleLevels.Add(role, RoleLevels.Count);
            }
            return this;
        }

        /// <summary>
        /// Set the name of the field in the JWT token that stores the user's role
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public RoleOptions WithFieldName(string fieldName)
        {
            if (!string.IsNullOrWhiteSpace(fieldName))
            {
                RoleFieldName = fieldName;
            }
            return this;
        } 
    }
}
