
using game.gameclasses;
using globals;
using System;

/*
 * A generic thing is a non-interactive game thing - part of the 'scenery' such
 * as lights or trees. The player may refer to them but they are not intended
 * to be used
 */

[Serializable]
class GenericThing : Thing {
    private bool _plural; // e.g. "lights", "trees"  

    public GenericThing(string aName, string aDescription, bool aPlural) 
        : base(aName, aDescription, false, false, Mass.UNKNOWN) { 
        _plural = aPlural;
        Show = false; // by default don't show among objects in room
    }

    public bool IsPlural() {
        return _plural;
    }
        
    protected override string Article(string s) {
        string a;

        if (IsPlural()) {
            //  a = "some ";
            a = "";
        } else {
            a = base.Article(s);
        }
        return a;
    }
        
    public override string DescribeLocation() {
        string d;
        string s;

        d = LongDescription;
        if (IsPlural()) {
            s = "They are " + Article(d) + d + ".";
        } else {
            s = "It is " + Article(d) + " " + d + ".";
        }
        return s;
    }

}
