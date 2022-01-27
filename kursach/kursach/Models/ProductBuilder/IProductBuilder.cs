using System;
using System.Collections.Generic;
using System.Text;

namespace kursach.Models.ProductBuilder
{
    interface IProductBuilder
    {
        void Reset();
        void SetNameOfProduct(string nameOfProduct);
        void SetPathToPicture(string pathToPicture);
        void SetCategory(string category);
        void SetRating(string rating);
        void SetPrice(string price);
        void SetNumber(string number);
        void SetSize(string size);
        void SetDescription(string description);
    }
}
