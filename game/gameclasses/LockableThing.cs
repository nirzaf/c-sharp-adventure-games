
using game.gameclasses;
using globals;
using System;

namespace game.gameclasses {
    // A container that mey be locked or unlocked using some other object such as a key
    [Serializable]
    class LockableThing : ContainerThing {

        private bool _locked;
        private Thing _thingToUnlockWith; // e.g. a specific key

        public LockableThing(string aName, string aDescription, int aMass, bool isLocked) :
            base(aName, aDescription, aMass) {
            _locked = isLocked;
            _thingToUnlockWith = null;
        }

        public LockableThing(string aName, string aDescription, int aMass,
                bool canTake, bool canMove, bool canOpen, bool isOpen,
                bool isLocked)
            : base(aName, aDescription, aMass, canTake, canMove, canOpen, isOpen) {
            _locked = isLocked;
            _thingToUnlockWith = null;
        }

        // This is the thing (e.g. a key) that can unlock this LockableThing
        public void CanBeUnlockedWith(Thing t) {
            _thingToUnlockWith = t;
        }

        public string TrytounlockWith(Thing t) {
            string s;

            if (!_locked) {
                s = Description + " is already unlocked";
            } else if (t == _thingToUnlockWith) {
                _locked = false;
                s = "You unlock the " + this.Description + " with the " + t.Description;
            } else {
                s = "You can't unlock the " + this.Description + " with the " + t.Description;
            }
            return s;
        }

        public string TrytolockWith(Thing t) {
            string s;

            if (_locked) {
                s = "The " + Description + " is already locked.";
            } else if (IsOpen) {
                s = "You cannot lock the " + Description + " while it is open.";
            } else if (t == _thingToUnlockWith) {
                _locked = true;
                s = "You lock the " + Description;
            } else {
                s = "You can't lock the " + this.Description + " with the " + t.Description;
            }
            return s;
        }

        public override string Open() {
            string s;

            if (_locked) {
                s = "You can't open the " + Description + " because it's locked.";
            } else {
                s = base.Open();
            }
            return s;
        }
    }
}