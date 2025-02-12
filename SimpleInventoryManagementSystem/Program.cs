using SimpleInventoryManagementSystem.models;

namespace SimpleInventoryManagementSystem
{
    internal static class Program
    {
        private static int _operation;

        private static void PrintMenu()
        {
            do
            {
                Console.WriteLine("What is the operation you want to do?");
                Console.WriteLine("1 for Adding a new product to the inventory");
                Console.WriteLine("2 for Deleting a product");
                Console.WriteLine("3 for Updating a product");
                Console.WriteLine("4 for Displaying all products");
                Console.WriteLine("5 for Searching for a product");
                Console.WriteLine("0 for Exiting the program");
                Console.Write("Enter your choice: ");
                if (int.TryParse(Console.ReadLine(), out _operation))
                {
                    continue;
                }
                _operation = -1;
                Console.WriteLine("Invalid input!");
            } while (_operation is > 5 or < 0);
        }

        private static void Main()
        {
            var inventory = Inventory.GetInstance();

            do
            {
                PrintMenu();

                switch (_operation)
                {
                    case (int)OperationName.AddProduct:
                        AddNewProduct(inventory);
                        break;
                    case (int)OperationName.DeleteProduct:
                        DeleteAProduct(inventory);
                        break;
                    case (int)OperationName.UpdateProduct:
                        UpdateProduct(inventory);
                        break;
                    case (int)OperationName.DisplayProducts:
                        inventory.Print();
                        break;
                    case (int)OperationName.SearchProduct:
                        SearchForAProduct(inventory);
                        break;
                    case (int)OperationName.Exit:
                        Console.WriteLine("Exiting the program...");
                        break;
                    default:
                        Console.WriteLine("Invalid operation. Please try again.");
                        break;
                }

                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");

            } while (_operation != 0);
        }

        private static void SearchForAProduct(Inventory inventory)
        {
            Console.Write("Enter the product name to search for: ");
            var searchTerm = Console.ReadLine();
            inventory.Search(searchTerm);
        }

        private static void DeleteAProduct(Inventory inventory)
        {
            Console.Write("Enter the product name to delete: ");
            var deleteProductName = Console.ReadLine();
            if (!inventory.DeleteProduct(deleteProductName))
            {
                Console.WriteLine("Product does not exist.");
            }
        }

        private static void UpdateProduct(Inventory inventory)
        {
            Console.Write("Enter the product name to update: ");
            var updateProductName = Console.ReadLine();
            Console.Write("Enter the new product name: ");
            var newProductName = Console.ReadLine();
            Console.Write("Enter the new product quantity: ");
            var updateProductQuantity = int.Parse(Console.ReadLine() ?? string.Empty);
            Console.Write("Enter the new product price: ");
            var updateProductPrice = int.Parse(Console.ReadLine() ?? string.Empty);
            if (!inventory.EditProduct(updateProductName, newProductName, updateProductQuantity, updateProductPrice))
            {
                Console.WriteLine("Product does not exist.");
            }
            
        }

        private static void AddNewProduct(Inventory inventory)
        {
            Console.Write("Enter the product name: ");
            var addProductName = Console.ReadLine();
            Console.Write("Enter the product quantity: ");
            var addProductQuantity = int.Parse(Console.ReadLine() ?? string.Empty);
            Console.Write("Enter the product price: ");
            var addProductPrice = int.Parse(Console.ReadLine() ?? string.Empty);
            inventory.AddProduct(addProductName, addProductQuantity, addProductPrice);
        }
    }
}
