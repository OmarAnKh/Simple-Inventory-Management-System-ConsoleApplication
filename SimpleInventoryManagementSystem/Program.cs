using System;
using SimpleInventoryManagementSystem.models;

namespace SimpleInventoryManagementSystem
{
    static class Program
    {
        private static void Main()
        {
            Products? products = Products.GetInstance();
            products.Search("Choocola");
            products.EditProduct("Choocola","d",10,2);
            products.Search("Choocola");
        }
    }
}