// #define MYDEBUG

using game.grammar;
using game.data;
using globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using game.gameclasses;
using game.game.lists.tools;

namespace game {
    partial class Adventure {

        // ===============================================================
        //                        --- Parser ---
        // ===============================================================

        private static string last_input;

        /* Command types (examples):
         * VERB                           // e.g. N or Look
         * VERB+NOUN                      // e.g. Take X, Drop Y
         * VERB+PREPOSITION+NOUN          // e.g. Look at X
         * VERB+NOUN+PREPOSITION+NOUN     // e.g. Put X in Y
         * */

        char[] delimiters = { ' ', '.', ',', ':' }; // characters used to delimit words entered by user

        private Actor activeActor;

        private string ProcessVerbNounPhrasePrepositionNounPhrase(List<GrammarUnit> command) {
            string s = "";
            GrammarUnit gu1 = command[0];
            GrammarUnit gu2 = command[1];
            GrammarUnit gu3 = command[2];
            GrammarUnit gu4 = command[3];
            string verb_word = gu1.Word;
            string noun_word = gu2.Word;
            string preposition_word = gu3.Word;
            string noun_word2 = gu4.Word;

            Verb v = null;
            Preposition prep = null;
            NounPhrase np = null;
            NounPhrase np2 = null;

            if (gu1 is Verb verb) {
                v = verb;
            }
            if (gu2 is NounPhrase phrase) {
                np = phrase;
            }
            if (gu3 is Preposition preposition) {
                prep = preposition;
            }
            if (gu4 is NounPhrase phrase1) {
                np2 = phrase1;
            }

            if ((v == null) || (prep == null)) {
                s = "Can't do this because I don't understand how to '" + verb_word + " something " + preposition_word + "' something!";
            } else if (np == null) {
                s = "Can't do this because '" + noun_word + "' is not an object!";
            } else if (np2 == null) {
                s = "Can't do this because '" + noun_word2 + "' is not an object!";
            } else {
                switch (verb_word + preposition_word) {
                    case "putin":
                    case "putinto":         // allow either "put in" or "put into"...
                        s = activeActor.PutInto(np, np2);
                        break;
                    case "openwith":
                        s = activeActor.OpenWith(np, np2);
                        break;
                    case "lockwith":
                        s = activeActor.LockWith(np, np2);
                        break;
                    case "unlockwith":
                        s = activeActor.UnlockWith(np, np2);
                        break;
                    default:
                        s = $"I don't know how to {gu1.Word} the {gu2.Word} {gu3.Word} the {gu4.Word}!";
                        break;
                }
            }
            return s;
        }

        private string ProcessVerbPrepositionNounPhrase(List<GrammarUnit> command) {
            string s = "";
            GrammarUnit gu1 = command[0];
            GrammarUnit gu2 = command[1];
            GrammarUnit gu3 = command[2];
            string verb_word = gu1.Word;
            string preposition_word = gu2.Word;
            string noun_word = gu3.Word;
            Verb v = null;
            Preposition prep = null;
            NounPhrase np = null;

            if (gu1 is Verb verb) {
                v = verb;
            }
            if (gu2 is Preposition preposition) {
                prep = preposition;
            }
            if (gu3 is NounPhrase phrase) {
                np = phrase;
            }

            if ((v == null) || (prep == null)) {
                s = "Can't do this because I don't understand'" + last_input + "' !";
            } else if (np == null) {
                s = "Can't do this because '" + noun_word + "' is not an object!";
            } else {
                switch (verb_word + preposition_word) {
                    case "lookat":
                        s = activeActor.LookAt(np);
                        break;
                    case "lookin":
                    case "lookinto":
                        s = activeActor.LookIn(np);
                        break;
                    default:
                        s = $"I don't know how to {gu1.Word} {gu2.Word} a {gu3.Word}!";
                        break;
                }
            }
            return s;
        }

        private string ProcessVerbNounPhrase(List<GrammarUnit> command) {
            string s = "";
            GrammarUnit gu1 = command[0];
            GrammarUnit gu2 = command[1];
            string verb_word = gu1.Word;
            string noun_word = gu2.Word;
            Verb v = null;
            NounPhrase np = null;

            if (gu1 is Verb verb) {
                v = verb;
            }
            if (gu2 is NounPhrase phrase) {
                np = phrase;
            }
            if (v == null) {
                s = "Can't do this because '" + verb_word + "' is not a command!";
            } else if (np == null) {
                s = "Can't do this because '" + noun_word + "' is not an object!";
            } else {
                switch (verb_word) {
                    case "take":
                    case "get":
                        s = activeActor.Take(np);
                        break;
                    case "drop":
                        s = activeActor.Drop(np);
                        break;
                    case "open":
                        s = activeActor.Open(np);
                        break;
                    case "close":
                        s = activeActor.Close(np);
                        break;
                    case "pull":
                        s = activeActor.Pull(np);
                        break;
                    case "push":
                        s = activeActor.Push(np);
                        break;
                    default:
                        s = $"I don't know how to {verb_word} a {noun_word}!";
                        break;
                }
            }
            return s;
        }

