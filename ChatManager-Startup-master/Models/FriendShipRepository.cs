using Antlr.Runtime.Misc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class FriendshipRepository : Repository<FriendShip>
    {
        //public override int Add(FriendShip friendship)
        //{
        //    BeginTransaction();
        //    base.Add(friendship);
        //    EndTransaction();
        //    return friendship.Id;
        //}
        public IOrderedEnumerable<User> Users;

        public override bool Update(FriendShip friendship)
        {
            BeginTransaction();
            var result = base.Update(friendship);
            EndTransaction();
            return result;
        }
        public override bool Delete(int Id)
        {
            FriendShip friendship = DB.FriendShips.Get(Id);
            if (friendship != null)
            {
                BeginTransaction();
                var result = base.Delete(Id);
                EndTransaction();
                return result;
            }
            return false;
        }
        public FriendShip GetFriendShipType(int id1, int id2)
        {
            FriendShip amis = DB.FriendShips.ToList().Where(u=> u.IdAmis1 == id1 && u.IdAmis2 == id2 || u.IdAmis1 == id2 && u.IdAmis2 == id1).FirstOrDefault();
            return amis;
        }
    }
}