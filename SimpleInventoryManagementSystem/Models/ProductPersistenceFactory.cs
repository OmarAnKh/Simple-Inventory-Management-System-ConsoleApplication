using SimpleInventoryManagementSystem.Models.Repositories.File;
using SimpleInventoryManagementSystem.Models.Repositories.Interfaces;
using SimpleInventoryManagementSystem.models.Repositories.MongoDB;
using SimpleInventoryManagementSystem.models.Repositories.SQLServer;

namespace SimpleInventoryManagementSystem.models;

static class ProductPersistenceFactory
{
    public static IProductPersistence CreatePersistence()
    {
        return new ProductsSqlServerPersistence();
    }
}