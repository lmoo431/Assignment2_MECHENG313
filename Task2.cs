using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment2_MECHENG313
{
    class Task2
    {
        static string console_log = ""; // Stores the console output and user input

        // Adds to the console log which will later be written to a text file
        private static void add_to_log(ref string log, string line, bool timestamp)
        {
            if (timestamp)
            {
                log += String.Format("{0} @ Time: {1}\n", line, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff")); // Log with timestamp
            }
            else
            {
                log += String.Format("{0}\n", line); // Log without timestamp
            }
        }

        // Action W
        private static void actionW()
        {
            Console.WriteLine("Action W");
            add_to_log(ref console_log, "Action W", true);
        }

        // Action X
        private static void actionX()
        {
            Console.WriteLine("Action X");
            add_to_log(ref console_log, "Action W", true);
        }

        // Action Y
        private static void actionY()
        {
            Console.WriteLine("Action Y");
            add_to_log(ref console_log, "Action Y", true);
        }

        // Action Z
        private static void actionZ()
        {
            Console.WriteLine("Action Z");
            add_to_log(ref console_log, "Action Z", true);
        }

        // Allows the user to save log file and quit machine
        private static void quit()
        {
            bool saved = false;
            string path = "";
            while (!saved) // While the file hasn't been saved will keep prompting user to enter a valid dir/name
            {
                Console.WriteLine("Please enter the log save file directory and name (e.g. c:\\temp\\log1.txt)");
                path = Console.ReadLine(); // Get the user input for the file dir/name
                try
                {
                    if (!path.EndsWith(".txt")) // Makes sure file being saved is a text file
                    {
                        Console.WriteLine("Error: Invalid filename, please make sure the filename ends with .txt");
                    }
                    else if (!Path.IsPathFullyQualified(path)) // Makes sure path is fully qualified
                    {
                        Console.WriteLine("Error: Path is not fully qualified, please ensure path is fully qualified (e.g. c:\\temp\\log1.txt)");
                    }
                    else
                    {
                        // Try to save the log to the given path, if successful will set saved to true
                        File.WriteAllText(path, console_log);
                        Console.WriteLine("Log has been saved successfully to: {0}", path);
                        saved = true;
                    }
                }
                catch (Exception e) // Catches exception like an invalid directory or file name
                {
                    Console.WriteLine("Error saving file, please ensure save directory and path is valid");
                }
            }
            Environment.Exit(0); // Exit the environment
        }
        
        public static void Main(string[] args)
        {
            // Stores the key corresponding to different events
            Dictionary<char, int> event_to_num = new Dictionary<char, int>();
            event_to_num['a'] = 0;
            event_to_num['b'] = 1;
            event_to_num['c'] = 2;

            // Setup the finite state table with corresponding actions, state 0 to 1, initial state as 0
            var fst = new FiniteStateTable(3, 3, 0);
            fst.SetNextState(0, 0, 1);
            fst.SetActions(0, 0, new Action[] { actionX, actionY });

            // State 1 to 0 triggered by event 0
            fst.SetNextState(1, 0, 0);
            fst.SetActions(1, 0, new Action[] { actionW });

            // State 1 to 2 triggered by event 1
            fst.SetNextState(1, 1, 2);
            fst.SetActions(1, 1, new Action[] { actionX, actionZ });

            // State 2 to 1 triggered by event 2
            fst.SetNextState(2, 2, 1);
            fst.SetActions(2, 2, new Action[] { actionX, actionY });

            // State 2 to 0 triggered by event 0
            fst.SetNextState(2, 0, 0);
            fst.SetActions(2, 0, new Action[] { actionW });

            // Log the initial state
            Console.WriteLine("Starting in State {0}", fst.currentState);
            add_to_log(ref console_log, String.Format("Starting in State {0}", fst.currentState), false);

            while (true)
            {
                // Get user input and log it
                ConsoleKeyInfo cki = Console.ReadKey(true);
                char key_input = Char.ToLower(cki.KeyChar);
                add_to_log(ref console_log, "User input: " + Char.ToString(key_input), true);

                // Quit if the user pressed the quit key q
                if (key_input == 'q')
                {
                    quit();
                }

                // Check if the key has a corresponding event
                else if (event_to_num.ContainsKey(key_input))
                {
                    int event_num = event_to_num[key_input]; // Get event number

                    // Get and complete actions associated with state and event
                    Action[] actions = fst.GetActions(event_num);
                    for (int i = 0; i < actions.Length; i++) { actions[i](); }

                    // If the state has changed then update it in the finite state table and log it
                    if (fst.currentState != fst.GetNextState(event_num))
                    {
                        fst.currentState = fst.GetNextState(event_num);
                        Console.WriteLine("Now in State {0}", fst.currentState);
                        add_to_log(ref console_log, String.Format("Now in State {0}", fst.currentState), false);
                    }
                }
            }
        }
    }
}
