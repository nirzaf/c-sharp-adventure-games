using System.Collections.Generic;

namespace game.grammar {
    public class NounPhrase : GrammarUnit {
        // either a noun on its own or a now with adjectives
        // e.g. "ring" or "small golden ring"

        private List<string> _adjectives;

        public NounPhrase(string aNoun, List<string> someAdjectives)
            : base(aNoun) {
            Adjectives = someAdjectives;
        }

        public List<string> Adjectives { get => _adjectives; set => _adjectives = value; }

        public string Phrase() {
            string s = "";
            foreach(string a in _adjectives) {
                s += a + " ";
            }
            s += this.Word;
            return s;
        }
    }
}
