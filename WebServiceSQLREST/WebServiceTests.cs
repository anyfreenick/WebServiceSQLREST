using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebServiceSQLREST
{
    [TestClass]
    public class WebServiceTests
    {
        private Product product;

        [TestMethod]
        public void CreatePruduct()
        {
            product = new Product("doberman", 75.3);
            Product.CreateProduct(product);
            Assert.IsTrue(product.ProductExist(), "Error while product creating");
            product.DeleteProduct();
            Assert.IsFalse(product.ProductExist(), "Error while product deleting");
        }
    }
}