using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace roBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        protected int contador = 1;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            // se o usuário digitar resetar o contador voltara ao 1
            if (activity.Text.Contains("resetar"))
            {
                contador = 1;
            }else
            {
                contador++;
            }

            // Return our reply to the user
            await context.PostAsync($" O contador está em { contador }, para resetar digite algo para eu saber.");


            context.Wait(MessageReceivedAsync);
        }
    }
}