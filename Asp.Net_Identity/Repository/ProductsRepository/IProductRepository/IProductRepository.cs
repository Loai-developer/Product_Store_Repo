using DataConnection;
using Repository.GeneralRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.ProductsRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<Product> ReadAllProducts(string SearchText);
        bool SaveProduct(Product product);
        bool DeleteProduct(Product product);
    }
}
