using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment2_MECHENG313
{

    class Task3
    {

        static string console_log = ""; // Stores the console output and user input

        private static object lock_log = new object();

        // Adds to the console log which will later be written to a text file
        private static void add_to_log(ref string log, string line, bool timestamp)
        {
            // Lock the contents of the add_to_log function so that only one thread can access it at a time
            lock (lock_log)
            {
                if (timestamp)
                {
                    log += String.Format("{0} @ Time: {1}\n", line, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
                }
                else
                {
                    log += String.Format("{0}\n", line);
                }
            }
        }

        // Action J
        private static void actionJ()
        {
            Console.WriteLine("Action J");
            add_to_log(ref console_log, "Action J", true);    
        }

        // Action K
        private static void actionK()
        { 
            Console.WriteLine("Action K");
            add_to_log(ref console_log, "Action K", true);             
        }

        // Action L
        private static void actionL()
        {
            Console.WriteLine("Action L");
            add_to_log(ref console_log, "Action L", true);
        }

        private static void actionW()
        {
            Console.WriteLine("Action W");
            add_to_log(ref console_log, "Action W", true);
        }

        // Action X
        private static void actionX()
        {
            Console.WriteLine("Action X");
            add_to_log(ref console_log, "Action X", true);
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
                catch (DirectoryNotFoundException e)
                {
                    // Catches error where directory is valid but doens't exist already
                    Console.WriteLine("Error: This directory doesn't exist");
                }
                catch // Catches exception like an invalid directory or file name
                {
                    Console.WriteLine("Error saving file, please ensure save directory and name is valid");
                }
            }
            Environment.Exit(0); // Exit the environment
        }

        public static void RunTask()
        {
            // Stores the key corresponding to different events
            Dictionary<char, int> event_to_num = new Dictionary<char, int>();
            event_to_num['a'] = 0;
            event_to_num['b'] = 1;
            event_to_num['c'] = 2;

            // Create and populate the finite state table from Task 2 
            var fstA = new FiniteStateTable(3, 3, 0); // Start in state S0

            // State 0 to 1 triggered by event 0
            fstA.SetNextState(0, 0, 1);
            fstA.SetActions(0, 0, new Action[] { actionX, actionY });

            // State 1 to 0 triggered by event 0
            fstA.SetNextState(1, 0, 0);
            fstA.SetActions(1, 0, new Action[] { actionW });

            // State 1 to 2 triggered by event 1
            fstA.SetNextState(1, 1, 2);
            fstA.SetActions(1, 1, new Action[] { actionX, actionZ });

            // State 2 to 1 triggered by event 2
            fstA.SetNextState(2, 2, 1);
            fstA.SetActions(2, 2, new Action[] { actionX, actionY });

            // State 2 to 0 triggered by event 0
            fstA.SetNextState(2, 0, 0);
            fstA.SetActions(2, 0, new Action[] { actionW });

            // Create and populate the new finite state table for Task 3
            var fstB = new FiniteStateTable(2, 3, 1); // Start in state SB

            // State 0 to 1 triggered by event 0
            fstB.SetNextState(0, 0, 1);

            // State 1 to 0 triggered by event 0, 1 or 2
            // The following state changes only happen when we are in state S1 in fstA
            fstB.SetNextState(1, 0, 0);
            fstB.SetNextState(1, 1, 0);
            fstB.SetNextState(1, 2, 0);

            // Set up actions for fstB state changes
            fstB.SetActions(1, 0, new Action[] { actionJ, actionK, actionL });
            fstB.SetActions(1, 1, new Action[] { actionJ, actionK, actionL });
            fstB.SetActions(1, 2, new Action[] { actionJ, actionK, actionL });

            // Set up dependencies for fstB (as it is concurrent & dependent on fstA)
            fstB.SetDependencies(1, 0, fstA, 1);
            fstB.SetDependencies(1, 1, fstA, 1);
            fstB.SetDependencies(1, 2, fstA, 1);            

            // Output initial states to the user and logs state
            Console.WriteLine("Starting in State {0}-{1}", fstA.currentState, (char)(fstB.currentState + 'A'));
            add_to_log(ref console_log, String.Format("Starting in State {0}-{1}", fstA.currentState, (char)(fstB.currentState + 'A')), false);
            while (true)
            {

                // Get user input and log it
                ConsoleKeyInfo cki;
                cki = Console.ReadKey(true);
                char key_input = char.ToLower(cki.KeyChar);
                add_to_log(ref console_log, "User input: " + Char.ToString(key_input), true);

                // Quit if the user pressed the quit key q
                if (key_input == 'q')
                {
                    quit();
                }

                // Check if the key has a corresponding event
                else if (event_to_num.ContainsKey(key_input))
                {
                    int event_num = event_to_num[char.ToLower(cki.KeyChar)];

                    // Determine what actions need to be taken as a result of input event
                    Action[] actionsA = fstA.GetActions(event_num);
                    Action[] actionsB = fstB.GetActions(event_num);
                    Thread[] threads = new Thread[actionsB.Length];

                    // Carry out all required actions
                    for (int i = 0; i < actionsA.Length; i++) { actionsA[i](); }
                    for (int i = 0; i < actionsB.Length; i++)
                    {
                        // Add each action for fstB to a new thread to be executed
                        Action a = actionsB[i];
                        threads[i] = new Thread(() => a());

                    }

                    // Start all threads to run all actions from fstB on different threads, then join them 
                    for (int i = 0; i < actionsB.Length; i++) { threads[i].Start(); }
                    for (int i = 0; i < actionsB.Length; i++) { threads[i].Join(); }
                


                    // Prepare to change state - important to not update the actual FST until all FST state changes are resolved, to prevent issues with FST states changing before the next
                    // change is sorted out
                    int newStateA = fstA.currentState;
                    int newStateB = fstB.currentState;
                    
                    // If the current event is associated with a state change from the current state, change the state and inform the user of the new state through console output
                    if (fstA.currentState != fstA.GetNextState(event_num) || fstB.currentState != fstB.GetNextState(event_num))
                    {
                        newStateA = fstA.GetNextState(event_num);
                        newStateB = fstB.GetNextState(event_num);

                        // Let user know how state has changed, and log this change
                        Console.WriteLine("Now in State {0}-{1}", newStateA, (char)(newStateB + 'A'));
                        add_to_log(ref console_log, String.Format("Now in State {0}-{1}", newStateA, (char)(newStateB + 'A')), false);
                    }

                    // Only update the current state of both FSTs once all state changes are resolved to prevent errors due to one state changing before another.
                    fstA.currentState = newStateA;
                    fstB.currentState = newStateB;

                }
        
            }
        }
    }
}
