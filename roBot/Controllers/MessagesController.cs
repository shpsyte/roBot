using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace roBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            ConnectorClient connect = new ConnectorClient(new Uri(activity.ServiceUrl));


            ///Pega o tipo da atividade
            string messageType = activity.GetActivityType();


            // se for do tipo Message, então a conversa é encaminhada para nosso dialog!
            if (messageType == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            // se for do tipo update então a conversa é NOVA e vamos mostar uma mensgem de Boas Vindas ao usuário
            else if (messageType == ActivityTypes.ConversationUpdate)
            {
                if (activity.MembersAdded != null && activity.MembersAdded.Any())
                {
                    foreach (var member in activity.MembersAdded)
                    {
                        if (member.Id != activity.Recipient.Id)
                        {
                            var reply = activity.CreateReply();
                            reply.Text = $"Olá, **Seja bem vindo**, sou o roBot!";

                            await connect.Conversations.ReplyToActivityAsync(reply);

                        };
                    }

                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            string messageType = message.GetActivityType();
            if (messageType == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (messageType == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (messageType == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (messageType == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            else if (messageType == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}


//ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

//else if (activity.Type == ActivityTypes.ConversationUpdate)
//            {
//                if (activity.MembersAdded != null && activity.MembersAdded.Any())
//                {
//                    foreach (var member in activity.MembersAdded)
//                    {
//                        if (member.Id != activity.Recipient.Id)
//                        {
//                            var reply = activity.CreateReply();
//reply.Text = $"asdfsadf";
//                            await connector.Conversations.ReplyToActivityAsync(reply);

//                        };
//                    }

//                }
//            }