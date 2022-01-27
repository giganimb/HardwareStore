using System;
using System.Collections.Generic;
using System.Text;

namespace kursach.Models.ProductBuilder
{
    class ProductWithDescription
    {
        public Product CreateProductWithDescription(string nameOfProduct, string pathToPicture, string category, string rating, string price, string number, string size, string description)
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
            productBuilder.SetDescription(description);

            return productBuilder.GetResult();
        }
    }
}
