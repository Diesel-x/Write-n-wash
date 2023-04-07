using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Services
{
    internal class PointService
    {
        private readonly DataContext _context;
        public PointService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<PointContext>> GetPoints()
        {
            List<PointContext> points = new();

            await Task.Run(async () =>
            {
                try
                {
                    List<PointContext> point = await _context.Point.ToListAsync();
                    foreach (var item in point)
                    {
                        points.Add(new PointContext
                        {
                            idpoints_of_issue = item.idpoints_of_issue,
                            index = item.index,
                            city = item.city,
                            street = item.street,
                            house_num = item.house_num
                        });
                    }
                }
                catch { }
            });
            return points;
        }
    }
}
