using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using kursach.Models.Data;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using kursach.Models.ProductBuilder;
using System.Text.RegularExpressions;
using kursach.Models.Control.Repository;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace kursach.Models.Control
{
    public class ProductControl : IRepository<Product>
    {
        private ApplicationContext db;
        public ProductControl(ApplicationContext context)
        {
            this.db = context;
        }

        public List<Product> GetAll()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<Product> result = db.Products.ToList();
                return result;
            }
        }


        public string Create(Product item)
        {
            string result = Views.Windows.Resources.Languages.Lang.m_Already_exists;
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Products.Any(el => el.NameOfProduct == item.NameOfProduct);
                if (!checkIsExist)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            Product tempObjectForValidation;
                            if (item.Description != null || item.Description != "")
                            {
                                tempObjectForValidation = new ProductWithDescription().CreateProductWithDescription(item.NameOfProduct, item.Picture, item.Category, item.Rating, item.Price, item.Number, item.Size, item.Description);
                            }
                            else
                            {
                                tempObjectForValidation = new ProductWithoutDescription().CreateProductWithoutDescription(item.NameOfProduct, item.Picture, item.Category, item.Rating, item.Price, item.Number, item.Size);
                            }

                            string patternForRating = @"[0-9]{1}\,[0-9]{1}";
                            string patternForPrice = @"[0-9]{1,5}\,[0-9]{1,2}";
                            string patternForNumber = @"[0-9]{1,3}";
                            string patternForSize = @"[0-9]{1,3}[x\|х][0-9]{1,3}";


                            //if (item.NameOfProduct == null || item.NameOfProduct == "")
                            //{
                            //    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_product_name, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                            //    return null;
                            //}


                            if (item.Category == null || item.Category == "")
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Category_missing, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }


                            if (item.Rating != null && item.Rating != "")
                            {
                                if (!Regex.IsMatch(item.Rating, patternForRating, RegexOptions.IgnoreCase))
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_rating_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                    return null;
                                }
                            }
                            else
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_rating, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }


                            if (item.Price != null && item.Price != "")
                            {
                                if (!Regex.IsMatch(item.Price, patternForPrice, RegexOptions.IgnoreCase))
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_price_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                    return null;
                                }
                            }
                            else
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_price, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
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


                            if (item.Size != null && item.Size != "")
                            {
                                if (!Regex.IsMatch(item.Size, patternForSize, RegexOptions.IgnoreCase))
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_size_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                    return null;
                                }
                            }
                            else
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_size, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }


                            //var results = new List<ValidationResult>();
                            //var context = new ValidationContext(tempObjectForValidation);
                            //if (!Validator.TryValidateObject(tempObjectForValidation, context, results, true))
                            //{
                            //    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Values_entered_incorrectly, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                            //    return null;
                            //}

                            db.Products.Add(tempObjectForValidation);
                            db.SaveChanges();
                            transaction.Commit();
                            result = Views.Windows.Resources.Languages.Lang.m_Product_added;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
            return result;
        }


        public string Delete(string nameOfProduct)
        {
            string result = Views.Windows.Resources.Languages.Lang.m_This_product_does_not_exist;
            using(ApplicationContext db = new ApplicationContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool checkIsExist = db.Products.Any(el => el.NameOfProduct == nameOfProduct);
                        if (checkIsExist)
                        {
                            Product deletingProduct = new Product();
                            var search = db.Products.Where(el => el.NameOfProduct == nameOfProduct);
                            foreach (Product p in search)
                            {
                                deletingProduct = p;
                                break;
                            }
                            db.Products.Remove(deletingProduct);
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

        #region ChangeAllFieldsViaClassAnnotations
        //public static string Update1(Product item)
        //{
        //    string result = "Такого товара не сущетвует";
        //    using(ApplicationContext db = new ApplicationContext())
        //    {
        //        bool checkIsExist = db.Products.Any(el => el.NameOfProduct == item.NameOfProduct);
        //        if (checkIsExist)
        //        {
        //            Product tempObjectForValidation = new Product(item.NameOfProduct, item.Picture, item.Category, item.Rating, item.Price, item.Number, item.Size, item.Description);
        //            var results = new List<ValidationResult>();
        //            var context = new ValidationContext(tempObjectForValidation);
        //            if (!Validator.TryValidateObject(tempObjectForValidation, context, results, true))
        //            {
        //                foreach (var error in results)
        //                {
        //                    string strWithError = error.ErrorMessage;
        //                    MessageBox.Show(strWithError);
        //                }
        //                return null;
        //            }

        //            Product NewProduct = db.Products.FirstOrDefault(el => el.NameOfProduct == item.NameOfProduct);
        //            NewProduct.Category = tempObjectForValidation.Category;
        //            NewProduct.Rating = tempObjectForValidation.Rating;
        //            NewProduct.Price = tempObjectForValidation.Price;
        //            NewProduct.Number = tempObjectForValidation.Number;
        //            NewProduct.Size = tempObjectForValidation.Size;
        //            NewProduct.Description = tempObjectForValidation.Description;
        //            db.SaveChanges();
        //            result = "Товар изменён";
        //        }
        //    }
        //    return result;
        //}
        #endregion
        
        public string Update(Product item)
        {
            string result = Views.Windows.Resources.Languages.Lang.m_This_product_does_not_exist;
            using (ApplicationContext db = new ApplicationContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool checkIsExist = db.Products.Any(el => el.NameOfProduct == item.NameOfProduct);
                        if (checkIsExist)
                        {
                            string patternForRating = @"[0-9]{1}\,[0-9]{1}";
                            string patternForPrice = @"[0-9]{1,5}\,[0-9]{1,2}";
                            string patternForNumber = @"[0-9]{1,3}";
                            string patternForSize = @"[0-9]{1,3}[x\|х][0-9]{1,3}";

                            Product NewProduct = db.Products.FirstOrDefault(el => el.NameOfProduct == item.NameOfProduct);

                            Product tempObjectForValidation = new Product();

                            tempObjectForValidation.NameOfProduct = NewProduct.NameOfProduct;
                            if (item.Category != null && item.Category != "")
                            {
                                tempObjectForValidation.Category = item.Category;
                            }
                            else
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Category_missing, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }

                            if (item.Rating != null && item.Rating != "")
                            {
                                if (Regex.IsMatch(item.Rating, patternForRating, RegexOptions.IgnoreCase))
                                {
                                    tempObjectForValidation.Rating = item.Rating;
                                }
                                else
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_rating_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_rating, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }

                            if (item.Price != null && item.Price != "")
                            {
                                if (Regex.IsMatch(item.Price, patternForPrice, RegexOptions.IgnoreCase))
                                {
                                    tempObjectForValidation.Price = item.Price;
                                }
                                else
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_price_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_price, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }

                            if (item.Number != null && item.Number != "")
                            {
                                if (Regex.IsMatch(item.Number, patternForNumber, RegexOptions.IgnoreCase))
                                {
                                    tempObjectForValidation.Number = item.Number;
                                }
                                else
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_quantity_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_number, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }

                            if (item.Size != null && item.Size != "")
                            {
                                if (Regex.IsMatch(item.Size, patternForSize, RegexOptions.IgnoreCase))
                                {
                                    tempObjectForValidation.Size = item.Size;
                                }
                                else
                                {
                                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Invalid_size_format, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Missing_size, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }

                            if (item.Description != null && item.Description != "")
                            {
                                tempObjectForValidation.Description = item.Description;
                            }


                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(tempObjectForValidation);
                            if (!Validator.TryValidateObject(tempObjectForValidation, context, results, true))
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Values_entered_incorrectly, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }


                            NewProduct.Category = tempObjectForValidation.Category;
                            NewProduct.Rating = tempObjectForValidation.Rating;
                            NewProduct.Price = tempObjectForValidation.Price;
                            NewProduct.Number = tempObjectForValidation.Number;
                            NewProduct.Size = tempObjectForValidation.Size;
                            NewProduct.Description = tempObjectForValidation.Description;

                            db.SaveChanges();
                            transaction.Commit();
                            result = Views.Windows.Resources.Languages.Lang.m_Product_changed;
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

        public static List<Product> SortProduct(bool isCheckNOP, bool isCheckRating)
        {
            List<Product> resultSort = new List<Product>();
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Products.Any();
                if(checkIsExist)
                {
                    if (isCheckNOP == true && isCheckRating == false)
                    {
                        var sort = db.Products.OrderBy(el => el.NameOfProduct);
                        resultSort.AddRange(sort);
                    }
                    else if (isCheckNOP == false && isCheckRating == true)
                    {
                        var sort = db.Products.OrderBy(el => el.Rating);
                        resultSort.AddRange(sort);
                    }
                    else if(isCheckNOP == true && isCheckRating == true)
                    {
                        var sort = from el in db.Products
                                   orderby el.NameOfProduct, el.Rating
                                   select el;
                        resultSort.AddRange(sort);
                    }
                }
                else
                {
                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Product_list_is_empty, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            return resultSort;
        }

        public static List<Product> SearchProduct(string nameOfProduct, string category, string number, string size, string startPrice, string finishPrice)
        {
            List<Product> resultSearch = new List<Product>();
            using(ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Products.Any();
                if(checkIsExist)
                {
                    var allProducts = db.Products;
                    resultSearch.AddRange(allProducts);
                    List<Product> temp = new List<Product>();

                    if(nameOfProduct != null && nameOfProduct != "")
                    {
                        temp.Clear();
                        Regex regex = new Regex(nameOfProduct, RegexOptions.IgnoreCase);
                        foreach (var x in resultSearch)
                        {
                            MatchCollection matches = regex.Matches(x.NameOfProduct);
                            if (matches.Count > 0)
                            {
                                temp.Add(new Product(x.NameOfProduct, x.Picture, x.Category, x.Rating, x.Price, x.Number, x.Size, x.Description));
                            }
                        }
                        resultSearch.Clear();
                        resultSearch.AddRange(temp);
                    }

                    if (category != null && category != "")
                    {
                        temp.Clear();
                        Regex regex = new Regex(category, RegexOptions.IgnoreCase);
                        foreach (var x in resultSearch)
                        {
                            MatchCollection matches = regex.Matches(x.Category);
                            if (matches.Count > 0)
                            {
                                temp.Add(new Product(x.NameOfProduct, x.Picture, x.Category, x.Rating, x.Price, x.Number, x.Size, x.Description));
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
                                temp.Add(new Product(x.NameOfProduct, x.Picture, x.Category, x.Rating, x.Price, x.Number, x.Size, x.Description));
                            }
                        }
                        resultSearch.Clear();
                        resultSearch.AddRange(temp);
                    }

                    if (size != null && size != "")
                    {
                        temp.Clear();
                        Regex regex = new Regex(size, RegexOptions.IgnoreCase);
                        foreach (var x in resultSearch)
                        {
                            MatchCollection matches = regex.Matches(x.Size);
                            if (matches.Count > 0)
                            {
                                temp.Add(new Product(x.NameOfProduct, x.Picture, x.Category, x.Rating, x.Price, x.Number, x.Size, x.Description));
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
                                    temp.Add(new Product(x.NameOfProduct, x.Picture, x.Category, x.Rating, x.Price, x.Number, x.Size, x.Description));
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
                                    temp.Add(new Product(x.NameOfProduct, x.Picture, x.Category, x.Rating, x.Price, x.Number, x.Size, x.Description));
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
    }
}
