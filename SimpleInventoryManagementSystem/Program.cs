using System;
using SimpleInventoryManagementSystem.models;

namespace SimpleInventoryManagementSystem
{
    static class Program
    {
        static int operation = 0;

        static void PrintMenu()
        {
            do
            {
                Console.WriteLine("What is the operation you want to do?");
                Console.WriteLine("1 for Adding a new product to the inventory");
                Console.WriteLine("2 for Displaying all products");
                Console.WriteLine("3 for Updating a product");
                Console.WriteLine("4 for Deleting a product");
                Console.WriteLine("5 for Searching for a product");
                Console.WriteLine("0 for Exiting the program");
                Console.Write("Enter your choice: ");
                int.TryParse(Console.ReadLine(), out operation);
            } while (operation > 5 || operation < 0);
        }

        private static void Main()
        {
            Inventory? inventory = Inventory.GetInstance();

            do
            {
                PrintMenu();

                switch (operation)
                {
                    case 1:
                        // Adding a new product
                        Console.Write("Enter the product name: ");
                        string? addProductName = Console.ReadLine();
                        Console.Write("Enter the product quantity: ");
                        int addProductQuantity = int.Parse(Console.ReadLine() ?? string.Empty);
                        Console.Write("Enter the product price: ");
                        int addProductPrice = int.Parse(Console.ReadLine());
                        inventory.AddProduct(addProductName, addProductQuantity, addProductPrice);
                        break;
                    case 2:
                        // Displaying all products
                        inventory.Print();
                        break;
                    case 3:
                        // Updating an existing product
                        Console.Write("Enter the product name to update: ");
                        string? updateProductName = Console.ReadLine();
                        Console.Write("Enter the new product name: ");
                        string? newProductName = Console.ReadLine();
                        Console.Write("Enter the new product quantity: ");
                        int updateProductQuantity = int.Parse(Console.ReadLine());
                        Console.Write("Enter the new product price: ");
                        int updateProductPrice = int.Parse(Console.ReadLine());
                        inventory.EditProduct(updateProductName, newProductName, updateProductQuantity, updateProductPrice);
                        break;
                    case 4:
                        // Deleting a product
                        Console.Write("Enter the product name to delete: ");
                        string deleteProductName = Console.ReadLine();
                        inventory.DeleteProduct(deleteProductName);
                        break;
                    case 5:
                        // Searching for a product
                        Console.Write("Enter the product name to search for: ");
                        string searchTerm = Console.ReadLine();
                        inventory.Search(searchTerm);
                        break;
                    case 0:
                        Console.WriteLine("Exiting the program...");
                        break;
                    default:
                        Console.WriteLine("Invalid operation. Please try again.");
                        break;
                }

                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");

            } while (operation != 0);
        }
    }
}
