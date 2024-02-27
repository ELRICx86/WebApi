using Backend.Models;

namespace Backend.Service
{
    public interface IService
    {
        Task<Message> GetById(int id);
        Task<Message> GetAll();
        Task<Message> Add(TodoList entity);
        Task<Message> Update(TodoList entity,int id);
        Task<Message> Delete(int id);
    }
}
