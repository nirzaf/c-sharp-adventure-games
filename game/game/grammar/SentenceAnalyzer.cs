using globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.grammar {
    public class SentenceAnalyzer {
        // Call Analyze()
        // Take a list of WordAndType
        // Return list of GrammarUnit

/* e.g. input list of WordAndType (String->WT) where WT is an enum const: 
 *                              "take"->VERB, "small"->AJECTIVE, "coin"->NOUN
 * output list of GrammarUnit objects:
 *                              Verb (Object) Word: "take" 
 *                              NounPhrase (Object) Word: "coin", Ajectives: "small"
 */

private List<GrammarUnit> _sentence;
private List<WordAndType> _rest;
private string _error;

public SentenceAnalyzer(List<WordAndType> wtlist) {
    _rest = wtlist;
    _sentence = new List<GrammarUnit>();
    _error = "";
}

private WordAndType GetNextElement() {
    if(_rest.Count == 0) {
        return null;
    } else {
        return _rest.First();
    }
}
private List<string> GetAdjectives(WordAndType wt) {
    List<string> adjectives = new List<string>();
    bool runloop = true;

    while(runloop) {
        if(wt == null) {
            runloop = false;
        } else if(wt.Type == WT.ADJECTIVE) {
            adjectives.Add(wt.Word);
            _rest.Remove(wt);
            wt = GetNextElement();
        } else {
            runloop = false;
        }
    }
    return adjectives;
}

public string Error {
    get => _error;
}

public bool ContainsError() {
    bool yes = false;
    _error = "";
    foreach(GrammarUnit gu in _sentence) {
        if(gu is GrammarError) {
            _error += ((GrammarError)gu).Word + "! ";
            yes = true;
        }
    }
    return yes;
}

private string GetNoun(WordAndType wt) {
    string s = "";

    if(wt != null) {
        if(wt.Type == WT.NOUN) {
            s = wt.Word;
        }
    }
    return s;
}

private void AddNounPhrase(WordAndType wt) {
    WordAndType nextWT;
    string noun;
    List<string> adjectives;

    nextWT = wt;
    adjectives = GetAdjectives(nextWT);           
    if(adjectives.Count == 0) {
        noun = GetNoun(nextWT);
    } else {
        nextWT = GetNextElement();
        noun = GetNoun(nextWT);
    }
    if(String.IsNullOrEmpty(noun)) {
        AddError("Missing Noun");
    } else {
        _sentence.Add(new NounPhrase(noun, adjectives));
        _rest.Remove(nextWT);
    }
}

private void AddError(string errorMsg) {
    _sentence.Add(new GrammarError(errorMsg));
}

private void AddPreposition(WordAndType wt) {
    _sentence.Add(new Preposition(wt.Word));
    _rest.Remove(wt);
}

private void AddVerb(WordAndType wt) {
    _sentence.Add(new Verb(wt.Word));
    _rest.Remove(wt);
}

public List<GrammarUnit> Analyze() {
    WordAndType wt;
    string word;

    while(_rest.Count > 0) {
        wt = GetNextElement();
        word = wt.Word;
        switch(wt.Type) {
            case WT.VERB:
                AddVerb(wt);
                break;
            case WT.ADJECTIVE:
            case WT.NOUN:
                AddNounPhrase(wt);
                break;
            case WT.PREPOSITION:
                AddPreposition(wt);
                break;
            case WT.ERROR:
                AddError("Grammar analysis ERROR");
                break;
            default:
                break;
        }
    }
    return _sentence;
}

}
}
