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
        public async Task<List<Points>> GetPoints()
        {
            List<Points> points = new();

            await Task.Run(async () =>
            {
                try
                {
                    List<PointContext> point = await _context.Point.ToListAsync();
                    foreach (var item in point)
                    {
                        points.Add(new Points
                        {
                            PointId = item.idpoints_of_issue,
                            PointIndex = item.index,
                            PointCity = item.city,
                            PointStreet = item.street,
                            PointHome = item.house_num
                        });
                    }
                }
                catch { }
            });
            return points;
        }
    }
}
