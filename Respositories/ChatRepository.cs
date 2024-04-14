using Neo4j.Driver;
using ProcureHub_ASP.NET_Core.Helper;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using System.Text.Json;

namespace ProcureHub_ASP.NET_Core.Respositories
{
    public class ChatRepository : IChatRepository
    {
        IConnectionDriver _connectionDriver;
        IServiceProvider _serviceProvider;

        public ChatRepository(IConnectionDriver driver, IServiceProvider serviceProvider) 
        {
            _connectionDriver = driver;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<Message>> GetAllMessages(int eventId, int userId)
        {
            var userContext = _serviceProvider.GetService<IUserContext>();
            var id = userContext.GetUserId();

            IDriver connection = _connectionDriver.GetConnection();
            var session = connection.AsyncSession();

            return await session.ExecuteWriteAsync(async tx =>
            {
                List<Message> sentMessages = new List<Message>();
                var senderQuery = "MATCH (s1:user)-[re1:SENT]->(m1:Message)-[re2:SENT_TO]->(r1:user) where ID(s1)=$senderId and ID(r1)=$receiverId MATCH (m1)-[r5:IS_MESSAGE_OF]->(e:Event) WHERE ID(e)=$eventId  RETURN COLLECT({date:m1.date, Text:m1.text, Id:ID(m1)}) as Sent";

                var sendReader = await tx.RunAsync(senderQuery, new
                {
                    senderId = id,
                    receiverId = userId, 
                    eventId = eventId,
                });

                while (await sendReader.FetchAsync())
                {
                    if (sendReader.Current["Sent"] != null)
                    {
                        var senderNode = sendReader.Current["Sent"];

                        var temp = JsonSerializer.Serialize(senderNode);
                        sentMessages = JsonSerializer.Deserialize<List<Message>>(temp);
                        sentMessages.ForEach(b => b.IsSent = true);
                    }
                }



                List<Message> receivedMessages = new List<Message>();
                var receiverQuery = "MATCH (s1:user)-[re1:SENT]->(m1:Message)-[re2:SENT_TO]->(r1:user) where ID(s1)=$senderId and ID(r1)=$receiverId MATCH (m1)-[r5:IS_MESSAGE_OF]->(e:Event) WHERE ID(e)=$eventId  RETURN COLLECT({date:m1.date, Text:m1.text, Id:ID(m1)}) as Received";
                var receivedReader = await tx.RunAsync(receiverQuery, new
                {
                    senderId = userId,
                    receiverId = id,
                    eventId = eventId,
                });

                while (await receivedReader.FetchAsync())
                {
                    if (receivedReader.Current["Received"] != null)
                    {
                        var senderNode = receivedReader.Current["Received"];

                        var temp = JsonSerializer.Serialize(senderNode);
                        receivedMessages = JsonSerializer.Deserialize<List<Message>>(temp);
                        receivedMessages.ForEach(b => b.IsSent = false);
                    }
                }

                List<Message> msgs = new List<Message>();
                msgs = msgs.Concat(receivedMessages).Concat(sentMessages).ToList();
                msgs = msgs.OrderBy(o=> DateTime.Parse(o.date)).ToList();
                return msgs;
            });
        }

        public async Task<Message> SendMessage(Message message)
        {

            IDriver connection = _connectionDriver.GetConnection();
            var session = connection.AsyncSession();

            var query = "MATCH (sender:user),(reciever:user),(event:Event) WHERE ID(sender)=$senderId and ID(reciever)=$recieverId and ID(event)=$eventId CREATE (sender)-[r:SENT]->(c:Message{text: $messageText ,date:$date})-[r1:SENT_TO]->(reciever) CREATE (c)-[r2:IS_MESSAGE_OF]->(event) return c";

            return await session.ExecuteWriteAsync(async tx =>
            {
                var reader = await tx.RunAsync(query, new
                {
                    senderId = message.SentBy,
                    recieverId = message.SentTo,
                    eventId = message.EventId,
                    messageText = message.Text,
                    date = DateTime.Now.ToString(),
                });

                Message newMessage = new Message();

                while(await reader.FetchAsync())
                {
                    var node = reader.Current["c"].As<INode>();
                    newMessage.Id = (int)node.Id;
                    newMessage.Text = node.Properties["text"].As<string>();
                    newMessage.date = node.Properties["date"].As<string>();
                }
                return newMessage;
            });
        }
    }
}
