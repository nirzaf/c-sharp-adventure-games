using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using game.data;
using game.gameclasses;
using game.grammar;
using globals;

namespace game {
    /// <summary>
    /// --------------------------------------
    /// BIFF (beta)
    /// Bitwise Interactive Fiction Framework 
    /// --------------------------------------
    /// Copyright (c) Huw Collingbourne
    /// This game is based on the adventure game framework created by Huw Collingbourne
    /// This code may be used 'as is' or modified to create new games as long
    /// as this copyright notice and website links are included.
    /// http://www.bitwisebooks.com
    /// http://www.bitwisecourses.com
    /// </summary>
    /// 

    /// The Adventure class contains the 'world' of the game:
    /// It creates and initializes the player and the map.

    [Serializable]
    public partial class Adventure {
        private RoomList _map;
        private Actor _player;       

        public Adventure() {
            VocabularyData.InitVocab();        //!! Be sure to initialize the vocabulary list...
            GameData.InitGame();
            _player = GameData.Player;
            _map = GameData.Map;
        }


        //// --- Player
        public Actor Player {
            get => _player;
        } // Player
        public RoomList Map { get => _map; set => _map = value; }
        
        public string Look() {
            return "You are in " + _player.Location.DescribeLocation();
        }
      
        public string TestCmd(string inputstr) {
            string s;
            string lowstr;

            s = "ok";
            lowstr = inputstr.Trim().ToLower();

            if (lowstr != "q") {
                if (lowstr == "") {
                    s = "You must enter a command";
                } else {
                    s = RunCommand(inputstr);
                }
            }
            return s;
        }
        void ShowStr(String s) {
            if (s != "") {
                Console.WriteLine(s.Trim());
            }
        }
        void ShowTest(String s) {
            ShowStr("> " + s);
            ShowStr(TestCmd(s));
        }
        void WalkThrough() {
            /* A 'sample game' to ket you try out map navigation and puzzles
             * by automatically entering commands 
             */
            // Gore Street
            ShowTest("robot, take leaflet");
            ShowTest("gold robot, take leaflet");
            ShowTest("gold robot, put the leaflet into the sack");
            ShowTest("gold robot, e");
            ShowTest("e");                  // Alleyway
            ShowTest("look");
            ShowTest("take coin");
            ShowTest("e");                  // OpiumTerrace
            ShowTest("n");                  // Dagger Street
            ShowTest("n");                  // DesertedShop
            ShowTest("up");                 // Attic
            ShowTest("e");                  // Bedroom
            ShowTest("open chest");
            ShowTest("unlock chest");
            ShowTest("take key");
            ShowTest("unlock chest with key");
            ShowTest("w");                  // Attic
            ShowTest("d");                  // DesertedShop
            ShowTest("s");                  // DaggerStreet
            ShowTest("e");                  // Ripper Mews
            ShowTest("s");                  // GardenN
            ShowTest("s");                  // GardenS
            ShowTest("e");                  // PalmHouse
            ShowTest("up");                 // Balcony
            ShowTest("look at sign");
            ShowTest("i");
            ShowTest("pull lever");
            ShowTest("put coin in slot");
            ShowTest("look at sign");
            ShowTest("pull lever");
            ShowTest("i");
            ShowTest("take the lovely glittering diamond");
            ShowTest("pull lever");
            ShowTest("d");                  // PalmHouse
            ShowTest("w");                  // GardenS
            ShowTest("up");                 // Oak Tree
            ShowTest("get acorn");
            ShowTest("down");               // GardenS
            ShowTest("n");                  // GardenN
            ShowTest("drop acorn");
            ShowTest("get gold key");
            ShowTest("w");                  // Opium Terrace
            ShowTest("n");                  // Dagger Street
            ShowTest("n");                  // DesertedShop
            ShowTest("up");                 // Attic
            ShowTest("e");                  // Bedroom          
            ShowTest("unlock chest with key");
            ShowTest("unlock chest with brass key");
            ShowTest("unlock chest with silver key");
            ShowTest("unlock chest with gold key");
            ShowTest("open chest");
            ShowTest("take the pearl");
        }
        void Test() {
            // For debugging, enter the command "test" at prompt
             WalkThrough();
            //    ShowStr(GameData.ObsInfo());            
        }

    }
}


