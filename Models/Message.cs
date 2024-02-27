namespace Backend.Models
{
    public class Message
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public TodoList TodoList { get; set; }
        public List<TodoList> lists { get; set; }
    }
}
