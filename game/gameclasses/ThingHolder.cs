using game.game.lists;

using game.grammar;
using globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.gameclasses {
    // A Thing that may contain a list of other things. Both a ContainerThing (such as a sack)
    // and a Room are descendents of ThingHolder. Also Actor (who can collect objects)
    [Serializable]
    public class ThingHolder : Thing {

        private ThingList _things = new ThingList();        
        private ThingList _thingsfound = new ThingList();
        private string _thingStr = "";
        private ThingList _flatlist = new ThingList(); // used by flatten() to return flat list


        // for objects that can't be opened: openable=false, open=true

        public ThingHolder(string aName, string aDescription, int aMass)
            : base(aName, aDescription, aMass) {
            _things = new ThingList();
        }

        public ThingHolder(string aName, string aDescription, bool isTakable, bool isMovable, int aMass)
            : base(aName, aDescription, isTakable, isMovable, aMass) {
            _things = new ThingList();
        }
        
        public ThingHolder(string aName, string aDescription, int aMass, ThingList tl)
            : base(aName, aDescription, aMass) {
            _things = tl;
        }

        public ThingHolder(string aName, string aDescription, bool isTakable, bool isMovable, ThingList tl, int aMass)
            : base(aName, aDescription, isTakable, isMovable, aMass) {
            _things = tl;
        }

        public ThingList Things {
            get => _things;
            set => _things = value;
        }

        public void AddThing(Thing aThing) {
            _things.Add(aThing);
            aThing.Container = this;
        }

        public void AddThings(ThingList aThingList) {                      
            foreach(Thing t in aThingList) {
                AddThing(t);
            }
        }

        private void DoDescribeThings(ThingHolder th, bool describeAll) {
            ThingList tlist = th.Things;
            ContainerThing container;

            foreach (Thing t in tlist) {               
                // don't show non-showable things (e.g. GenericThing objects such as lights)
                if (t.Show) {
                    _thingStr += t.DescriptionWithArticle() + "\n"; 
                }
                if (describeAll) {
                    container = ToContainerThing(t);
                    if ((container != null) && (container.IsOpen)) {
                        if (container.NumberOfThings() > 0) {
                            DoDescribeThings(container, describeAll);
                        }
                    }
                }
            }
        }

        // Describe all things - even those that are "in" other things
        public string DescribeThings() {
            _thingStr = "";
            DoDescribeThings(this, true);
            return _thingStr;
        }

        // Don't describe things that are "in" other things
        public string DescribeTopLevelThings() {
            _thingStr = "";
            DoDescribeThings(this, false);
            return _thingStr;
        }
      
        public static ContainerThing ToContainerThing(Thing t) {
            ContainerThing ct = null;

            if (t is ContainerThing thing) {
                ct = thing;
            }
            return ct;
        }

        public int NumberOfThings() {
            return _things.Count;
        }

        private ThingList ThingsToFlatList(ThingHolder th) {
            foreach (Thing t in th.Things) {
                _flatlist.Add(t);
                if (t is ContainerThing thing) {
                    ThingsToFlatList(thing);
                }
            }
            return _flatlist;
        }

        public ThingList Flatten() {
            _flatlist.Clear();
            return ThingsToFlatList(this);
        }
        private void FindThingInAnyList(ThingHolder th, NounPhrase np) {
            ContainerThing container;

            foreach (Thing t in th.Things) {
                if (t.MatchThing(np)) {                   
                    _thingsfound.Add(t);
                }
                container = ToContainerThing(t);
                if ((container != null) && (container.IsOpen)) {
                    FindThingInAnyList(container, np);
                }
            }
        }

        public bool ContainsThing(Thing t, bool searchAllContainers) {
            bool yes;

            if (searchAllContainers) {
                yes = ContainsThingInNestedLists(t);
            } else {
                yes = Things.Contains(t);
            }
            return yes;
        }

        // Is Thing t in list of objects owned by ThingHolder
        // including Things that may be inside other things
        // such as boxes and bags etc.?
        private bool ContainsThingInNestedLists(Thing t) {
            // searches for t in all lists (e.g. inside containers in containers)
            ThingList tl;
            bool yes;

            tl = Flatten();
            if (tl.Contains(t)) {
                yes = true;
            } else {
                yes = false;
            }
            return yes;
        }

        public ThingList FindThings(NounPhrase np) {       
            _thingsfound = new ThingList();
            FindThingInAnyList(this, np);
            return _thingsfound;
        }

        // Is Thing t in list of objects directly owned by ThingHolder
        // e.g. the top-level list of Room or Actor (not counting Things that
        // may be inside boxes and bags etc.)?
        public bool InTopLevelList(Thing t) {
            bool yes;

            if (_things.Contains(t)) {
                yes = true;
            } else {
                yes = false;
            }
            return yes;
        }

        public void Remove(Thing t) {
            _things.Remove(t); // returns true or false
            t.Container = null;
        }

        private void Add(Thing t) {
            _things.Add(t);
            t.Container = this;
        }
        protected void TransferOb(Thing t, ThingHolder from_TH, ThingHolder to_TH) {
            from_TH.Remove(t);
            to_TH.Add(t);
            t.Container = to_TH;
        }

    } // ThingHolder
}
