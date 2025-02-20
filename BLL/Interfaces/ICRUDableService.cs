namespace BLL.Interfaces
{
    public interface ICRUDableService<ItemDTO>
    {
        public void Create(ItemDTO itemDto);
        public void Update(ItemDTO itemDto);
        public ItemDTO Delete(int id);
        public IEnumerable<ItemDTO> GetAll();
        public ItemDTO Retrieve(int id);
    }
}
