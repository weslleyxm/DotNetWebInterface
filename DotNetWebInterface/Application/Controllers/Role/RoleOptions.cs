
namespace DotNetWebInterface.Controllers.Role
{
    /// <summary>
    /// Represents options for managing roles and their levels
    /// </summary>
    public class RoleOptions
    {
        /// <summary>
        /// A dictionary that maps role names to their respective levels
        /// </summary>
        public Dictionary<string, int> RoleLevels { get; set; } = new();

        /// <summary>
        /// The name of the field in the JWT token that stores the user's role
        /// </summary>
        public string RoleFieldName { get; set; } = "roles";

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
        /// Adds a new role to the RoleLevels dictionary
        /// The level of the role will be determined by the order in which it is added
        /// </summary>
        /// <param name="role">The name of the role to add</param>
        public void AddRole(string role)
        {
            RoleLevels.Add(role, RoleLevels.Count);
        }
    }
}
