using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRespository;

        public ChatService(IChatRepository chatRespository) 
        {
            _chatRespository = chatRespository;
        }

        public async Task<List<Message>> GetMessages(int eventId, int userId)
        {
            return await this._chatRespository.GetAllMessages(eventId, userId);
        }

        public async Task<Message> SendMessage(Message message)
        {
            return await this._chatRespository.SendMessage(message);
        }
    }
}
