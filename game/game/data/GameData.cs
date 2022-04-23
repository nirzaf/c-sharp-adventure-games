
using game.gameclasses;
using globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using game.data;



namespace game.data {

    public class GameData {

        /*
           * 
           *                                  Attic -- Bedroom
           *                                     ^
           *                                     |
           *                           DesertedShop -- Kitchen
           *                                |    |
           *                                |    V
           *                                |  Basement
           *                                |
           *                   ------  DaggerStreet --------- Ripper Mews
           *                   |            |                   |
           * GoreStreet -- Alleyway -- OpiumTerrace  -------- GardenN       
           *    |                                               |  Oak Tree    Balcony
           *    |                                               |  ^              ^
           *    |                                               |  |              |
           *  DeadEnd               VegetableGarden --------- GardenS --------- PalmHouse
           *                                               
           *  
          * */

        private static RoomList _map;
        private static Actor _player;
        private static Dictionary<Ob, Thing> _obs;
        private static string _introtext;

        public static RoomList Map { 
            get => _map; 
            set => _map = value; 
        }
        public static Actor Player {
            get => _player;
            set => _player = value;
        }
        public static Dictionary<Ob, Thing> Obs {
            get => _obs;
            set => _obs = value;
        }
        public static string Introtext {
            get => _introtext;
        }
        
        public static Room GetRoom(Rm r) {
            return _map[r];
        }


