using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class FriendShip
    {
        public FriendShip()
        {
           
            DateTime = DateTime.Now;
            TypeAmitie = type.Requete;


        }
        public int Id { get; set; }
        public int IdAmis1 { get; set; }
        public int IdAmis2 { get; set; }
        public DateTime DateTime { get; set; }
        public type TypeAmitie { get; set; }
        
        
    }
    public enum type {Requete, Amis, DeclinedAsked, RequeteAsked }

}