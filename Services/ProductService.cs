using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Write_Wash.Services
{
    public class ProductService
    {
        private readonly DataContext _context;
        public ProductService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProducts()
        {
            List<Product> products = new();

            await Task.Run(async () =>
            {
                try
                {
                    List<ProductContext> product = await _context.Product.ToListAsync();
                    
                    List<ManufacturesContext> pmanufactures = await _context.Manufacturers.ToListAsync();

                    foreach (var item in product)
                    {
                        products.Add(new Product
                        {
                            Image = item.ProductPhoto == string.Empty ? "picture.png" : item.ProductPhoto,
                            Title = item.ProductName,
                            Description = item.ProductDescription,
                            Manufacturer = pmanufactures.SingleOrDefault(pm => pm.idmanufacturer == item.ProductManufacturer).name,
                            Price = item.ProductCost,
                            Discount = item.ProductDiscountAmount,
                            ProductCount = 1
                        }); 
                    }
                }
                catch { }
            });
            return products;
        }
        
    }
}
