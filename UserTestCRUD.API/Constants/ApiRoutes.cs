namespace UserTestCRUD.API.Constants
{
    public static class ApiRoutes
    {
        public static class Users
        {
            public const string GetUsers = "users";

            public const string GetUserById = "users/{userId:int}";

            public const string CreateUser = "users";

            public const string UpdateUser = "users/{userId:int}";

            public const string RemoveUser = "users/{userId:int}";

            public const string AddRoleToUser = "users/{userId:int}/roles";
        }

        public static class Auth
        {
            public const string Login = "auth/login";
        }
    }
}
