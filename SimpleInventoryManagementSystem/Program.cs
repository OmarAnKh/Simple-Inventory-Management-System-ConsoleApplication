using System;
using SimpleInventoryManagementSystem.models;

namespace SimpleInventoryManagementSystem
{
    static class Program
    {
        private static void Main()
        {
            Products? products = Products.GetInstance();
            products.DeleteProduct("Cooc");
            products.Print();
        }
    }
}