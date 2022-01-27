using System;
using System.Collections.Generic;
using System.Text;

namespace kursach.Models.ProductBuilder
{
    class ProductWithoutDescription
    {
        public Product CreateProductWithoutDescription(string nameOfProduct, string pathToPicture, string category, string rating, string price, string number, string size)
        {
            ProductBuilder productBuilder = new ProductBuilder();
            productBuilder.Reset();

            productBuilder.SetNameOfProduct(nameOfProduct);
            productBuilder.SetPathToPicture(pathToPicture);
            productBuilder.SetCategory(category);
            productBuilder.SetRating(rating);
            productBuilder.SetPrice(price);
            productBuilder.SetNumber(number);
            productBuilder.SetSize(size);
            productBuilder.SetDescription(null);

            return productBuilder.GetResult();
        }
    }
}
