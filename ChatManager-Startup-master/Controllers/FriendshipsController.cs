using ChatManager.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Web.Mvc;

namespace ChatManager.Controllers
{
    public class FriendshipsController : Controller
    {
        private string Recherche
        {
            get { 
                if (Session["SearchText"] == null)
                {
                    return "";
                }
                return (string)Session["SearchText"];
                }
            set { Session["SearchText"] = value; }
        }
        #region Filters
        private bool NotFriend
        {
            get
            {
                if(Session["SetFilterNotFriend"] == null)
                {
                    Session["SetFilterNotFriend"] = true;
                }
                
                return (bool)Session["SetFilterNotFriend"]; 
            }
            set
            {
                Session["SetFilterNotFriend"] = value;
            }
        }
        private bool Pending
        {
            get
            {
                if (Session["SetFilterPending"] == null)
                {
                    Session["SetFilterPending"] = true;
                }

                return (bool)Session["SetFilterPending"];
            }
            set
            {
                Session["SetFilterPending"] = value;
            }
        }
        private bool Requested
        {
            get
            {
                if (Session["SetFilterRequest"] == null)
                {
                    Session["SetFilterRequest"] = true;
                }

                return (bool)Session["SetFilterRequest"];
            }
            set
            {
                Session["SetFilterRequest"] = value;
            }
        }
        private bool Blocked
        {
            get
            {
                if (Session["SetFilterBlocked"] == null)
                {
                    Session["SetFilterBlocked"] = true;
                }

                return (bool)Session["SetFilterBlocked"];
            }
            set
            {
                Session["SetFilterBlocked"] = value;
            }
        }
        private bool Friend
        {
            get
            {
                if (Session["SetFilterFriend"] == null)
                {
                    Session["SetFilterFriend"] = true;
                }
                return (bool)Session["SetFilterFriend"];
            }
            set
            {
                Session["SetFilterFriend"] = value;
            }
        }
        private bool Refused
        {
            get
            {
                if (Session["SetFilterRefused"] == null)
                {
                    Session["SetFilterRefused"] = true;
                }
                return (bool)Session["SetFilterRefused"];
            }
            set
            {
                Session["SetFilterRefused"] = value;
            }
        }
        #endregion

        public ActionResult Index()
        {
            if (Session["SetFilterRefused"] == null)
            {
                Session["SetFilterRefused"] = true;
            }
            if (Session["SetFilterFriend"] == null)
            {
                Session["SetFilterFriend"] = true;
            }
            if (Session["SetFilterBlocked"] == null)
            {
                Session["SetFilterBlocked"] = true;
            }
            if (Session["SetFilterRequest"] == null)
            {
                Session["SetFilterRequest"] = true;
            }
            if (Session["SetFilterPending"] == null)
            {
                Session["SetFilterPending"] = true;
            }
            if (Session["SetFilterNotFriend"] == null)
            {
                Session["SetFilterNotFriend"] = true;
            }
            return View();
        }
        public ActionResult GetFriendsShipsStatus(bool forceRefresh = false)
        {
            if (forceRefresh || DB.FriendShips.HasChanged)
            {
                return PartialView(Users());
            }
            return null;
        }
        public ActionResult SendFriendshipRequest(int id)
        {
            FriendShip friendShip = new FriendShip();
            friendShip.IdAmis1= OnlineUsers.GetSessionUser().Id;
            friendShip.IdAmis2 = id;
            DB.FriendShips.Add(friendShip);
            return RedirectToAction("Index");
        }
        public ActionResult AccepterFriend(int Id)
        {
            FriendShip Friends  = DB.FriendShips.ToList().Where(u=>u.Id == Id).FirstOrDefault();
            Friends.TypeAmitie = type.Amis;
            DB.FriendShips.Update(Friends);
            return RedirectToAction("Index");
        }
        public ActionResult AnnulerFriend (int Id)
        {
            
            DB.FriendShips.Delete(Id);
            return RedirectToAction("Index");
        }
        public ActionResult DeclinedFriend(int Id)
        {
            FriendShip Friends = DB.FriendShips.ToList().Where(u => u.Id == Id).FirstOrDefault();
            Friends.TypeAmitie = type.DeclinedAsked;
            DB.FriendShips.Update(Friends);
            return RedirectToAction("Index");
        }
        public ActionResult DeclinedAcceptFriend(int Id)
        {
            FriendShip Friends = DB.FriendShips.ToList().Where(u => u.Id == Id).FirstOrDefault();
            Friends.TypeAmitie = type.RequeteAsked;
            DB.FriendShips.Update(Friends);
            return RedirectToAction("Index");
        }
        public ActionResult AccepterFriend2(int Id)
        {
            FriendShip Friends = DB.FriendShips.ToList().Where(u => u.Id == Id).FirstOrDefault();
            Friends.TypeAmitie = type.Amis;
            DB.FriendShips.Update(Friends);
            return RedirectToAction("Index");
        }
        public ActionResult Search(string text)
        {
            Recherche = text;
            return null;
        }
        #region CheckBox
        public ActionResult SetFilterNotFriend(bool check)
        {
            if (check)
            {
                NotFriend = true;
            }
            else
            {
                NotFriend = false;
            }
            return null;
        }
        public ActionResult SetFilterRequest(bool check)
        {
            if (check)
            {
                Requested = true;
            }
            else
            {
                Requested = false;
            }
            return null;
        }
        public ActionResult SetFilterPending(bool check)
        {
            if (check)
            {
                Pending = true;
            }
            else
            {
                Pending = false;
            }
            return null;
        }
        public ActionResult SetFilterFriend(bool check)
        {
            if (check)
            {
                Friend = true;
            }
            else
            {
                Friend = false;
            }
            return null;
        }
        public ActionResult SetFilterRefused(bool check)
        {
            if (check)
            {
                Refused = true;
            }
            else
            {
                Refused = false;
            }
            return null;
        }
        public ActionResult SetFilterBlocked(bool check)
        {
            if (check)
            {
                Blocked = true;
            }
            else
            {
                Blocked = false;
            }
            return null;
        }
        #endregion

