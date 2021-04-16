using Microsoft.EntityFrameworkCore;
using SampleCommonInfo.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCommonInfo
{
    public class CompanyInstanceRepository
    {
        private ETLWorkerContext _ETLWorkerContext = new ETLWorkerContext();
        public async ValueTask<bool> AddOrUpdate(IEnumerable<Company_Worker> workers, int id_company)
        {
            var allVersion = workers.Select(w => w.id_worker);
            await _ETLWorkerContext.ETL_Workers.Where(e => allVersion.Contains(e.id_worker)).ToDictionaryAsync(e => e.type);

            var insertGroups = workers.Where(w => w.id <= 0);
            var changeGroups = workers.Where(w => w.id > 0);
            var changeIDs = changeGroups.Select(c => c.id);
            var currentChanges = await _ETLWorkerContext.Company_Workers.Where(w => w.id_company == id_company && changeIDs.Contains(w.id)).ToListAsync();
            foreach (var currentChange in currentChanges)
            {
                currentChange.id_worker = workers.First(w => w.id == currentChange.id).id_worker;
                currentChange.quantity = workers.First(w => w.id == currentChange.id).quantity;
            }
            foreach (var insertItem in insertGroups)
            {
                insertItem.id_company = id_company;
                insertItem.deleted = false;
            }
            await _ETLWorkerContext.Company_Workers.AddRangeAsync(insertGroups);
            await _ETLWorkerContext.SaveChangesAsync();
            return true;
        }
        public async ValueTask<IEnumerable<ResponseModel>> GetInstanceWorker(int id_company)
        {
            return await _ETLWorkerContext.Company_Workers.Join(
                _ETLWorkerContext.ETL_Workers,
                i => i.id_worker,
                w => w.id_worker,
                (i, w) => new ResponseModel
                {
                    checksum = w.checksum,
                    deleted = i.deleted,
                    file_name = w.file_name,
                    id = i.id,
                    id_company = i.id_company,
                    id_worker = w.id_worker,
                    location = w.location,
                    quantity = i.quantity,
                    type = w.type,
                    version_name = w.version_name
                }).Where(w=>w.id_company==id_company).ToListAsync();
        }
        public async ValueTask<IEnumerable<ETL_Worker>> GetWorker()
        {
            return await _ETLWorkerContext.ETL_Workers.Select(e => e).ToListAsync();
        }
    }
}
