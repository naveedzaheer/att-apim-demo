using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditApiV2.Models
{
    public interface IAuditRepository
    {
        Task CreateAuditItem(AuditItem auditItem);
        Task UpdateAuditItem(AuditItem auditItem);
        Task DeleteAuditItem(string itemId);
        Task<AuditItem> GetAuditItem(string itemId);
        Task<IEnumerable<AuditItem>> ListAuditItems();
    }
}
