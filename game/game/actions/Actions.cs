
using game.data;
using game.gameclasses;
using globals;
using System;

class Actions {

    public static string PutIntoSpecial(Thing t, ContainerThing container) {
        string s = "";

        if ((t == GameData.Obs[Ob.Coin]) && (container == GameData.Obs[Ob.Slot])) {
            s = "The sound of turning cogwheels can be heard deep inside the wall\n"
                + "The sign flickers and some next text appears on it";
            GameData.Obs[Ob.SignCoinOperated].Description = "The sign says: 'Coin operated (Pull lever to activate)'";
            GameData.Obs[Ob.Lever].ObState = State.ACTIVATED;
            t.Container.Remove(t);
        }

        return s;
    }

    public static string DropSpecial(Thing t, Room r) {
        string s;
        s = "";
        if (t == GameData.Obs[Ob.Acorn] && (r == GameData.GetRoom(Rm.GardenN))) {
            r.AddThing(GameData.Obs[Ob.GoldKey]);
            t.Container.Remove(t);  // acorn leaves game!
            r.Description = "a grassy area";
            s = "A squirrel suddenly runs across the grass in front of you. It seems to be holding a gold key." +
                "\r\nThe squirrel squeals, drops the key, picks up the acorn, then runs off into the distance.";
        }
        return s;
    }

    public static string TakeSpecial(Thing t, Room r) {
        string s;
        s = "";
        return s;
    }


    public static string PullSpecial(Thing t, Room r) {
        string s = "";
        if (t == GameData.Obs[Ob.Lever]) {
            if (t.ObState == State.ACTIVATED) {
                r.AddThing(GameData.Obs[Ob.Diamond]);   // add diamond to game
                t.ObState = State.NORMAL;
                s = "A lovely glittering diamond falls out of the slot";
            }
        }
        return s;
    }
}
