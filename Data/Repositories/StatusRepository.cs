using ECommerce1.Models;
using System.Collections.Generic;

namespace ECommerce1.Data.Repositories
{
    public class StatusRepository
    {
        public IEnumerable<Status> GetStatuses()
        {
            List<Status> statuses = new List<Status>()
            {
                new Status
                {
                    Id = 1,
                    Title = "Active"
                },
                new Status
                {
                    Id = 2,
                    Title = "Out-of-Stock"
                }
            };

            return statuses;
        }
    }
}