        public static void InitGame() {
            // Initialize _map with Rooms
            _map = new RoomList {
                /*                  N                   S                   W                   E               Up              Down   */
               { Rm.GoreStreet, new Room("Gore Street", "a narrow, twisty street that smells of decay.\r\nThere is a sign on the wall",
                               Rm.NOEXIT, Rm.DeadEnd, Rm.NOEXIT, Rm.Alleyway) },
               { Rm.Alleyway, new Room("Alleyway", "a dismal alleyway enclosed by the crumbling walls of tall buildings",
                               Rm.NOEXIT, Rm.NOEXIT, Rm.GoreStreet, Rm.OpiumTerrace) },
               { Rm.DeadEnd, new Room("Dead End", "a sinister little street that leads nowhere.\r\nThere is a button on the wall",
                               Rm.GoreStreet, Rm.NOEXIT, Rm.NOEXIT, Rm.OpiumTerrace) },
               { Rm.OpiumTerrace, new Room("Opium Terrace", "a dark, dripping terrace illuminated by a hissing gas lamp.\r\nThere is a sign on the wall",
                               Rm.DaggerStreet, Rm.NOEXIT, Rm.Alleyway, Rm.GardenN) },
               { Rm.DaggerStreet, new Room("Dagger Street", "a long, winding street with exits to North and South. There is a narrow alleyway to the west." +
                                       "\r\nOn the northern side stands a deserted shop. There is a sign on the wall",
                               Rm.DesertedShop, Rm.OpiumTerrace, Rm.Alleyway, Rm.RipperMews) },
               { Rm.RipperMews, new Room("Ripper Mews", "a delightful little street filled with the scent of gardenia.\r\nThere is a sign on the wall",
                               Rm.NOEXIT, Rm.GardenN, Rm.DaggerStreet, Rm.NOEXIT) },
               { Rm.GardenN, new Room("Garden", "a lovely garden. A gardenia bush blossoms next to the gate. Walkways lead to other parts of the garden.\r\n" +
                                       "A squirrel suddenly runs across the grass in front of you. It seems to be holding a gold key." +
                                       "\r\nThe squirrel squeals then runs off into the distance.",
                               Rm.RipperMews, Rm.GardenS, Rm.OpiumTerrace, Rm.NOEXIT) },
               { Rm.GardenS, new Room("Garden", "a lovely garden. A mighty oak tree towers above you. Walkways lead to other parts of the garden",
                               Rm.GardenN, Rm.NOEXIT, Rm.VegetableGarden, Rm.PalmHouse, Rm.OakTree, Rm.NOEXIT) },
               { Rm.OakTree, new Room("Oak tree", "a mighty oak tree that towers over the garden. You have a glorious view of a splendid Palm House to your East," +
                                       "the vegetable garden to the West and other parts of the garden to the North. You vaguely see other parts of the city" +
                                       "in the distance, but the buildings are obscured by drifting fog.",
                               Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.GardenS) },
               { Rm.VegetableGarden, new Room("Vegetable garden", "a garden filled with sprouting vegetables in bewildering variety",
                               Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.GardenS) },
               { Rm.PalmHouse, new Room("Palm House", "a tall, elegant structure made of glass panes set in an cast-iron frame. The atmosphere is hot and humid." +
                                       "\r\nHuge palm trees rise high overhead. An elaborate cast-iron staircase leads up to a bacony",
                               Rm.NOEXIT, Rm.NOEXIT, Rm.GardenS, Rm.NOEXIT, Rm.Balcony, Rm.NOEXIT) },
               { Rm.Balcony, new Room("Balcony", "a balcony at the height of the tallest palms. All you can see are palm fronds everywhere. The windows are too misty to see through",
                               Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.PalmHouse) },
               { Rm.DesertedShop, new Room("Deserted shop", "a musty old shop. There is nothing here but dust",
                               Rm.NOEXIT, Rm.DaggerStreet, Rm.NOEXIT, Rm.Kitchen, Rm.Attic, Rm.Basement) },
               { Rm.Basement, new Room("Basement", "a dank, dripping basement that smells of fungus",
                               Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.DesertedShop, Rm.NOEXIT) },
               { Rm.Attic, new Room("Attic", "a cramped space under the roof. A hungry-looking rat is sitting on a rafter",
                               Rm.NOEXIT, Rm.NOEXIT, Rm.NOEXIT, Rm.Bedroom, Rm.NOEXIT, Rm.DesertedShop) },
               { Rm.Bedroom, new Room("Bedroom", "a tiny room containing a small, damp bed and a huge wooden chest",
                               Rm.NOEXIT, Rm.NOEXIT, Rm.Attic, Rm.NOEXIT) },
               { Rm.Kitchen, new Room("Kitchen", "an untidy kitchen that smells of rancid cheese",
                               Rm.NOEXIT, Rm.NOEXIT, Rm.DesertedShop, Rm.NOEXIT) }
             };


            // Create Dictionary of game objects
            _obs = new Dictionary<Ob, Thing> {
                // Basic Things
                { Ob.Acorn,     new Thing("acorn", "ordinary-looking acorn")},
                { Ob.Bed,       new Thing("bed", "nasty, rather damp bed", false, true)},
                { Ob.Bin,       new ContainerThing("bin", "smelly old bin", Mass.MEDIUM, true, true, true, true) {
                                    Adjectives = new List<string>(new string[] { "smelly", "old" }) } },
                { Ob.Bone,      new Thing("bone", "bone that looks as though it has been gnawed by a dog") },
                { Ob.BrassKey,  new Thing("key", "small brass key"){
                                    Adjectives = new List<string>(new string[] { "small", "brass" }) }},
                { Ob.Button,    new Thing("button", "glowing green button on the wall", false, true)},
                { Ob.Cheese,    new Thing("cheese", "smelly lump of cheese")},
                { Ob.Chest,     new LockableThing("chest", "wooden chest", Mass.MEDIUM, false, true, true, false,true) {
                                    Adjectives = new List<string>(new string[] { "wood", "wooden" }) }},                
                { Ob.Coin,      new Thing("coin", "small, silver coin of unknown denomination", Mass.TINY) },
                { Ob.Diamond,   new Thing ("diamond", "lovely glittering diamond", Mass.TINY){
                                    Adjectives = new List<string>(new string[] { "lovely", "glittering" })}},                       // initially has no location!!!
                { Ob.Dust,      new GenericThing("dust", "ordinary-looking dust", false)},
                { Ob.GoldKey,  new Thing("key", "small gold key"){                                                           // initially has no location!!!
                                    Adjectives = new List<string>(new string[] { "small", "gold", "golden" }) }},
                { Ob.Knife,     new Thing("knife", "sharp knife") },
                { Ob.Leaflet,   new Thing("leaflet", "small leaflet which says: 'Ripper Strikes Again!'") {
                                    Adjectives = new List<string>(new string[] { "small" })}},
                { Ob.Lamp,      new Thing("lamp", "hissing gas lamp is fixed high up on a wall. It produces a sickly, green, flickering light", false, false)},
                { Ob.Lever,     new Thing("lever", "lever fixed to the wall", false, true)},
                { Ob.Rat,       new Thing("rat", "scabby, hungry-looking rat with yellow teeth and green eyes.\r\nIt is sitting on the rafter sniffing the air and squeaking.", false, false)},                
                { Ob.Pearl,     new Thing("pearl", "pearl", Mass.TINY) },
                { Ob.Sack,      new ContainerThing("sack", "smelly old sack", Mass.MEDIUM, true, true, true, true) {
                                    Adjectives = new List<string>(new string[] { "smelly", "old" })}},
                { Ob.Shop,      new Thing("shop", "deserted shop", false, false)},
                { Ob.SignCoinOperated,  new Thing("sign", "sign that says: 'Coin operated'", false, true)},
                { Ob.SignDaggerStreet,  new Thing("sign", "sign that says 'Dagger Street'", false, false) },
                { Ob.SignGoreStreet,  new Thing("sign", "sign that says 'Gore Street'", false, false) },
                { Ob.SignOpiumTerrace, new Thing("sign", "sign that says 'Opium Terrace'", false, false) },
                { Ob.SilverBox,  new LockableThing("silver box", "small silver box", Mass.SMALL, true, true, true, false, true){
                                    Adjectives = new List<string>(new string[] { "small", "silver"}) }},
                { Ob.SilverKey,  new Thing("key", "small silver key"){
                                    Adjectives = new List<string>(new string[] { "small", "silver"}) }},
                { Ob.Slot,      new ContainerThing("slot","slot in the wall", Mass.TINY, false, false, false, true)},
                // InteractiveActors
                { Ob.Robot1,     new InteractiveActor("robot","small silver robot", null, new ThingList()){
                                    Adjectives = new List<string>(new string[] { "small", "silver"}) }},
                { Ob.Robot2,     new InteractiveActor("robot","small gold robot", null, new ThingList()){
                                    Adjectives = new List<string>(new string[] { "small", "gold"}) }}
            };


            // Add Things To Containers
            ((ContainerThing)_obs[Ob.Bin]).AddThings(new ThingList { _obs[Ob.Bone], _obs[Ob.Coin] });
            ((ContainerThing)_obs[Ob.Chest]).AddThing(_obs[Ob.Pearl]);

            // Set Other Special properties/fields
            ((LockableThing)_obs[Ob.SilverBox]).CanBeUnlockedWith(_obs[Ob.SilverKey]);
            ((LockableThing)_obs[Ob.Chest]).CanBeUnlockedWith(_obs[Ob.GoldKey]);
            _obs[Ob.SignGoreStreet].Show = false;

            // Add Things to selected Rooms (after Rooms are created and added to map)           
            _map[Rm.GoreStreet].AddThings(new ThingList { _obs[Ob.Leaflet], _obs[Ob.SignGoreStreet], _obs[Ob.Sack], _obs[Ob.Robot1], _obs[Ob.Robot2] });
            _map[Rm.Alleyway].AddThings(new ThingList { _obs[Ob.Bin], _obs[Ob.Knife] });
            _map[Rm.OpiumTerrace].AddThing(_obs[Ob.Lamp]);
            _map[Rm.OpiumTerrace].AddThing(_obs[Ob.SignOpiumTerrace]);
            _map[Rm.DaggerStreet].AddThing(_obs[Ob.SignDaggerStreet]);
            _map[Rm.DaggerStreet].AddThing(_obs[Ob.Shop]);
            _map[Rm.OakTree].AddThing(_obs[Ob.Acorn]);
            _map[Rm.Bedroom].AddThing(_obs[Ob.Bed]);
            _map[Rm.Bedroom].AddThing(_obs[Ob.Chest]);
            _map[Rm.Bedroom].AddThing(_obs[Ob.SilverKey]);
            _map[Rm.DesertedShop].AddThing(_obs[Ob.Dust]);
            _map[Rm.Attic].AddThing(_obs[Ob.Rat]);
            _map[Rm.Kitchen].AddThing(_obs[Ob.Cheese]);
            _map[Rm.Balcony].AddThing(_obs[Ob.Lever]);
            _map[Rm.Balcony].AddThing(_obs[Ob.SignCoinOperated]);
            _map[Rm.Balcony].AddThing(_obs[Ob.Slot]);
            _map[Rm.Basement].AddThing(_obs[Ob.SilverBox]);
            _map[Rm.DeadEnd].AddThing(_obs[Ob.Button]);
            _map[Rm.RipperMews].AddThing(_obs[Ob.BrassKey]);            
          
            _player = new Actor("You", "The Player", _map.RoomAt(Rm.GoreStreet), new ThingList());

            _introtext = $"Welcome to the Dark Neon City --- a futuristic Victorian adventure game";
        }

        public static string ObsInfo() {
            // For debugging - show details of all game objects
            string s = "";
            string cname = "";
            foreach (var item in _obs) {
                if (item.Value.Container == null) {
                    cname = "NULL";
                } else {
                    cname = $"({item.Value.Container.GetType().Name}) {item.Value.Container.Name}"; ;
                }
                s += $"{item.Key} Name:{item.Value.Name} Container:{cname} InGame:{item.Value.InGame}\n";
            }
            return s;
        }

    }
}