        private List<User> Users()
        { 
            List<User> users = new List<User>();
            foreach (User user in DB.Users.ToList())
            {
                if(Pending)
                {
                    if (DB.FriendShips.ToList().Where(u => u.IdAmis2 == user.Id && u.IdAmis1 == OnlineUsers.GetSessionUser().Id && u.TypeAmitie == type.Requete).FirstOrDefault() != null)
                    {
                        users.Add(user);
                    }
                }
                if (Requested)
                {
                    if (DB.FriendShips.ToList().Where(u => u.IdAmis1 == user.Id && u.IdAmis2 == OnlineUsers.GetSessionUser().Id && u.TypeAmitie == type.Requete).FirstOrDefault() != null)
                    {
                        users.Add(user);
                    }
                }
                if(NotFriend)
                {
                    if (DB.FriendShips.ToList().Where(u => u.IdAmis1 == user.Id && u.IdAmis2 == OnlineUsers.GetSessionUser().Id|| u.IdAmis2 == user.Id && u.IdAmis1 == OnlineUsers.GetSessionUser().Id).FirstOrDefault() == null && !user.Blocked && !OnlineUsers.GetSessionUser().Blocked)
                    {
                        users.Add(user);
                    }
                }
                if (Friend)
                {
                    if (DB.FriendShips.ToList().Where(u => (u.IdAmis1 == user.Id && u.IdAmis2 == OnlineUsers.GetSessionUser().Id || u.IdAmis2 == user.Id && u.IdAmis1 == OnlineUsers.GetSessionUser().Id) && u.TypeAmitie == type.Amis).FirstOrDefault() != null)
                    {
                        users.Add(user);
                    }
                }
               
                if(Refused)
                {
                    if (DB.FriendShips.ToList().Where(u => u.IdAmis2 == user.Id && u.IdAmis1 == OnlineUsers.GetSessionUser().Id && u.TypeAmitie == type.RequeteAsked).FirstOrDefault() != null)
                    {
                        users.Add(user);
                    }

                }
            }
            if (Blocked)
            {
                List<User> usersblocekd = DB.Users.ToList().Where(u => u.Blocked == true).ToList();
                users.AddRange(usersblocekd);
            }
            if (Recherche != "")
            {
                users = users.ToList().Where(u => u.Id != OnlineUsers.GetSessionUser().Id && u.LastName.ToLower().Contains(Recherche.ToLower()) || u.FirstName.ToLower().Contains(Recherche.ToLower())).OrderBy(u => u.LastName).ToList();
            }
            return users.Where(u => u.Id != OnlineUsers.GetSessionUser().Id).OrderBy(u => u.GetFullName()).ToList();
        }
    }
   
}