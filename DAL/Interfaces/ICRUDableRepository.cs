using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICRUDableRepository<Item> where Item : class
    {
        public void Create(Item trainee);
        public Item? Retrieve(int id);
        public void Update(Item trainee);
        public Item? Delete(int id);
        public IEnumerable<Item> GetAll();
    }
}
