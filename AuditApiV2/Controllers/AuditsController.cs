using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using AuditApiV2.Models;

namespace AuditApiV2.Controllers
{
    [Route("api/[controller]")]
    public class AuditsController : Controller
    {
        private readonly IAuditRepository auditRepository;

        public AuditsController(IAuditRepository auditRepository)
        {
            this.auditRepository = auditRepository;
        }

        // GET api/values
        [HttpGet(Name = "getAudit")]
        public async Task<IEnumerable<AuditItem>> Get()
        {
            return await auditRepository.ListAuditItems();
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "getAuditById")]
        public async Task<AuditItem> Get(string id)
        {
            return await auditRepository.GetAuditItem(id);
        }

        // POST api/values
        [HttpPost(Name = "addAudit")]
        public async Task Post([FromBody]AuditItem auditItem)
        {
            await auditRepository.CreateAuditItem(auditItem);
        }

        // PUT api/values/5
        [HttpPut("{id}", Name = "updateAudit")]
        public async Task Put([FromBody]AuditItem auditItem)
        {
            await auditRepository.UpdateAuditItem(auditItem);
        }

        // DELETE api/values/5
        [HttpDelete("{id}", Name = "deleteAudit")]
        public async Task Delete(string id)
        {
            await auditRepository.DeleteAuditItem(id);
        }
    }
}
