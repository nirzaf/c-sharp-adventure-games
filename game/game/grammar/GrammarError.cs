using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.grammar {
    class GrammarError : GrammarUnit {
        // A Simple type that defines an error object that may be added by the
        // parser when an attempt to analyse a valid sentence fails
        public GrammarError(String aWord) : base(aWord) {

        }
    }
}
