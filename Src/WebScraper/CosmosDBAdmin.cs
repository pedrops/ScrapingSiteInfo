using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebScraper
{
    public interface ICosmosEntity
    {
        string Id { get;  }
        string PartitionKey { get; }

    }

    public static class CosmosDBAdmin 
    {

        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        public static async Task<Database> CreateOrGetDatabaseAsync(CosmosClient cosmosClient, string databaseId)
        {
            return await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }

        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        public static async Task<Database> CreateOrGetDatabaseAsync(string EndpointUri, string PrimaryKey, string databaseId, string AppName )
        {
            var cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = AppName });
            return await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }



        public static async Task<int> GetContainerThroughputAsync(this Container container)
        {
            var throughput = await container.ReadThroughputAsync();
            if (throughput.HasValue) return throughput.Value;
            return -1;
        }

        public static async Task ScaleContainerAsync(this Container container, int newThroughput)
        {
            await container.ReplaceThroughputAsync(newThroughput);
        }

        public static async Task<Container> CreateContainerAsync(this Database database, string containerId, string partitionKey, int throughput)
        {
            var container = await database.CreateContainerIfNotExistsAsync(containerId, partitionKey, throughput);
            return container;
        }

        public static async Task SaveAsync<T>(this Container container, T fam) where T:ICosmosEntity
        {
            try
            {
                await container.UpsertItemAsync(fam);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                await container.AddAsync(fam);
            }
        }


        public static async Task UpdateAsync<T>(this Container container, T fam) where T:ICosmosEntity
        {
            await container.ReplaceItemAsync<T>(fam, fam.Id, new PartitionKey(fam.PartitionKey));
        }

        public static async Task DeleteAsync<T>(this Container container, T fam) where T:ICosmosEntity
        {
            await container.DeleteItemAsync<T>(fam.Id, new PartitionKey(fam.PartitionKey));
        }

        public static async Task<T> GetById<T> (this Container container, string id, string partitionKeyName) where T:ICosmosEntity
        {
            var response = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKeyName));
            var itemBody = response.Resource;
            return itemBody;
        }

        public static async Task AddAsync<T>(this Container container, T andersenFamily) where T:ICosmosEntity
{
try
{
// Read the item to see if it exists.  
                await container.ReadItemAsync<T>(andersenFamily.Id, new PartitionKey(andersenFamily.PartitionKey));
            }
            catch(CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
// Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
                await container.CreateItemAsync<T>(andersenFamily, new PartitionKey(andersenFamily.PartitionKey));
            }
        }

        public static async Task<IEnumerable<T>> QueryItemsAsync<T>(this Container container,  string sqlQueryText ) where T:ICosmosEntity
        {
            var queryDefinition = new QueryDefinition(sqlQueryText);
            var queryResultSetIterator = container.GetItemQueryIterator<T>(queryDefinition);

            var lst = new List<T>();
            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var family in currentResultSet)  lst.Add(family );
            }

            return lst;
        }


    }
}
