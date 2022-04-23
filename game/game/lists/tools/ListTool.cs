using game.gameclasses;

using System;
using System.Collections.Generic;

namespace game.game.lists.tools {
    class ListTool {


        /*
        * check there is exactly 1 thing in things list
        * and 1 exactly container in container_things list
        * If so, return "", otherwise return error message
        *
        * This checks objects found in response to commands such as:
        * "Put X into Y"
        * where there must be 1 of both X and Y
        *
        * X (thing1Desc) assumed to be in inventory - so you can so something with it
        * Y (thing2Desc) assumed to be 'here' so something can be done to it
        */
        public static string OneThingAndOneContainerInLists(
               ThingList things,
               ThingList container_things,
               string thing1Desc,
               string thing2Desc,
               string verb, string preposition) {
            string s = "";

            if (things.IsNullOrEmpty()) {
                s = "You haven't got " + thing1Desc;
            } else if (container_things.IsNullOrEmpty()) {
                s = "There is no " + thing2Desc + " here!";
            } else if (things.Count > 1) {
                s = "Which of these do you want to " + verb + " " + preposition + " the " + thing2Desc + "?\n";
                s += ListMultipleThings(things);
            } else if (container_things.Count > 1) {
                s = "Which " + thing2Desc + " do you want to put the " + thing1Desc + " into?\n";
                s += ListMultipleThings(container_things);
            }

            return s;
        }

        private static string ListMultipleThings(ThingList things) {
            string s = "";

            foreach (Thing t in things) {
                s += t.Description + "\n";
            }
            return s;
        }

        public static string OneThingInList(ThingList things, string thingDesc,
            string verb, bool mustBeInInventory = false) { //mustBeInInventory true for "drop" or "put into..."
            string s = "";

            if (things.IsNullOrEmpty()) {
                if (mustBeInInventory) {
                    s = "You haven't got " + thingDesc;
                } else {
                    s = "Can't see " + thingDesc + " here.";
                }
            } else if (things.Count > 1) {
                s = "Which of these do you want to " + verb + "?\n";
                s += ListMultipleThings(things);
            }
            return s;
        }

    }
}
