using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.gameclasses {
    
    [Serializable]
    public class ThingList : List<Thing> {
        // Thing is the basic game object and thsi defines a List structure for
        // Thing and descendent objects

        private string _name;

        public ThingList() {            
            _name = "unnamed";
        }

        public ThingList(String aName) {          
            _name = aName;
        }

        public string Name { 
            get => _name; 
            set => _name = value; 
        }      

        public bool IsNullOrEmpty() {
            return ((this == null) || (this.Count == 0));
        }
    }
}
