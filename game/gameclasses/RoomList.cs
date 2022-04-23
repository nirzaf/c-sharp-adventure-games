using globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using game.data;

namespace game.gameclasses {

    [Serializable]
    public class RoomList : Dictionary<Rm, Room> {

        protected RoomList(SerializationInfo info, StreamingContext context)
       : base(info, context) {
            // constructor needed for serialization
        }
        
        public RoomList() { }

        public Room RoomAt(Rm id) {
            return this[id];
        }

        public string Describe() {
            string s = "";
            if (this.Count == 0) {
                s = "Nothing in RoomList.";
            } else {
                foreach (KeyValuePair<Rm, Room> kv in this) {
                    s = s + kv.Value.DescribeLocation() + "\r\n";
                }              
            }
            return s;
        }
    }
}
