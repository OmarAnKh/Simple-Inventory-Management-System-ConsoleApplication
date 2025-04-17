using SimpleInventoryManagementSystem.Models.Repositories.File;
using SimpleInventoryManagementSystem.Models.Repositories.Interfaces;
using SimpleInventoryManagementSystem.models.Repositories.MongoDB;
using SimpleInventoryManagementSystem.models.Repositories.SQLServer;

namespace SimpleInventoryManagementSystem.models;

static class ProductPersistenceFactory
{
    public static IProductPersistence CreatePersistence(RepositoriesType type)
    {
        if (type == RepositoriesType.MongoDb)
        {
            return new ProductMongoDbPersistence(Environment.GetEnvironmentVariable("MONGODBCONNECTIONSTRING"),
                Environment.GetEnvironmentVariable("MONGODBDATABASENAME"),
                Environment.GetEnvironmentVariable("MONGODBCOLLECTIONNAME"));
        }
        else if (type == RepositoriesType.SqlServer)
        {
            return new ProductsSqlServerPersistence(Environment.GetEnvironmentVariable("SQLSERVERCONNECTIONSTRING"));
        }
        else
        {
            return new ProductsFilePersistence(Environment.GetEnvironmentVariable("FILECONNECTIONSTRING"));
        }
    }
}