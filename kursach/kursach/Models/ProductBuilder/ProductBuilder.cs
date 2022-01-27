using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using kursach.ViewModels.Base;

namespace kursach.Models.ProductBuilder
{
    class ProductBuilder: IProductBuilder
    {
        Product result = new Product();
        public void Reset()
        {
            result = new Product();
        }

        public void SetNameOfProduct(string nameOfProduct)
        {
            result.NameOfProduct = nameOfProduct;
        }

        public void SetPathToPicture(string pathToPicture)
        {
            if (pathToPicture == null || pathToPicture == "")
            {
                result.Picture = Path.GetFullPath(@"images\error.png");
            }
            else
            {
                result.Picture = pathToPicture;
            }
        }

        public void SetCategory(string category)
        {
            result.Category = category;
        }

        public void SetRating(string rating)
        {
            result.Rating = rating;
        }

        public void SetPrice(string price)
        {
            result.Price = price;
        }

        public void SetNumber(string number)
        {
            result.Number = number;
        }

        public void SetSize(string size)
        {
            result.Size = size;
        }

        public void SetDescription(string description)
        {
            result.Description = description;
        }


        public Product GetResult()
        {
            return result;
        }
    }
}
