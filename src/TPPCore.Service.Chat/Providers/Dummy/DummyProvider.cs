using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPPCore.Service.Chat.DataModels;
using TPPCore.Service.Common;

namespace TPPCore.Service.Chat.Providers.Dummy
{
    public class DummyProvider : IProviderAsync
    {
        public string Name { get; } = "dummy";

        private bool running = true;
        private ProviderContext context;
        private int chatNoiseCounter = 2;

        public void Configure(ProviderContext providerContext)
        {
            this.context = providerContext;
        }

        public async Task Run()
        {
            while (running)
            {
                foreach (var i in Enumerable.Range(0, chatNoiseCounter))
                {
                    var chatMessage = newFakeReceivedMessage();
                    context.PublishChatEvent(chatMessage);
                }

                chatNoiseCounter *= 2;
                await Task.Delay(1000);
            }
        }

        public void Shutdown()
        {
            running = false;
        }

        public string GetUserId()
        {
            return "dummy";
        }

        public string GetUsername()
        {
            return "dummy";
        }

        public async Task SendMessage(string channel, string message)
        {
            await Task.Delay(100);
            var chatMessage = new ChatMessage() {
                ProviderName = Name,
                TextContent = message,
                Channel = channel,
                IsSelf = true
            };
            context.PublishChatEvent(chatMessage);
        }

        public async Task SendPrivateMessage(string user, string message)
        {
            await Task.Delay(100);
            var chatMessage = new ChatMessage() {
                ProviderName = Name,
                TextContent = message,
                Channel = user,
                IsSelf = true
            };
            context.PublishChatEvent(chatMessage);
        }

        public async Task<IList<ChatUser>> GetRoomList(string channel)
        {
            await Task.Delay(100);
            var user1 = new ChatUser() {
                UserId = "dummy",
                Username = "dummy",
                Nickname = "Dummy",
            };
            var user2 = new ChatUser() {
                UserId = "someone",
                Username = "someone",
                Nickname = "Someone",
            };
            return new List<ChatUser>() { user1, user2 };
        }

        private ChatMessage newFakeReceivedMessage() {
            var random = new Random();
            var userId = random.Next(0, 1000000).ToString();

            var user = new ChatUser() {
                UserId = userId,
                Username = $"someone{userId}",
                Nickname = $"Someone{userId}",
            };
            var chatMessage = new ChatMessage() {
                ProviderName = Name,
                Sender = user,
                TextContent = "hello " + random.Next(0, 10000)
            };

            return chatMessage;
        }
    }
}
