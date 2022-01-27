using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace kursach.Models.Control.Repository
{
    interface IRepository<T> where T : class
    {
        List<T> GetAll();
        string Create(T item);
        string Update(T item);
        string Delete(string nameOfProduct);
    }
}
