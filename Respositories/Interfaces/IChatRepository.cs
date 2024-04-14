using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Respositories.Interfaces
{
    public interface IChatRepository
    {
        public Task<Message> SendMessage(Message message); 
        public Task<List<Message>> GetAllMessages(int eventId, int userId);
    }
}
