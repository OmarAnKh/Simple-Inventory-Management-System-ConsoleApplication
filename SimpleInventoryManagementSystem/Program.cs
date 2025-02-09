using System;
using SimpleInventoryManagementSystem.models;

namespace SimpleInventoryManagementSystem
{
    static class Program
    {
        private static void Main()
        {
            Products? products = Products.GetInstance();
            products.EditProduct("Cooc", 11, 3);
            products.Print();
        }
    }
}