using game.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace game {
    class GameRunner {
        /* The entry point of a game. Here the Adventure object (the actual game)
         * is created and the main loop executes until the player quits. 
         * Game save/load is also done here.
         */
        Adventure adv; // the Adventure object
        public GameRunner() {
            InitGame();
        }
        private void InitGame() {
            adv = new Adventure();
            StartGame();
        }

        private void StartGame() {
            string input;
            string output = "";
            Console.WriteLine(GameData.Introtext);
            Console.WriteLine(adv.Look());
            Console.WriteLine("Where do you want to go now?");
            Console.WriteLine("Enter a command. To Quit the game, enter Q.");
            // Run main program loop until user enters Q to quit
            do {
                Console.Write("> ");
                input = Console.ReadLine().ToLower();
                output = SaveOrLoad(input);
                if (output == "") {                     // output will only be assigned when "q" or an error msg (e.g. no input given)
                    output = adv.RunCommand(input);
                }
                if (output.Trim() != "") {
                    Console.WriteLine(output.Trim());
                };
            } while (output != "q");
        }

        public string SaveOrLoad(string input) {
            // Test if save or load was entered. 
            string output = "";
            if (input.Trim() == "save") {
                output = SaveGame();
            } else if (input.Trim() == "load") {
                output = LoadGame();
            }
            return output;
        }
        
        public string SaveGame() {
            FileStream fs;
            BinaryFormatter binfmt;
            string filename;
            string msg = "saved";
            Console.Write("Enter filename to save: ");
            filename = Console.ReadLine();           
            try {
                fs = new FileStream(filename, FileMode.Create);
                binfmt = new BinaryFormatter();
                binfmt.Serialize(fs, adv);
                fs.Close();
            } catch (SerializationException e) {
                msg = "Save (serialization) failed: " + e.Message;
            } catch (Exception e2) {
                msg = "Save failed: " + e2.Message;
            } 
            return msg;
        }

        public string LoadGame() {
            FileStream fs;
            BinaryFormatter binfmt;
            string filename;
            string msg = "loaded";
            Console.Write("Enter filename to load: ");
            filename = Console.ReadLine();                  
            try {
                fs = new FileStream(filename, FileMode.Open);
                binfmt = new BinaryFormatter();
                adv = (Adventure)binfmt.Deserialize(fs);
                fs.Close();
            } catch (SerializationException e) {
                msg = "Load (serialization) failed: " + e.Message;
            } catch (Exception e2) {
                msg = "Load failed: " + e2.Message;
            } 
            return msg;
        }
    }
}
