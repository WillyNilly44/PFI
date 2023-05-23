using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class Chat
    {
        public Chat()
        {
            DateTime = DateTime.Now;
            EtatMessage = EtatMessage.Envoye;
            this.Message = TempMessage; 
        }

        public int Id { get; set; } 
        public int IdSend { get; set; }
        public int IdReceive { get; set; }
        public string Message { get; set; }

        static public string TempMessage { get; set; }
        static public int IdSelected { get; set; }
        
        public DateTime DateTime { get; set; }

        public EtatMessage EtatMessage { get; set; }

        
    }

    public enum EtatMessage { Recu, Envoye, Modifié, Retiré }
}