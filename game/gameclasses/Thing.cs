using game.grammar;
using globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.gameclasses {
    // The most basic game object. Actual Things such as Treasures may be created from this
    // It also has more specialised descendents that define things with special characteristics

    [Serializable]
    public class Thing : BaseThing {
        private string _long_description;
        private bool _takable;
        private bool _movable;
        private bool _show; // show as one of the objects in the room? 
        private ThingHolder _container;
        private List<string> _adjectives;
        private State _obstate;
        private int _mass;

        public Thing(string aName, string aDescription, int aMass = Mass.SMALL)
            : base(aName, aDescription) {
            // standard constructor:            
            _mass = aMass;
            _takable = true; // set default value
            _movable = true; // set default value
            _long_description = "";
            _show = true;
            _adjectives = new List<string>();
            _container = null;
            _obstate = State.NORMAL;
            TestMass();
        }

        public Thing(string aName, string aDescription, bool isTakable, bool isMovable, int aMass = Mass.SMALL)
             : base(aName, aDescription) {
            // alternative constructor
            _mass = aMass;
            _takable = isTakable;
            _movable = isMovable;
            _long_description = "";
            _show = true;
            _adjectives = new List<string>();
            _container = null;
            _obstate = State.NORMAL;
            TestMass();
        }

        private void TestMass() {
            if ((_mass < Mass.UNKNOWN) || (_mass > Mass.HUGE)) {
                throw new IncorrectMassException(Name + "--> Mass value " + _mass + " is invalid!");
            }
        }

        public int GetMass() {
            return _mass;
        }

        public virtual int TotalMass() {
            return _mass;
        }
        // Given a string, return an appropriate article ("a" or "an")
        protected virtual string Article(string s) {
            char initial;
            string a;

            a = "a";
            char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
            initial = (s.ToLower()[0]);
            foreach (char c in vowels) {
                if (c == initial) {
                    a = "an";
                }
            }
            return a;
        }

        public bool Takable {
            get => _takable;
            set => _takable = value;
        }
        public bool Movable {
            get => _movable;
            set => _movable = value;
        }
        public List<string> Adjectives {
            get => _adjectives;
            set => _adjectives = value;
        }

        public string AdjectivesAsString() {
            string s = "";
            foreach (string adj in _adjectives) {
                s += adj + " ";
            }
            return s;
        }

        public string LongDescription {
            get {
                string d;
                if (_long_description == "") {
                    d = Description;
                } else {
                    d = _long_description;
                }
                return d;
            }
            set => _long_description = value;
        }

        public bool Show {
            get => _show;
            set => _show = value;
        }

        public State ObState {
            get => _obstate;
            set => _obstate = value;
        }

        public virtual string Open() {
            return "Cannot open " + Description + " because it isn't a container.";
        }

        public virtual string Close() {
            return "Cannot close " + Description + " because it isn't a container.";
        }

        public virtual string DescribeLocation() {
            string d = "";

            if (Container is ContainerThing) {
                d = $"(In {Container.Name}) ";
            }
            d += $"It is {Article(LongDescription)} {LongDescription}.";
            return d;
        }

        public string DescriptionWithArticle() {
            return $"{Article(Description)} {Description}";
        }

        public ThingHolder Container {
            get => _container;
            set => _container = value;
        }

        private bool AdjectivesMatch(List<string> someAdjectives) {
            bool ok = true;
            foreach (string a in someAdjectives) {
                if (!_adjectives.Contains(a)) {
                    ok = false;
                }
            }
            return ok;
        }

        // try to find a Thing with the name and adjectives (if any)
        // supplied by the NounPhrase np
        public bool MatchThing(NounPhrase np) {
            bool match = false;
            if ((Name == np.Word) && AdjectivesMatch(np.Adjectives)) {
                match = true;
            }
            return match;
        }

        private bool IsInside(ContainerThing aContainer) {
            ThingHolder th;
            bool isInContainer = false;

            th = this.Container;
            while (th != null) {
                if (th == aContainer) {
                    isInContainer = true;
                }
                th = th.Container;
            }
            return isInContainer;
        }

        public bool IsIn(Thing t) {
            return (t is ContainerThing thing) && (IsInside(thing));
        }

        public bool InGame {
            get => Container != null;
        }

    }

    // This exception will be thrown when a Thing object is created if the mass is invalid
    // Its aim is to prevent compilation until the error is fixed
    class IncorrectMassException : SystemException {

        public IncorrectMassException(string errorMessage) {
            Console.WriteLine(errorMessage);
        }
    }
}
