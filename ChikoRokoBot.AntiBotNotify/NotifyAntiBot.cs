using System.Threading.Tasks;
using ChikoRokoBot.AntiBotNotify.Extensions;
using ChikoRokoBot.AntiBotNotify.Interfaces;
using ChikoRokoBot.AntiBotNotify.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChikoRokoBot.AntiBotNotify
{
    public class NotifyAntiBot
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IAntiBotPictureManager _antiBotPictureManager;

        public NotifyAntiBot(
            ITelegramBotClient telegramBotClient,
            IAntiBotPictureManager antiBotPictureManager)
        {
            _telegramBotClient = telegramBotClient;
            _antiBotPictureManager = antiBotPictureManager;
        }

        [FunctionName("NotifyAntiBot")]
        public async Task Run([QueueTrigger("notifyantibot", Connection = "AzureWebJobsStorage")]Notification myQueueItem, ILogger log)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "Home Page",
                    url: myQueueItem.SiteState.Url)
            });

            var picturesUrl = await _antiBotPictureManager.GetPicturesUrl();

            await _telegramBotClient.SendPhotoAsync(
                chatId: myQueueItem.User.ChatId,
                messageThreadId: myQueueItem.User.TopicId,
                replyMarkup: inlineKeyboard,
                caption: "Hey there\\! I am detecting AntiBot protection\\. Go to home page and check if there is something interesting awaiting\\!",
                photo: InputFile.FromString(picturesUrl.GetRandomItem()),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2
            );
        }
    }
}

