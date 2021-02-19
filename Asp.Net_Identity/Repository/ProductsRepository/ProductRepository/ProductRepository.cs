using DataConnection;
using Repository.GeneralRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Repository.ProductsRepository;

namespace Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context)
            : base(context)
        {

        }

        public IEnumerable<Product> ReadAllProducts(string SearchText)
        {
            var TrimmedSearchText = SearchText == "" ? SearchText : SearchText.Trim();
            return context.Products.Where(s => TrimmedSearchText == null? true : 
            (s.Name.Contains(TrimmedSearchText) || s.Price.ToString().Contains(TrimmedSearchText))).OrderBy(s => s.Id).ToList();
        }

        public bool SaveProduct(Product product)
        {
            try
            {
                if (product.Id == 0)
                {
                    var ProductObj = new Product();
                    ProductObj.Name = product.Name;
                    ProductObj.Price = product.Price;
                    ProductObj.Photo = product.Photo;
                    ProductObj.LastUpdated = DateTime.Now;
                    context.Products.Add(ProductObj);
                }
                else
                {
                    var OldProduct = context.Products.Find(product.Id);
                    OldProduct.Name = product.Name;
                    OldProduct.Price = product.Price;
                    OldProduct.Photo = product.Photo;
                    OldProduct.LastUpdated = DateTime.Now;
                }
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProduct(Product product)
        {
            try
            {
                var OldProduct = context.Products.Find(product.Id);
                context.Products.Remove(OldProduct);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
