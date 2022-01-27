using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace kursach.Models
{
    public class SoldProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SoldProductID { get; set; }
        [Required(ErrorMessage = "Отсутствует имя продукта")]
        public string NameOfProduct { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
        [Required(ErrorMessage = "Отсутствует количество")]
        [MaxLength(3, ErrorMessage = "Слишком большое количество! Введите меньшее")]
        [RegularExpression(@"[0-9]{1,3}", ErrorMessage = "Неверный формат количества. Правильный формат: 777")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Отсутствует дата продажи")]
        [RegularExpression(@"[0-9]{4}\-[0-9]{2}\-[0-9]{2}", ErrorMessage = "Неверный формат даты. Правильный формат: 2021-05-20")]
        public string DateOfSale { get; set; }
        [Required(ErrorMessage = "Отсутствует время продажи")]
        [RegularExpression(@"[0-9]{2}\:[0-9]{2}", ErrorMessage = "Неверный формат времени. Правильный формат: 15:40")]
        public string TimeOfSale { get; set; }
        [Required(ErrorMessage = "Отсутствует логин пользователя")]
        public string Login { get; set; }
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }

        public SoldProduct() { }

        public SoldProduct(string nameOfProduct, string category, string price, string number, string dateOfSale, string timeOfSale, string login)
        {
            this.NameOfProduct = nameOfProduct;
            this.Category = category;
            this.Price = price;
            this.Number = number;
            this.DateOfSale = dateOfSale;
            this.TimeOfSale = timeOfSale;
            this.Login = login;
        }
    }
}
