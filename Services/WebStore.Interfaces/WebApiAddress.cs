﻿namespace WebStore.Interfaces;

public static class WebApiAddress
{
    public const string Products = "api/v1/products";
    public const string Orders = "api/v1/orders";
    public const string Employees = "api/v1/employees";
    public const string Values = "api/v1/values";
    
    public static class Identity
    {
        public const string Users = "api/v1/identity/users";
        public const string Roles = "api/v1/identity/roles";
    }
}