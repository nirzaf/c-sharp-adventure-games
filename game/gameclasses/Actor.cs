using game.data;
using game.game.lists;
using game.game.lists.tools;

using game.grammar;
using globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.gameclasses {
    /* A character capable of navigating a map and interacting
     * with objects. Every game will have at least one Actor - the player
     */

    [Serializable]
    public class Actor : ThingHolder {

        const int MAXLOAD = (int)Mass.MEDIUM + (int)Mass.SMALL;
        private int _load;
        protected string _pronoun;
        protected string _tobe;

        public Actor(string aName, string aDescription, Room aRoom, ThingList tl) :
           base(aName, aDescription, Mass.UNKNOWN, tl) {
            Container = aRoom;
            _pronoun = "You";
            _tobe = "are";
        }
        
        // NOTE: Always test if Location != null before tryng to use it
        // TODO: Consider return NullRoom (a room with no description and no exits)
        // instead of null (???)
        public Room Location {
            get {
                if (Container is Room room) {
                    return room;
                } else { // if Actor is not in a room (e.g. could be in inventory or a box etc) then Location is null
                    return null;
                }
            }
            set => Container = value;
        }

        public override string DescribeLocation() {
            return $"[{Name}] ({Description}) is in {Location.DescribeLocation()}";
        }

        public string Look() {
            string s = "";

            if (Location != null) {
                s = $"{_pronoun} {_tobe} in {Location.DescribeLocation()}";
            }else if (Container != null) {
                if (Container is Actor) {
                    s = $"{_pronoun} {_tobe} in the inventory of {Container.Description}";
                } else {
                    s = $"{_pronoun} {_tobe} in the {Container.Description}";
                }
            } else {
                s = "Cannot determine location";
            }
            return s;
        }

        public String Inventory() {
            String s;

            s = DescribeThings();
            if (s == "") {
                s = "nothing";
            }
            return $"{_pronoun} have {s}";
        }

        
        // I think the problem here is that this method assumes container must be a room
        public ThingList MatchingThingsInRoom(NounPhrase np) {
            Room r;
            ThingList things;

            things = new ThingList();            
            r = Location;
            if (Location != null) { // will be null if (for example) Actor is inside a container
                things = (r.FindThings(np));
            }
            return things;
        }

        public ThingList MatchingThingsInInventory(NounPhrase np) {
            ThingList things;
            ThingList tl;

            tl = new ThingList();
            things = FindThings(np);
            return things;
        }

        public ThingList MatchingThingsHere(NounPhrase np) {
            ThingList tl;
            tl = new ThingList();

            tl = MatchingThingsInInventory(np);
            tl.AddRange(MatchingThingsInRoom(np));
            return tl;
        }

        private string DoTake(Thing t) {
            string s = "";
            ContainerThing ct;
            int tmass;

            tmass = t.TotalMass();
            if (t.Takable) {
                if (this.InTopLevelList(t)) {
                    s = $"{_pronoun} already have the " + t.Description;
                } else if ((_load + tmass) > MAXLOAD) {
                    s = $"{_pronoun} {_tobe} carrying too much. {_pronoun} may need to drop\n"
                            + "something before taking the " + t.Description;
                } else {
                    _load += tmass;
                    // special action?
                    s = Actions.TakeSpecial(t, this.Location);
                    if (s == "") {
                        TransferOb(t, t.Container, this);
                        if (t.Container is ContainerThing) {
                            s = $"{_pronoun} take the " + t.Description + " from the " + t.Container.Description;
                        } else {
                            s = t.Description + " taken!";
                        }
                    }
                }
            } else { // it's not Takable
                s = $"{_pronoun} can't take the " + t.Description + "!";
            }
#if MYDEBUG
            ct = ToContainerThing(t);
            s += DebugTakeDrop(t, tmass, ct != null);
#endif
            return s;
        }

        public string Take(NounPhrase np) {
            string s;
            Thing t;
            ThingList things;

            things = MatchingThingsHere(np);
            s = ListTool.OneThingInList(things, np.Phrase(), "take");
            if (s == "") {
                t = things[0];
                if (t == this) {
                    s = "You cannot take yourself!";
                } else {
                    s = DoTake(t);
                }
            }
            return s;
        }


        public string Drop(NounPhrase np) {
            string s;
            ThingList things;
            Thing t;
            ContainerThing ct;
            int tmass;

            things = MatchingThingsInInventory(np);
            s = ListTool.OneThingInList(things, np.Phrase(), "drop", true);
            if (s == "") {
                t = things[0];
                tmass = t.TotalMass();
                // special action?
                _load -= tmass;
                s = Actions.DropSpecial(t, this.Location);
                if (s == "") {
                    TransferOb(t, t.Container, this.Location);                    
                    s = t.Description + " dropped!";
#if MYDEBUG
                    ct = ToContainerThing(t);
                    s += DebugTakeDrop(t, tmass, ct != null);

#endif
                }
            }
            return s;
        }

        // Only used for Debugging
        private string DebugTakeDrop(Thing t, int totalMass, bool isContainer) {
            string s = "";

            s += "\nPlayer load=" + _load + " MAXLOAD=" + MAXLOAD
                    + ". Mass of " + t.Name + "=" + t.GetMass();
            if (isContainer) {
                s += " (This is a Container) total mass = " + totalMass;
            }
            return s;
        }

        public string Open(NounPhrase np) {
            string s;
            ThingList things;
            Thing t;

            things = MatchingThingsHere(np);
            s = ListTool.OneThingInList(things, np.Phrase(), "open");
            if (s == "") {
                t = things[0];
                s = t.Open();
            }
            return s;
        }

        public string Close(NounPhrase np) {
            string s;
            ThingList things;
            Thing t;

            things = MatchingThingsHere(np);
            s = ListTool.OneThingInList(things, np.Phrase(), "close");
            if (s == "") {
                t = things[0];
                s = t.Close();
            }
            return s;
        }

        public string Pull(NounPhrase np) {
            string s;
            ThingList things;
            Thing t;

            things = MatchingThingsHere(np);
            s = ListTool.OneThingInList(things, np.Phrase(), "pull");
            if (s == "") {
                t = things[0];
                // special action?
                s = Actions.PullSpecial(t, this.Location);
                if (s == "") {
                    if (t.Movable) {
                        s = $"The {np.Phrase()} moves slightly when {_pronoun} pull it.";
                    } else {
                        s = $"{_pronoun} try to pull the {np.Phrase()} but nothing happens.";
                    }
                }
            }
            return s;
        }

        public string Push(NounPhrase np) {
            string s;
            ThingList things;
            Thing t;

            things = MatchingThingsHere(np);
            s = ListTool.OneThingInList(things, np.Phrase(), "push");
            if (s == "") {
                t = things[0];
                if (t.Movable) {
                    s = $"The {np.Phrase()} moves slightly when {_pronoun} push it.";
                } else {
                    s = $"{_pronoun} try to push the {np.Phrase()} but nothing happens.";
                }
            }
            return s;
        }

        public string LookIn(NounPhrase np) {
            string s = "";
            Thing t;
            ContainerThing container;
            ThingList things;

            things = MatchingThingsHere(np);
            s = ListTool.OneThingInList(things, np.Phrase(), "look in");
            if (s == "") {
                t = things[0];
                container = ToContainerThing(t);
                if (container == null) {
                    s = $"{_pronoun} can't look inside the " + t.Description + ".";
                } else {
                    if (container.IsOpen) {
                        s = container.DescribeThings();
                        if (s == "") {
                            s = "There is nothing in the " + container.Description;
                        } else {
                            s = "The " + container.Description + " contains:\n" + s;
                        }
                    } else {
                        s += "The " + container.Description + " isn't open.";
                    }
                }
            }
            return s;
        }

        public string LookAt(NounPhrase np) {
            string s = "";
            Thing t;
            ThingList things;

            things = MatchingThingsHere(np);
            s = ListTool.OneThingInList(things, np.Phrase(), "look at");
            if (s == "") {
                t = things[0];
                s += t.Description;
            }
            return s;
        }

        private void DoPutInto(Thing t, ThingHolder fromThingHolder, ContainerThing container) {
            TransferOb(t, fromThingHolder, container);
            // is Container visible in this room?
            if (this.Location.ContainsThing(container, true)) {
                // if putting into container in Room
                _load -= t.GetMass(); // decrease load  
            }

        }

        private string PutInto_SanityCheck(Thing t, ContainerThing container,
            string thingStr, String containerStr) {
            string s = "";

            if ((container == null)) {      // container is not a ContainerThing
                s = $"{_pronoun} can't put the " + thingStr + " into the " + containerStr + "!";
            } else if (t == container) {
                s = $"{_pronoun} can't put the " + thingStr + " into itself!";
            } else if (container.ContainsThing(t, true)) {
                s = "The " + thingStr + " is already in the " + containerStr;
            } else if (!(container).IsOpen) {
                s = $"{_pronoun} can't put the " + thingStr + " into a closed " + containerStr + "!";
            } else if (container.IsIn(t)) {
                s = $"{_pronoun} can't put the " + thingStr + " into the " + containerStr
                        + "\nbecause the " + containerStr + " is already in the " + thingStr + "!";
            } else if ((container.TotalMass() + t.TotalMass()) > container.Capacity) {
                s = "The " + containerStr + " isn't big enough for the " + thingStr;
                if (container.NumberOfThings() > 0) {
                    s += $"\nMaybe {_pronoun} could take something out of it and try again?";
                }
            }
            return s;
        }

        public string PutInto(NounPhrase np, NounPhrase np2) {
            string s;
            Thing t;
            ThingList things;
            ThingList container_things;
            ContainerThing container;

            things = MatchingThingsInInventory(np);
            container_things = MatchingThingsHere(np2);
            s = ListTool.OneThingAndOneContainerInLists(things, container_things, np.Phrase(),
                    np2.Phrase(), "put", "into");
            if (s == "") {
                // if Thing and Container are found                                        
                t = things[0];
                container = ToContainerThing(container_things[0]);
                s = PutInto_SanityCheck(t, container, np.Phrase(), np2.Phrase());
                if (s == "") {
                    // Are there any "special actions" defined when t is put
                    // into container? If so, do them instead of default actions
                    s = Actions.PutIntoSpecial(t, container);
                    if (s == "") {
                        DoPutInto(t, t.Container, container);
                        s = $"{_pronoun} put the " + np.Phrase() + " into the " + np2.Phrase() + ".";
                    }
                }
            }
            return s;
        }

        public string OpenWith(NounPhrase np, NounPhrase np2) {
            string s;
            ThingList thingToOpenList;
            ThingList thingToUseList;

            thingToOpenList = MatchingThingsHere(np);
            thingToUseList = MatchingThingsInInventory(np2);
            s = ListTool.OneThingInList(thingToOpenList, np.Phrase(), "open");
            if (s == "") {
                s = ListTool.OneThingInList(thingToUseList, np2.Phrase(), "use", true);
                if (s == "") {
                    s = $"{_pronoun} can't open the " + np.Phrase() + " with the " + np2.Phrase();
                }
            }
            return s;
        }

        public string LockWith(NounPhrase np, NounPhrase np2) {
            string s;
            Thing t;
            Thing t2;
            ThingList thingToOpenList;
            ThingList thingToUseList;

            thingToOpenList = MatchingThingsHere(np);
            thingToUseList = MatchingThingsInInventory(np2);
            s = ListTool.OneThingInList(thingToOpenList, np.Phrase(), "lock");
            if (s == "") {
                s = ListTool.OneThingInList(thingToUseList, np2.Phrase(), "use", true);
                if (s == "") {
                    t = thingToOpenList[0];
                    t2 = thingToUseList[0];
                    if (t is LockableThing lockablething) {
                        s = lockablething.TrytolockWith(t2);
                    } else {
                        s = $"{_pronoun} can't lock the " + np.Phrase() + " with the " + np2.Phrase();
                    }
                }
            }
            return s;
        }

        public string UnlockWith(NounPhrase np, NounPhrase np2) {
            string s;
            Thing t;
            Thing t2;
            ThingList thingToOpenList;
            ThingList thingToUseList;

            thingToOpenList = MatchingThingsHere(np);
            thingToUseList = MatchingThingsInInventory(np2);
            s = ListTool.OneThingInList(thingToOpenList, np.Phrase(), "unlock");
            if (s == "") {
                s = ListTool.OneThingInList(thingToUseList, np2.Phrase(), "use", true);
                if (s == "") {
                    t = thingToOpenList[0];
                    t2 = thingToUseList[0];
                    if (t is LockableThing lockablething) {
                        s = lockablething.TrytounlockWith(t2);
                    } else {
                        s = $"{_pronoun} can't unlock the " + np.Phrase() + " with the " + np2.Phrase();
                    }
                }
            }
            return s;
        }

        protected Rm DoMoveTo(Dir direction, RoomList theMap) {
            // Location should be a Room. But it may be null (if the
            // actor has been taken or put into something, for example).
            Room r = Location;
            Rm exit = Rm.NOEXIT;

            if (Location != null) {
                switch (direction) {
                    case Dir.NORTH:
                        exit = r.N;
                        break;
                    case Dir.SOUTH:
                        exit = r.S;
                        break;
                    case Dir.EAST:
                        exit = r.E;
                        break;
                    case Dir.WEST:
                        exit = r.W;
                        break;
                    case Dir.UP:
                        exit = r.Up;
                        break;
                    case Dir.DOWN:
                        exit = r.Down;
                        break;
                    default:
                        exit = Rm.NOEXIT;
                        break;
                }
                if (exit != Rm.NOEXIT) {
                    Location = theMap.RoomAt(exit);
                }
            }
            return exit;
        }

        public virtual string MoveTo(Dir direction, RoomList theMap) {
            string s;

            if (Location != null) {
                if (DoMoveTo(direction, theMap) == Rm.NOEXIT) {
                    s = "There is no exit in that direction\r\n";
                } else {
                    s = Location.DescribeLocation();
                }
            } else {
                s = $"{_pronoun} {_tobe} not in a Room so cannot move to another one";
            }
            return s;
        }
    }
}
