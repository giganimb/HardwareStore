using kursach.Models.Control.Repository;
using kursach.Models.Control.UnitOfWork;
using kursach.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursach.Models.Control
{
    public class UserControl : IRepository<User>
    {
        private ApplicationContext db;
        public UserControl(ApplicationContext context)
        {
            this.db = context;
        }

        public List<User> GetAll()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                List<User> result = db.Users.ToList();
                return result;
            }
        }

        public string Create(User item)
        {
            string result = Views.Windows.Resources.Languages.Lang.m_Already_exists;
            using (ApplicationContext db = new ApplicationContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool checkIsExist = db.Users.Any(el => el.Login == item.Login);
                        if (!checkIsExist)
                        {
                            if (item.Login == null || item.Login == "")
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Login_missing, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }


                            if (item.Password == null || item.Password == "")
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Password_missing, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }


                            User tempObjectForValidation = new User(item.Login, item.Password, item.IsAdmin);
                            var results = new List<ValidationResult>();
                            var context = new ValidationContext(tempObjectForValidation);
                            if (!Validator.TryValidateObject(tempObjectForValidation, context, results, true))
                            {
                                MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Values_entered_incorrectly, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                                return null;
                            }

                            string encryptedPassword = UserControl.Encrypt(tempObjectForValidation.Password);
                            tempObjectForValidation.Password = encryptedPassword;

                            db.Users.Add(tempObjectForValidation);
                            db.SaveChanges();
                            transaction.Commit();
                            result = Views.Windows.Resources.Languages.Lang.m_User_added;
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

        public static string ComeIn(string login, string password)
        {
            string result = Views.Windows.Resources.Languages.Lang.m_This_user_does_not_exist;
            using(ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Users.Any(el => el.Login == login);
                if(checkIsExist)
                {
                    User entriedUser = db.Users.FirstOrDefault(el => el.Login == login);
                    string dencryptedPassword = UserControl.Dencrypt(entriedUser.Password);
                    if (password == dencryptedPassword)
                    {
                        return null;
                    }
                    else
                    {
                        result = Views.Windows.Resources.Languages.Lang.m_Password_entered_incorrectly;
                    }
                }
            }
            return result;
        }

        public static string Encrypt(string password)
        {
            string hash = "Password@2021$";
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripleDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripleDES.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
        }


        public static string Dencrypt(string password)
        {
            string hash = "Password@2021$";
            byte[] data = Convert.FromBase64String(password);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripleDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripleDES.CreateDecryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result); 
        }


        public static string IsAdmin(string login)
        {
            string result = "NO";
            using (ApplicationContext db = new ApplicationContext())
            {
                User entriedUser = db.Users.FirstOrDefault(el => el.Login == login);
                if(entriedUser.IsAdmin == "YES")
                {
                    result = "YES";
                }
            }
            return result;
        }


        public static bool IsFpassEqualsSpass(string firstPassword, string secondPassword)
        {
            if (firstPassword == secondPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public string Delete(string login)
        {
            string result = "Такого пользователя не существует";
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Users.Any(el => el.Login == login);
                if (checkIsExist)
                {
                    User deletingUser = new User();
                    var search = db.Users.Where(el => el.Login == login);
                    foreach (User p in search)
                    {
                        deletingUser = p;
                        break;
                    }
                    db.Users.Remove(deletingUser);
                    db.SaveChanges();
                    result = "Пользователь удвлён";
                }
            }
            return result;
        }

        public string Update(User item)
        {
            string result = "Такого пользователя не сущетвует";
            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Users.Any(el => el.Login == item.Login);
                if (checkIsExist)
                {
                    User NewUser = db.Users.FirstOrDefault(el => el.Login == item.Login);

                    if (item.Login != null && item.Login != "")
                    {
                        NewUser.Login = item.Login;
                    }

                    if (item.Password != null && item.Password != "")
                    {
                        NewUser.Password = item.Password;
                    }

                    db.SaveChanges();
                    result = "Пользователь изменён";
                }
            }
            return result;
        }
    }
}
