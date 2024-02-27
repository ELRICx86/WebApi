using Backend.Models;

namespace Backend.Repository
{
    public interface IRepository
    {
            Task<Message> GetById(int id);
            Task<Message> GetAll();
            Task<Message> Add(TodoList entity);
            Task<Message> Update(TodoList entity, int id);
            Task<Message> Delete(int id);
    }
}



