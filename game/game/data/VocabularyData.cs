using globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.data {
    // Each word recognised by the parser must be added here along
    // with its Word "type" WT
    class VocabularyData {

        public static Dictionary<string, WT> vocab = new Dictionary<string, WT>();
        public static void InitVocab() {
            vocab.Add("acorn", WT.NOUN);
            vocab.Add("bed", WT.NOUN);
            vocab.Add("bin", WT.NOUN);
            vocab.Add("bone", WT.NOUN);
            vocab.Add("box", WT.NOUN);
            vocab.Add("button", WT.NOUN);
            vocab.Add("cheese", WT.NOUN);
            vocab.Add("chest", WT.NOUN);
            vocab.Add("coin", WT.NOUN);
            vocab.Add("diamond", WT.NOUN);
            vocab.Add("door", WT.NOUN);
            vocab.Add("dust", WT.NOUN);
            vocab.Add("gardenia", WT.NOUN);
            vocab.Add("key", WT.NOUN);
            vocab.Add("knife", WT.NOUN);
            vocab.Add("lamp", WT.NOUN);
            vocab.Add("leaflet", WT.NOUN);
            vocab.Add("lever", WT.NOUN);
            vocab.Add("pearl", WT.NOUN);
            vocab.Add("rat", WT.NOUN);
            vocab.Add("robot", WT.NOUN);
            vocab.Add("sack", WT.NOUN);
            vocab.Add("shop", WT.NOUN);
            vocab.Add("sign", WT.NOUN);
            vocab.Add("slot", WT.NOUN);
            vocab.Add("squirrel", WT.NOUN);
            vocab.Add("i", WT.VERB);
            vocab.Add("inventory", WT.VERB);
            vocab.Add("get", WT.VERB);
            vocab.Add("take", WT.VERB);
            vocab.Add("drop", WT.VERB);
            vocab.Add("put", WT.VERB);
            vocab.Add("look", WT.VERB);
            vocab.Add("open", WT.VERB);
            vocab.Add("close", WT.VERB);
            vocab.Add("pull", WT.VERB);
            vocab.Add("press", WT.VERB);
            vocab.Add("push", WT.VERB);
            vocab.Add("lock", WT.VERB);
            vocab.Add("unlock", WT.VERB);
            vocab.Add("n", WT.VERB);
            vocab.Add("s", WT.VERB);
            vocab.Add("w", WT.VERB);
            vocab.Add("e", WT.VERB);
            vocab.Add("u", WT.VERB);
            vocab.Add("d", WT.VERB);
            vocab.Add("l", WT.VERB);
            vocab.Add("q", WT.VERB);
            vocab.Add("quit", WT.VERB);
            vocab.Add("up", WT.VERB);
            vocab.Add("down", WT.VERB);
            vocab.Add("save", WT.VERB); // save and load added
            vocab.Add("load", WT.VERB);
            vocab.Add("test", WT.VERB);
            vocab.Add("big", WT.ADJECTIVE);
            vocab.Add("brass", WT.ADJECTIVE);
            vocab.Add("dirty", WT.ADJECTIVE);
            vocab.Add("glittering", WT.ADJECTIVE);
            vocab.Add("gold", WT.ADJECTIVE);
            vocab.Add("golden", WT.ADJECTIVE);
            vocab.Add("lovely", WT.ADJECTIVE);
            vocab.Add("old", WT.ADJECTIVE);
            vocab.Add("silver", WT.ADJECTIVE);
            vocab.Add("small", WT.ADJECTIVE);
            vocab.Add("smelly", WT.ADJECTIVE);
            vocab.Add("wood", WT.ADJECTIVE);
            vocab.Add("wooden", WT.ADJECTIVE);
            vocab.Add("a", WT.ARTICLE);
            vocab.Add("an", WT.ARTICLE);
            vocab.Add("the", WT.ARTICLE);
            vocab.Add("in", WT.PREPOSITION);
            vocab.Add("into", WT.PREPOSITION);
            vocab.Add("at", WT.PREPOSITION);
            vocab.Add("with", WT.PREPOSITION);
        }
    }
}
