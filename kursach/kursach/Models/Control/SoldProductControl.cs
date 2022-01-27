using kursach.Models.Control.Repository;
using kursach.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace kursach.Models.Control
{
    public class SoldProductControl : IRepository<SoldProduct>
    {
        private ApplicationContext db;
        public SoldProductControl(ApplicationContext context)
        {
            this.db = context;
        }

        public  List<SoldProduct> GetAll()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<SoldProduct> result = db.SoldProducts.ToList();
                return result;
            }
        }

        public string Create(SoldProduct item)
        {
            string result = null;
            using (ApplicationContext db = new ApplicationContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (IsProductExist(item.NameOfProduct))
                        {
                            if (IsUserExist(item.Login))
                            {

                                SoldProduct tempObjectForValidation = new SoldProduct(item.NameOfProduct, item.Category, item.Price, item.Number, item.DateOfSale, item.TimeOfSale, item.Login);


                                string patternForNumber = @"[0-9]{1,3}";
                                string patternForDateOfSale = @"[0-9]{4}\-[0-9]{2}\-[0-9]{2}";
                                string patternForTimeOfSale = @"[0-9]{2}\:[0-9]{2}";


                                if (item.NameOfProduct == null || item.NameOfProduct == "")
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_product_name, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                    return null;
                                }


                                if (item.Login == null || item.Login == "")
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_User_login_missing, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                    return null;
                                }


                                if (item.Number != null && item.Number != "")
                                {
                                    if (!Regex.IsMatch(item.Number, patternForNumber, RegexOptions.IgnoreCase))
                                    {
                                        MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_quantity_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                        return null;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_number, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                    return null;
                                }


                                if (item.DateOfSale != null && item.DateOfSale != "")
                                {
                                    if (!Regex.IsMatch(item.DateOfSale, patternForDateOfSale, RegexOptions.IgnoreCase))
                                    {
                                        MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_date_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                        return null;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Sale_date_missing, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                    return null;
                                }


                                if (item.TimeOfSale != null && item.TimeOfSale != "")
                                {
                                    if (!Regex.IsMatch(item.TimeOfSale, patternForTimeOfSale, RegexOptions.IgnoreCase))
                                    {
                                        MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_time_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                        return null;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Sale_time_is_missing, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                    return null;
                                }


                                var results = new List<ValidationResult>();
                                var context = new ValidationContext(tempObjectForValidation);
                                if (!Validator.TryValidateObject(tempObjectForValidation, context, results, true))
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Values_entered_incorrectly, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                    return null;
                                }

                                DateTime dt1;
                                TimeSpan dt2;
                                bool tempOfDate = DateTime.TryParse(item.DateOfSale, out dt1);
                                bool tempOfTime = TimeSpan.TryParse(item.TimeOfSale, out dt2);

                                if (tempOfDate)
                                {
                                    if (tempOfTime)
                                    {
                                        string WWN = WorkWithNumber(item.NameOfProduct, item.Number);
                                        if (WWN == null)
                                        {
                                            Product product = db.Products.FirstOrDefault(el => el.NameOfProduct == item.NameOfProduct);
                                            tempObjectForValidation.Category = product.Category;
                                            tempObjectForValidation.Price = product.Price;
                                            db.SoldProducts.Add(tempObjectForValidation);
                                            db.SaveChanges();
                                            transaction.Commit();
                                            result = Views.Windows.Resources.Languages.Lang.m_Product_added;
                                        }
                                        else
                                        {
                                            result = WWN;
                                        }
                                    }
                                    else
                                    {
                                        result = Views.Windows.Resources.Languages.Lang.m_Invalid_time_of_sale;
                                    }
                                }
                                else
                                {
                                    result = Views.Windows.Resources.Languages.Lang.m_Invalid_date_of_sale;
                                }

                            }
                            else
                            {
                                result = Views.Windows.Resources.Languages.Lang.m_The_selected_user_does_not_exist;
                            }
                        }
                        else
                        {
                            result = Views.Windows.Resources.Languages.Lang.m_The_selected_product_does_not_exist;
                        }
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            return result;
        }

        public static bool IsProductExist(string nameOfproduct)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Products.Any(el => el.NameOfProduct == nameOfproduct);
                return checkIsExist;
            }
        }

        public static bool IsUserExist(string login)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Users.Any(el => el.Login == login);
                return checkIsExist;
            }
        }

        public static string WorkWithNumber(string name, string number)
        {
            string result = null;
            
            using (ApplicationContext db = new ApplicationContext())
            {
                Product product = db.Products.FirstOrDefault(el => el.NameOfProduct == name);
                string str = product.Number;
                int ProductNumber = Convert.ToInt32(str);

                if(ProductNumber >= Convert.ToInt32(number))
                {
                    ProductNumber = ProductNumber - Convert.ToInt32(number);
                    product.Number = Convert.ToString(ProductNumber);
                    db.SaveChanges();
                }
                else
                {
                    result = Views.Windows.Resources.Languages.Lang.m_The_selected_quantity_of_products_exceeds_the_allowable; 
                }
            }
            return result;
        }

        public string Delete(string nameOfSoldProduct)
        {
            string result = Views.Windows.Resources.Languages.Lang.m_This_product_does_not_exist;
            using (ApplicationContext db = new ApplicationContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool checkIsExist = db.SoldProducts.Any(el => el.NameOfProduct == nameOfSoldProduct);
                        if (checkIsExist)
                        {
                            SoldProduct deletingSoldProduct = new SoldProduct();
                            var search = db.SoldProducts.Where(el => el.NameOfProduct == nameOfSoldProduct);
                            foreach (SoldProduct p in search)
                            {
                                deletingSoldProduct = p;
                                break;
                            }
                            db.SoldProducts.Remove(deletingSoldProduct);
                            db.SaveChanges();
                            transaction.Commit();
                            result = Views.Windows.Resources.Languages.Lang.m_Product_removed;
                        }
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            return result;
        }

        public static List<SoldProduct> SearchProduct(string nameOfProduct, string user, string number, string date, string startPrice, string finishPrice)
        {
            List<SoldProduct> resultSearch = new List<SoldProduct>();
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.SoldProducts.Any();
                if (checkIsExist)
                {
                    var allProducts = db.SoldProducts;
                    resultSearch.AddRange(allProducts);
                    List<SoldProduct> temp = new List<SoldProduct>();

                    if (nameOfProduct != null && nameOfProduct != "")
                    {
                        temp.Clear();
                        Regex regex = new Regex(nameOfProduct, RegexOptions.IgnoreCase);
                        foreach (var x in resultSearch)
                        {
                            MatchCollection matches = regex.Matches(x.NameOfProduct);
                            if (matches.Count > 0)
                            {
                                temp.Add(new SoldProduct(x.NameOfProduct, x.Category, x.Price, x.Number, x.DateOfSale, x.TimeOfSale, x.Login));
                            }
                        }
                        resultSearch.Clear();
                        resultSearch.AddRange(temp);
                    }

                    if (user != null && user != "")
                    {
                        temp.Clear();
                        Regex regex = new Regex(user, RegexOptions.IgnoreCase);
                        foreach (var x in resultSearch)
                        {
                            MatchCollection matches = regex.Matches(x.Login);
                            if (matches.Count > 0)
                            {
                                temp.Add(new SoldProduct(x.NameOfProduct, x.Category, x.Price, x.Number, x.DateOfSale, x.TimeOfSale, x.Login));
                            }
                        }
                        resultSearch.Clear();
                        resultSearch.AddRange(temp);
                    }

                    if (number != null && number != "")
                    {
                        temp.Clear();
                        Regex regex = new Regex(number, RegexOptions.IgnoreCase);
                        foreach (var x in resultSearch)
                        {
                            MatchCollection matches = regex.Matches(x.Number);
                            if (matches.Count > 0)
                            {
                                temp.Add(new SoldProduct(x.NameOfProduct, x.Category, x.Price, x.Number, x.DateOfSale, x.TimeOfSale, x.Login));
                            }
                        }
                        resultSearch.Clear();
                        resultSearch.AddRange(temp);
                    }

                    if (date != null && date != "")
                    {
                        temp.Clear();
                        Regex regex = new Regex(date, RegexOptions.IgnoreCase);
                        foreach (var x in resultSearch)
                        {
                            MatchCollection matches = regex.Matches(x.DateOfSale);
                            if (matches.Count > 0)
                            {
                                temp.Add(new SoldProduct(x.NameOfProduct, x.Category, x.Price, x.Number, x.DateOfSale, x.TimeOfSale, x.Login));
                            }
                        }
                        resultSearch.Clear();
                        resultSearch.AddRange(temp);
                    }


                    string pattern = @"[0-9]{1,5}\,[0-9]{1,2}";

                    if (startPrice != null && startPrice != "")
                    {
                        if (Regex.IsMatch(startPrice, pattern, RegexOptions.IgnoreCase))
                        {
                            temp.Clear();
                            foreach (var x in resultSearch)
                            {
                                string str = x.Price;
                                double currencyPrice = Convert.ToDouble(str);
                                if (currencyPrice >= Convert.ToDouble(startPrice))
                                {
                                    temp.Add(new SoldProduct(x.NameOfProduct, x.Category, x.Price, x.Number, x.DateOfSale, x.TimeOfSale, x.Login));
                                }
                            }
                            resultSearch.Clear();
                            resultSearch.AddRange(temp);
                        }
                        else
                        {
                            MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_price_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    if (finishPrice != null && finishPrice != "")
                    {
                        if (Regex.IsMatch(finishPrice, pattern, RegexOptions.IgnoreCase))
                        {
                            temp.Clear();
                            foreach (var x in resultSearch)
                            {
                                string str = x.Price;
                                double currencyPrice = Convert.ToDouble(str);
                                if (currencyPrice <= Convert.ToDouble(finishPrice))
                                {
                                    temp.Add(new SoldProduct(x.NameOfProduct, x.Category, x.Price, x.Number, x.DateOfSale, x.TimeOfSale, x.Login));
                                }
                            }
                            resultSearch.Clear();
                            resultSearch.AddRange(temp);
                        }
                        else
                        {
                            MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_price_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                }
                else
                {
                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Product_list_is_empty, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            return resultSearch;
        }

        public string Update(SoldProduct item)
        {
            string result = "Такого товара не сущетвует";
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.SoldProducts.Any(el => el.NameOfProduct == item.NameOfProduct);
                if (checkIsExist)
                {
                    string patternForPrice = @"[0-9]{1,5}\,[0-9]{1,2}";
                    string patternForNumber = @"[0-9]{1,3}";
                    string patternForDate = @"[0-9]{4}\-[0-9]{2}\-[0-9]{2}";
                    string patternForTime = @"[0-9]{2}\:[0-9]{2}";

                    SoldProduct NewProduct = db.SoldProducts.FirstOrDefault(el => el.NameOfProduct == item.NameOfProduct);

                    if (item.Category != null && item.Category != "")
                    {
                        NewProduct.Category = item.Category;
                    }

                    if (item.Price != null && item.Price != "")
                    {
                        if (Regex.IsMatch(item.Price, patternForPrice, RegexOptions.IgnoreCase))
                        {
                            NewProduct.Price = item.Price;
                        }
                        else
                        {
                            MessageBox.Show("Неверный формат цены. Правильный формат: 12356,67");
                        }
                    }

                    if (item.Number != null && item.Number != "")
                    {
                        if (Regex.IsMatch(item.Number, patternForNumber, RegexOptions.IgnoreCase))
                        {
                            NewProduct.Number = item.Number;
                        }
                        else
                        {
                            MessageBox.Show("Неверный формат количества. Правильный формат: 777");
                        }
                    }

                    if (item.DateOfSale != null && item.DateOfSale != "")
                    {
                        if (Regex.IsMatch(item.DateOfSale, patternForDate, RegexOptions.IgnoreCase))
                        {
                            NewProduct.DateOfSale = item.DateOfSale;
                        }
                        else
                        {
                            MessageBox.Show("Неверный формат даты. Правильный формат: 2021-05-20");
                        }
                    }

                    if (item.TimeOfSale != null && item.TimeOfSale != "")
                    {
                        if (Regex.IsMatch(item.TimeOfSale, patternForTime, RegexOptions.IgnoreCase))
                        {
                            NewProduct.TimeOfSale = item.TimeOfSale;
                        }
                        else
                        {
                            MessageBox.Show("Неверный формат времени. Правильный формат: 15:40");
                        }
                    }


                    db.SaveChanges();
                    result = "Товар изменён";
                }
            }
            return result;
        }
    }
}
