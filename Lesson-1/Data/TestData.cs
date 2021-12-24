﻿using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> Employees { get; } = new List<Employee>
        {
            new Employee
            {
                Id = 1, SecondName = "Пупкин", FirstName = "Василий", Patronymic = "Яковлевич", Age = 23, Income = 18000
            },
            new Employee
            {
                Id = 2, SecondName = "Чепкасов", FirstName = "Евгений", Patronymic = "Петрович", Age = 25,
                Income = 28000
            },
            new Employee
            {
                Id = 3, SecondName = "Валынин", FirstName = "Александр", Patronymic = "Евгеньевич", Age = 22,
                Income = 25000
            },
            new Employee
            {
                Id = 4, SecondName = "Убытков", FirstName = "Матвей", Patronymic = "Борисович", Age = 20, Income = 15000
            },
            new Employee
            {
                Id = 5, SecondName = "Быстроногов", FirstName = "Борис", Patronymic = "Алексеевич", Age = 18,
                Income = 10000
            },
            new Employee
            {
                Id = 6, SecondName = "Запаховский", FirstName = "Анатолий", Patronymic = "Маркович", Age = 47,
                Income = 55000
            }
        };
    }
}