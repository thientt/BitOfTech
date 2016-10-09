namespace BitOfTech.WebApi22.Utility
{
    public class RouteNameConfig
    {
        public const string API_ROLE = "api/roles";
        public const string API_USER = "api/accounts";

        public const string ROUTE_TEMPLATE_ID = "{id}";
        public const string ROUTE_TEMPLATE_ALL = "all";
        public const string ROUTE_TEMPLATE_CREATE = "create";
        public const string ROUTE_TEMPLATE_USER = "users";

        public const string ROUTE_NAME_GET_ROLE_BY_ID = "GetRoleById";
        public const string ROUTE_NAME_GET_ALL_ROLES = "GetAll";

        public const string ROUTE_GET_USER_BY_ID = "user/{id}";
        public const string ROUTE_GET_USER_BY_USER_NAME = "user/{username}";
        public const string ROUTE_NAME_GET_USER_BY_ID = "GetUserById";

        public const string ROUTE_CONFIRM_EMAIL = "ConfirmEmail";
        public const string ROUTE_NAME_CONFIRM_EMAIL = "ConfirmEmailRoute";

        public const string ROUTE_CHANGE_PASSWORD = "ChangePassword";
        public const string ROUTE_GET_ROLES_BY_USER_ID = "user/{id}/roles";
    }
}