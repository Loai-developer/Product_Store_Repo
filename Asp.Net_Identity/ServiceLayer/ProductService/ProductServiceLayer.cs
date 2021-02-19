using DataConnection;
using Repository.GeneralRepository;
using System.Collections.Generic;

namespace ServiceLayer.ProductService
{
    public class ProductServiceLayer :IProductServiceLayer
    {
        private readonly IUnitOfWork _UnitOfWork;
        public ProductServiceLayer(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IEnumerable<Product> ReadProducts(string SearchText)
        {
            return _UnitOfWork.Products.ReadAllProducts(SearchText);
        }

        public bool SaveProduct(Product product)
        {
            return _UnitOfWork.Products.SaveProduct(product);
        }

        public bool DeleteProduct(Product product)
        {
            return _UnitOfWork.Products.DeleteProduct(product);
        }
    }
}
