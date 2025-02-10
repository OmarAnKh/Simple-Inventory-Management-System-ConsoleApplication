namespace SimpleInventoryManagementSystem.models;

public static class ProductPersistenceFactory
{
    public static IProductReader CreateReader()
    {
        return new ProductsFilePersistence();
    }

    public static IProductWriter CreateWriter()
    {
        return new ProductsFilePersistence();
    }
}