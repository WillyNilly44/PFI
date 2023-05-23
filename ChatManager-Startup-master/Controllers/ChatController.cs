using ChatManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class ChatController : Controller
    {
        private int IdSelected
        {
            get
            {
                if (Session["IdSelected"] == null)
                {
                    Session["IdSelected"] = 0;
                }
                return (int)Session["IdSelected"];
            }
            set
            {
                Session["IdSelected"] = value;
            }
        }

        private List<Chat> Chats(int userId)
        {
            return DB.Chat.ToList().OrderBy(c => c.DateTime).Where(c => c.IdSend == OnlineUsers.GetSessionUser().Id || c.IdReceive == OnlineUsers.GetSessionUser().Id && c.IdReceive == userId || c.IdSend == userId).ToList();
        }
        // GET: Chat
        [OnlineUsers.UserAccess]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetFriendsShipsList(bool forceRefresh = false)
        {
            if (forceRefresh || DB.FriendShips.HasChanged)
            {
                List<FriendShip> friendships = DB.FriendShips.ToList().OrderBy(c => c.Id).Where(m => m.TypeAmitie == type.Amis && m.IdAmis1 == OnlineUsers.GetSessionUser().Id || m.IdAmis2 == OnlineUsers.GetSessionUser().Id).ToList();
                List<User> users = new List<User>();
                foreach (var friendship in friendships)
                {
                    if (friendship.IdAmis1 == OnlineUsers.GetSessionUser().Id)
                        users.Add(DB.Users.Get(friendship.IdAmis2));
                    if (friendship.IdAmis2 == OnlineUsers.GetSessionUser().Id)
                        users.Add(DB.Users.Get(friendship.IdAmis1));
                }
                return PartialView(users);
            }
            return null;
        }


        public ActionResult GetMessages(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Chat.HasChanged)
            {
                ViewBag.IdSelected = IdSelected;
                return PartialView(Chats(IdSelected));
            }

            return null;
        }


        public ActionResult SetCurrentTarget(int id)
        {
            IdSelected = id; // Le mettre statiquee permet de juste pas devoir le mettre dans le constructeur permet de juste le Set
            return null; //
        }

        public ActionResult Send(string message)
        {
            if (IdSelected != 0)
            {
                Chat.IdSelected = OnlineUsers.GetSessionUser().Id;
                Chat.TempMessage = message;
                Chat chat = new Chat();
                chat.IdSend = OnlineUsers.GetSessionUser().Id;
                chat.IdReceive = IdSelected;
                DB.Chat.Add(chat); OnlineUsers.AddNotification((int)Session["IdSelected"], $"Bonjour, vous avez reÃ§u un nouveau message de {OnlineUsers.GetSessionUser().Id}");
            }
            return null;
        }


        public ActionResult IsTyping()
        {
            return null;
        }

        public ActionResult StopTyping()
        {
            return null;
        }


        public ActionResult Delete(int id)
        {
            DB.Chat.Delete(id);
            return null;
        }

        public ActionResult Update(int id, string message)
        {
            Chat chat = DB.Chat.GetChat(id);
            chat.Message = message;
            DB.Chat.Update(chat);
            return null;
        }

        public ActionResult IsTargetTyping()
        {
            return null;
        }

        public ActionResult Conversations()
        {
            return View();
        }

        private List<Chat> Logs()
        {
            return DB.Chat.ToList().OrderBy(c => c.DateTime).ToList();
        }

        public ActionResult GetChatLogs(bool forceRefresh = false)
        {
            if (forceRefresh || DB.Chat.HasChanged)
            {
                return PartialView(Logs());
            }

            return null;
        }

        public ActionResult SupprimerChatLog(int Id)
        {

            DB.Chat.Delete(Id);
            return RedirectToAction("Conversations");
        }
    }
}