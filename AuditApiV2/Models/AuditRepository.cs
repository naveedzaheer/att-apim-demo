using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditApiV2.Models
{
    public class AuditRepository : IAuditRepository
    {
        private CloudTable auditTable;

        public AuditRepository(string storageConnectionString)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            auditTable = tableClient.GetTableReference("audits");
            auditTable.CreateIfNotExistsAsync().Wait();
        }

        public async Task CreateAuditItem(AuditItem auditItem)
        {
            AuditEntity audit = new AuditEntity(auditItem.Id);
            audit.Id = auditItem.Id;
            audit.Data = auditItem.Data;
            audit.Status = auditItem.Status;

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(audit);

            // Execute the insert operation.
            await auditTable.ExecuteAsync(insertOperation);
        }

        public async Task DeleteAuditItem(string itemId)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<AuditEntity>(itemId, itemId);

            // Execute the retrieve operation.
            TableResult retrievedResult = await auditTable.ExecuteAsync(retrieveOperation);
            AuditEntity auditEntity = (AuditEntity)retrievedResult.Result;
            TableOperation deleteOperation = TableOperation.Delete(auditEntity);

            // Execute the insert operation.
            await auditTable.ExecuteAsync(deleteOperation);
        }

        public async Task<AuditItem> GetAuditItem(string itemId)
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<AuditEntity>(itemId, itemId);

            // Execute the retrieve operation.
            TableResult retrievedResult = await auditTable.ExecuteAsync(retrieveOperation);
            AuditEntity auditEntity = (AuditEntity)retrievedResult.Result;
            return new AuditItem(auditEntity.Id, auditEntity.Data, auditEntity.Status);
        }

        public async Task<IEnumerable<AuditItem>> ListAuditItems()
        {
            List<AuditItem> audits = new List<AuditItem>();
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<AuditEntity> query = new TableQuery<AuditEntity>();

            // Print the fields for each Audit.
            TableContinuationToken token = null;
            TableQuerySegment<AuditEntity> segment = await auditTable.ExecuteQuerySegmentedAsync(query, token);
            foreach (AuditEntity auditEntity in segment)
            {
                audits.Add(new AuditItem(auditEntity.Id, auditEntity.Data, auditEntity.Status));
            }

            return audits;
        }

        public async Task UpdateAuditItem(AuditItem auditItem)
        {
            AuditEntity audit = new AuditEntity(auditItem.Id);
            audit.Id = auditItem.Id;
            audit.Data = auditItem.Data;
            audit.Status = auditItem.Status;

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(audit);

            // Execute the insert operation.
            await auditTable.ExecuteAsync(insertOrReplaceOperation);
        }
    }
}
