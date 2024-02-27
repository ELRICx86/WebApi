using Backend.Models;
using Backend.Repository;
using System.Formats.Asn1;

namespace Backend.Service
{
    

    public class implService : IService
    {
        private readonly IRepository _repository;
        
        public implService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<Message> Add(TodoList entity)
        {
            return  await _repository.Add(entity);
        }

        public async Task<Message> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<Message> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Message> GetById(int id)
        {
           return await _repository.GetById(id);
        }

        public async Task<Message> Update(TodoList entity, int id)
        {
            return await _repository.Update(entity, id);
        }
    }
}
