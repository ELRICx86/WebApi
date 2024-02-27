using Backend.Models;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly IService _services;
        public TodoController(IService iservice)
        {
            _services = iservice;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _services.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _services.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Add(TodoList entity)
        {
            return Ok(await _services.Add(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(TodoList entity,int id)
        {
            return Ok(await _services.Update(entity,id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _services.Delete(id));
        }
    }
}
