using DataConnection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.ProductService
{
    public interface IProductServiceLayer
    {
        IEnumerable<Product> ReadProducts(string SearchText);
        bool SaveProduct(Product product);
        bool DeleteProduct(Product product);
    }
}
