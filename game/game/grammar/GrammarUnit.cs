namespace game.grammar {
    public class GrammarUnit {
        // The base glass for grammatical elements such as NounPhrase, Verb etc.
        private string _word;

        public GrammarUnit(string aWord) {
            _word = aWord;
        }
        public string Word { get => _word; }
    }
}
