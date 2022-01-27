using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace kursach.Models
{
    public class Product
    {
        [Key]
        [Required(ErrorMessage = "Отсутствует название продукта")]
        [MaxLength(30, ErrorMessage = "Слишком длинное имя продукта")]
        public string NameOfProduct { get; set; }
        public string Picture { get; set; }

        [Required(ErrorMessage = "Отсутствует категория")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Отсутствует рейтинг")]
        [MaxLength(3, ErrorMessage = "Превышено допустимое количество символов в рейтинге!")]
        [RegularExpression(@"[0-9]{1}\,[0-9]{1}", ErrorMessage = "Неверный формат рейтинга. Правильный формат: 6,7")]
        public string Rating { get; set; }

        [Required(ErrorMessage = "Отсутствует цена")]
        [MaxLength(8, ErrorMessage = "Слишком большая цена! Введите меньшую")]
        [RegularExpression(@"[0-9]{1,5}\,[0-9]{1,2}", ErrorMessage = "Неверный формат цены. Правильный формат: 12356,67")]
        public string Price { get; set; }

        [Required(ErrorMessage = "Отсутствует количество")]
        [MaxLength(3, ErrorMessage = "Слишком большое количество! Введите меньшее")]
        [RegularExpression(@"[0-9]{1,3}", ErrorMessage = "Неверный формат количества. Правильный формат: 777")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Отсутствует размер")]
        [MaxLength(7, ErrorMessage = "Слишком большой размер! Введите меньший")]
        [RegularExpression(@"[0-9]{1,3}[x\|х][0-9]{1,3}", ErrorMessage = "Неверный формат размера. Правильный формат: 200х300")]
        public string Size { get; set; }

        public string Description { get; set; }

        public List<SoldProduct> SoldProducts { get; set; }

        public Product() { }
        public Product(string nameOfProduct, string pathToPicture, string category, string rating, string price, string number, string size, string description)
        {
            this.NameOfProduct = nameOfProduct;
            this.Category = category;
            if (pathToPicture == null || pathToPicture == "")
            {
                this.Picture = Path.GetFullPath(@"images\error.png");
            }
            else
            {
                this.Picture = pathToPicture;
            }

            //----------------------------------------BAD USE----------------------------------------------------
            //if (Category == "Телефон")
            //{
            //    //pathToPicture = @"D:\University\2 курс\2-ой сем\ООП\lab6_7\lab6_7\bin\Debug\netcoreapp3.1\images\phone.jpg";
            //    Picture = Path.GetFullPath(@"images\phone.jpg");
            //}
            //else if (Category == "Ноутбук")
            //{
            //    //pathToPicture = @"D:\University\2 курс\2-ой сем\ООП\lab6_7\lab6_7\bin\Debug\netcoreapp3.1\images\notebook.jpg";
            //    Picture = Path.GetFullPath(@"images\notebook.jpg");
            //}
            //else if (Category == "Утюг")
            //{
            //    //pathToPicture = @"D:\University\2 курс\2-ой сем\ООП\lab6_7\lab6_7\bin\Debug\netcoreapp3.1\images\iron.jpg";
            //    Picture = Path.GetFullPath(@"images\iron.jpg");
            //}
            //else
            //{
            //    Picture = Path.GetFullPath(@"images\error.png");
            //}
            //----------------------------------------------------------------------------------------------

            this.Rating = rating;
            this.Price = price;
            this.Number = number;
            this.Size = size;
            this.Description = description;
        }
    }
}
