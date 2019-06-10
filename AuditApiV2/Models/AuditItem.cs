using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditApiV2.Models
{
    public class AuditItem
    {
        public AuditItem(string id, string data, string status)
        {
            this.Id = id;
            this.Data = data;
            this.Status = status;
        }

        public AuditItem() { }

        public string Id { get; set; }

        public string Data { get; set; }

        public string Status { get; set; }
    }

    public class AuditEntity : TableEntity
    {
        public AuditEntity(string id)
        {
            this.PartitionKey = id;
            this.RowKey = id;
        }

        public AuditEntity() { }

        public string Id { get; set; }

        public string Data { get; set; }

        public string Status { get; set; }
    }
}
