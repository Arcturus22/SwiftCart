using System.Runtime.CompilerServices;

namespace SwiftCart.Utility
{
    public static class SD 
    {
        // SD = Static Details or Static Data
        public static string Role_Admin = "Admin";
        public static string Role_Customer = "Customer";

        public static readonly IEnumerable<string> Roles = new[] { Role_Admin, Role_Customer };

    }
}
