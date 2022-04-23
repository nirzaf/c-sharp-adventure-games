
using game.data;
using globals;
using System;
using System.Collections.Generic;

namespace game.gameclasses {
    [Serializable]
    public class InteractiveActor : Actor {
        /*
         * The main difference between an Actor and an InteractiveActor is that the latter
         * (this class) is an object that can be seen and interacted with, just like all other
         * game objects (e.g. a Room 'contains it') whereas an Actor knows its Location
         * (a Room) but does not appear as an object in that Room. 
         * */
        public InteractiveActor(string aName, string aDescription, Room aRoom, ThingList tl) :
            base(aName, aDescription, aRoom, tl) {
            _pronoun = "I";
            _tobe = "am";
            Takable = false;
        }

        /*
         * The Actor class (defining the player) only has a one-way reference to its Location
         * The player's Location references a room but the player is not an object in that
         * room. The InteractiveActor class (defining a character) needs a two-way reference: 
         * 1) The character keeps a refernce of the cirrent Room (Location)
         * 2) The room must 'contain' that character so it is listed among the 'things here'
         * and the player can interact with the character just as with other game objects.
         * 
         * That is why MoveTo() is overriden. In addition to changing the character's
         * Location, it must also transfer to character from the objects in one Room (the
         * previous Location) to the objects in the new room (the new Location). The
         * TransferOb() method takes care of this.
         * */
        
        public override string MoveTo(Dir direction, RoomList theMap) {
            string s;
            Room previousLocation;

            if (Location != null) {
                previousLocation = Location;
                if (base.DoMoveTo(direction, theMap) == Rm.NOEXIT) {
                    s = "There is no exit in that direction\r\n";
                } else {
                    TransferOb(this, previousLocation, Location);                                        
                    s = $"The {Description} goes {direction.ToString().ToLower()}, leaving the current location.";
                }
            } else {
                s = $"{_pronoun} {_tobe} not in a Room so {_pronoun} cannot move to another one";
            }
            return s;
        }

    }
}
