using ProcureHub_ASP.NET_Core.Models;

namespace ProcureHub_ASP.NET_Core.Services.Interfaces
{
    public interface IChatService
    {
        public Task<List<Message>> GetMessages(int eventId, int userId);
        public Task<Message> SendMessage(Message message);
    }
}