        private string ProcessVerb(List<GrammarUnit> command) {
            string s = "";
            GrammarUnit gu = command[0];
            string word = gu.Word;
            Verb v = null;

            if (gu is Verb verb) {
                v = verb;
            }
            if (v == null) {
                s = $"Can't do this because '{word}' is not a command!";
            } else {
                switch (word) {
                    case "i":
                    case "inventory":
                        s = activeActor.Inventory();
                        break;
                    case "look":
                    case "l":
                        s = activeActor.Look();
                        break;
                    case "n":
                        s = activeActor.MoveTo(Dir.NORTH, Map);
                        break;
                    case "s":
                        s = activeActor.MoveTo(Dir.SOUTH, Map);
                        break;
                    case "w":
                        s = activeActor.MoveTo(Dir.WEST, Map);
                        break;
                    case "e":
                        s = activeActor.MoveTo(Dir.EAST, Map);
                        break;
                    case "up":
                    case "u":
                        s = activeActor.MoveTo(Dir.UP, Map);
                        break;
                    case "down":
                    case "d":
                        s = activeActor.MoveTo(Dir.DOWN, Map);
                        break;
                    case "q":           // need to 'understand' quit but do nothing here
                    case "quit":
                        s = "q";
                        break;
                    case "test":
                        Test();
                        break;
                    default:
                        s = $"Sorry, I can't {word}!";
                        break;
                }
            }
            return s;
        }

        // Note the use of a Tuple to return multiple values
        public Tuple<InteractiveActor, string> FindInteractiveActor(NounPhrase np) {
            string s;
            ThingList things;
            Thing t;
            InteractiveActor ia = null;

            things = Player.MatchingThingsHere(np);
            s = ListTool.OneThingInList(things, np.Phrase(), "speak to");
            if (s == "") {
                t = things[0];
                if (t is InteractiveActor actor) {
                    s = $"{t.Description} is an interactive actor.";
                    ia = actor;
                } else {
                    s = $"{t.Description} is not an interactive actor. It is a {t.GetType()}";
                    ia = null;
                }
            }
            return new Tuple<InteractiveActor, string>(ia, s);
        }

        // NOTE: Don't allow character to interact with itself (e.g. robot take robot)
        // Note the use of a Tuple to return multiple values
        private Tuple<TFN, string> IsInteractiveActor(List<GrammarUnit> grammarunits) {
            TFN t = TFN.NOTFOUND;
            InteractiveActor ia;
            string s = "";
            Tuple<InteractiveActor, string> ia_s;

            activeActor = _player; // defaults to player
            // Try to find InteractiveActor. If found, send commands to it rather than to player        
            if (grammarunits[0] is NounPhrase np) {
                ia_s = FindInteractiveActor(np);
                ia = ia_s.Item1;
                s = ia_s.Item2;
                if (ia == null) {
                    t = TFN.FALSE;
                } else {
                    activeActor = ia;
                    grammarunits.Remove(grammarunits[0]);
                    t = TFN.TRUE;
                }
            }
            return new Tuple<TFN, string>(t, s);
        }
        private string ProcessCommand(List<WordAndType> command) {
            string s = "";
            TFN t;
            SentenceAnalyzer analyzer;
            List<GrammarUnit> grammarunits = new List<GrammarUnit>();
            Tuple<TFN, string> tfn_s;

            analyzer = new SentenceAnalyzer(command);
            grammarunits = analyzer.Analyze();
            if (grammarunits.Count == 0) {
                s = "You must write a command!";
            } else if (analyzer.ContainsError()) {
                s = "Cannot understand that command - " + analyzer.Error;
            } else {                
                tfn_s = IsInteractiveActor(grammarunits);
                t = tfn_s.Item1;
                s = tfn_s.Item2;

                //------------ start debug                
#if MYDEBUG
                string temps;
                temps = "About to process command:";
                foreach (GrammarUnit gu in grammarunits) {
                    temps += "\nWord = " + gu.Word;
                    temps += "\nType = " + gu.GetType();
                    if (gu is NounPhrase phrase) {
                        List<string> adjectives = phrase.Adjectives;
                        temps += "\nAdjectives...";
                        foreach (string adj in adjectives) {
                            temps += adj + " ";
                        }
                    }
                }
                Console.WriteLine(temps);
#endif
                if (t != TFN.FALSE) {
                    //------------ end debug
                    switch (grammarunits.Count) {
                        case 1:
                            s = ProcessVerb(grammarunits);
                            break;
                        case 2:
                            s = ProcessVerbNounPhrase(grammarunits);
                            break;
                        case 3:
                            s = ProcessVerbPrepositionNounPhrase(grammarunits);
                            break;
                        case 4:
                            s = ProcessVerbNounPhrasePrepositionNounPhrase(grammarunits);
                            break;
                        default:
                            s = "\nI don't understand that command - it is too long!";
                            break;
                    }
                }
            }
            return s;
        }

        private string ParseCommand(List<string> wordlist) {
            List<WordAndType> command = new List<WordAndType>();
            WT wordtype;
            string errmsg = "";
            string s = "";

            foreach (string k in wordlist) {
                // Check to see if Key, s,
                // exists. If not, set WordType to ERROR
                if (VocabularyData.vocab.ContainsKey(k)) {
                    wordtype = VocabularyData.vocab[k];
                    if (wordtype == WT.ARTICLE) { // ignore articles such as "the" or "a"
                    } else {
                        command.Add(new WordAndType(k, wordtype));
                    }
                } else {    // if word isn't found in vocab
                    command.Add(new WordAndType(k, WT.ERROR));
                    errmsg = $"Sorry, I don't understand '{k}'";
                }
            }
            if (errmsg != "") {
                s = errmsg;
            } else {
                s = ProcessCommand(command);
            }
            return s;
        }

        public string RunCommand(string inputstr) {
            List<string> strlist;
            string s = "";

            string lowstr = inputstr.Trim().ToLower();
            if (lowstr == "") {
                s = "You must enter a command";
            } else {
                last_input = inputstr; // save user input (for error messages)
                strlist = new List<string>(inputstr.Split(delimiters, StringSplitOptions.RemoveEmptyEntries));
                s = ParseCommand(strlist);
            }
            return s;
        }
    }
}
