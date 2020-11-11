using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_Lab1_Cats.Models;
using Telegram.Bot;
using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Web_Lab1_Cats.Controllers
{
   

  
        public class BotTelegram
        {
            private readonly TelegramBotClient Bot =
                new TelegramBotClient("1400016317:AAFXkTKq7JMelsJ-cVcAr81dmxuRJa_4-Pk");
            private readonly CatsContext _context;
            public BotTelegram(CatsContext context)
            {
                _context = context;
                Bot.OnMessage += Bot_OnMessage;
                Bot.OnCallbackQuery += Bot_OnCallBack;

                Bot.StartReceiving();
            }
            private void Bot_OnCallBack(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
            {
                string operation = e.CallbackQuery.Data.ToString().Substring(0, 6);
                string value = e.CallbackQuery.Data.ToString().Substring(7);
                string text;
                Telegram.Bot.Types.InputFiles.InputOnlineFile photo;
                switch (operation)
                {
                    case "info_s":
                        var species = _context.Species.Where(s => s.Name == value).FirstOrDefault();
                        text = "Назва породи: " + species.Name + "\n" + "Середня тривалість життя: " + species.Lifetime.ToString() + "\n"
                                + "Розмір: " + species.Size.ToString() +  " \n" + "Шерсть: " + species.Wool + "\n" + "Країна: " + species.Country;
                        try
                        {
                            var imageFileS = System.IO.File.Open("C:/Users/Asus/source/repos/Cats/Web_Lab1_Cats/wwwroot/img/" + species.Photo, FileMode.Open);
                            photo = new Telegram.Bot.Types.InputFiles.InputOnlineFile(imageFileS);
                            Bot.SendPhotoAsync(e.CallbackQuery.From.Id, photo,
                                            caption: text).GetAwaiter().GetResult();
                            imageFileS.Close();
                        }
                        catch
                        {
                            Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, text);
                        }
                        break;
                    case "info_c":

                        var cats = _context.Cats.Where(a => a.Name == value).FirstOrDefault();
                        string info = cats.Info;//.Substring(0, 200);
                        text = "Ім'я котика: " + cats.Name + "\n" + "Вік: " + cats.Age.ToString() + "\n"+ "Порода: " + cats.Species.Name + "\n"
                                + "Опис: " + info;
                        try
                        {
                            // Console.WriteLine("/home/vasmax10/Документи/WebLab1ver2/WebLab1ver2/wwwroot" + actors.Photo);
                            var imageFileC = System.IO.File.Open("C:/Users/Asus/source/repos/Cats/Web_Lab1_Cats/wwwroot/img/" + cats.Photo, FileMode.Open);

                            photo = new Telegram.Bot.Types.InputFiles.InputOnlineFile(imageFileC);
                            Bot.SendPhotoAsync(e.CallbackQuery.From.Id, photo).GetAwaiter().GetResult();
                            Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, text);
                            imageFileC.Close();
                        }
                        catch
                        {
                            Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, text);
                        }
                        break;
                    default:
                        Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, e.CallbackQuery.Data + "\n" +
                        "У мене лапки. Я не розумію. Спробуй /info");
                        break;
                }

            }
        private static InlineKeyboardButton[][] GetInlineKeyboard(string type, string[] stringArray)
        {
            var keyboardInline = new InlineKeyboardButton[stringArray.Length][];
            var keyboardButtons = new InlineKeyboardButton[stringArray.Length];
            for (var i = 0; i < stringArray.Length; i++)
            {
                keyboardButtons[i] = new InlineKeyboardButton
                {
                    Text = stringArray[i],
                    CallbackData = type + " " + stringArray[i],
                };
                keyboardInline[i] = new InlineKeyboardButton[1] { keyboardButtons[i] };
            }
            return keyboardInline;
        }
        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                InlineKeyboardMarkup keyboardMarkup;
                string[] items;
                switch (e.Message.Text)
                {
                    case "/start":
                        var rkm = new ReplyKeyboardMarkup();

                        rkm.Keyboard =
                            new KeyboardButton[][]
                            {
                                new KeyboardButton[]
                                {
                                    new KeyboardButton("Породи"),
                                    new KeyboardButton("Котики")
                                }
                            };
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Привіт, " + e.Message.Chat.Username+ " )) \n Якщо ти тут, значить тебе також цікавлять котики.😺 \n Звичайних кішок не буває. (Колетт)  \n Бот допоможе обрати тобі свою незвичайну кішку!", default, false, false, 0, rkm);
                        break;
                    case "Породи":
                        items = _context.Species.Select(s => s.Name).ToArray();
                        keyboardMarkup = new InlineKeyboardMarkup(GetInlineKeyboard("info_s", items));
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Обери породу:", replyMarkup: keyboardMarkup);
                        break;
                    case "/species":
                        items = _context.Species.Select(s => s.Name).ToArray();
                        keyboardMarkup = new InlineKeyboardMarkup(GetInlineKeyboard("info_s", items));
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Обери породу:", replyMarkup: keyboardMarkup);
                        break;
                    case "Котики":
                        items = _context.Cats.Select(s => s.Name).ToArray();
                        keyboardMarkup = new InlineKeyboardMarkup(GetInlineKeyboard("info_c", items));
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Обери котика:", replyMarkup: keyboardMarkup);
                        break;
                    case "/cats":
                        items = _context.Cats.Select(s => s.Name).ToArray();
                        keyboardMarkup = new InlineKeyboardMarkup(GetInlineKeyboard("info_c", items));
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Обери котика:", replyMarkup: keyboardMarkup);
                        break;
                    case "/info":
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Поки що, ти можеш обрати лише котиків /cats чи породи /species ");
                        break;
                    default:
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, e.Message.Text + "\n" +
                        "У мене лапки. Я не розумію. Спробуй /info");
                        break;

                }
              
            }
            else if (e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            {
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "У мене лапки. Я не розумію. Спробуй /info");

            }
        }
    }
}
