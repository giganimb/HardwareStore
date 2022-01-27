using kursach.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace kursach.Models.Control.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationContext db = new ApplicationContext();
        private ProductControl productControl;
        private SoldProductControl soldProductControl;
        private UserControl userControl;

        public ProductControl Products
        {
            get
            {
                if (productControl == null)
                    productControl = new ProductControl(db);
                return productControl;
            }
        }

        public SoldProductControl SoldProducts
        {
            get
            {
                if (soldProductControl == null)
                    soldProductControl = new SoldProductControl(db);
                return soldProductControl;
            }
        }

        public UserControl Users
        {
            get
            {
                if (userControl == null)
                    userControl = new UserControl(db);
                return userControl;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
