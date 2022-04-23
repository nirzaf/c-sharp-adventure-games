using globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.gameclasses {
    // A game object that may contaion other objects

    [Serializable]
    public class ContainerThing : ThingHolder {

        private bool _openable;
        private bool _isopen;
        private int _capacity;

        public ContainerThing(string aName, string aDescription, int aMass)
             : base(aName, aDescription, aMass, new ThingList(aDescription + " list")) {
            _isopen = true;
            _openable = false;
            _capacity = GetMass() * 2; // set default volume to twice mass
        }

        // For an openable object
        public ContainerThing(string aName, string aDescription, int aMass,
           bool canTake, bool canMove, bool canOpen, bool isOpen)
            : base(aName, aDescription, canTake, canMove, new ThingList(aDescription + " list"), aMass) {
            _openable = canOpen;
            _isopen = isOpen;
            _capacity = GetMass() * 2; // set default volume to twice mass
        }


        public ContainerThing(string aName, string aDescription, int aMass,
          bool canTake, bool canMove, bool canOpen, bool isOpen, ThingList tl)
           : base(aName, aDescription, canTake, canMove, tl, aMass) {
            _openable = canOpen;
            _isopen = isOpen;
            _capacity = GetMass() * 2; // set default volume to twice mass
        }

        public override string Open() {
            string s;

            if (!_openable) {
                s = "Can't open the " + Description;
            } else {
                if (_isopen) {
                    s = "The " + Description + " is already open.";
                } else {
                    _isopen = true;
                    s = "You open the " + Description;
                }
            }
            return s;
        }

        public override string Close() {
            string s;

            if (!_openable) {
                s = "Can't close the " + Description;
            } else {
                if (_isopen) {
                    _isopen = false;
                    s = "You close the " + Description;
                } else {
                    s = "The " + Description + " is already closed.";
                }
            }
            return s;
        }

        public override string DescribeLocation() {
            string s;
            s = $"The {Name} is {Description}";
            if (_openable) {
                if (_isopen) {
                    s += " (open)";
                } else {
                    s += " (closed)";
                }
            }
            if (_isopen) {
                if (Things.Count() > 0) {
                    s += "\nThere is something in it.";
                }
            }
            return s;
        }

        // return mass contents of Container
        public int ContentsMass() {
            ThingList tl;
            int countmass;

            tl = Flatten();
            countmass = 0;
            foreach (Thing t in tl) {
                countmass += t.GetMass();
            }
            return countmass;
        }

        // return mass of Container and all it contains

        public override int TotalMass() {
            return this.GetMass() + ContentsMass();
        }
        public bool Openable { get => _openable; set => _openable = value; }
        public bool IsOpen { get => _isopen; set => _isopen = value; }
        public int Capacity { get => _capacity; set => _capacity = value; }
    }
}