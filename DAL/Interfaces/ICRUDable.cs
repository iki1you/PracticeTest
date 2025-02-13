using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICRUDable<T> where T : class
    {
        public void Create(T trainee);
        public T? Retrieve(Guid id);
        public void Update(T trainee);
        public T? Delete(Guid id);
        public IEnumerable<T> GetAll();
    }
}
