﻿namespace WebStore.Services;

public static class WebAddresses
{
    public const string Orders = "api/v1/orders";
    public const string Products = "api/v1/products";
    public const string Employees = "api/v1/employees";
    public const string Values = "api/v1/values";
    public const string Console = "api/v1/console";

    public static class Identity
    {
        public const string Users = "api/v1/identity/users";
        public const string Roles = "api/v1/identity/roles";
    }
}