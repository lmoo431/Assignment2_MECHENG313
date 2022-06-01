using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment2_MECHENG313
{

    class Task3
    {

        static string console_log = "";

        private static object lock_log = new object();

        private static void add_to_log(ref string log, string line, bool timestamp)
        {
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

        private static void actionJ()
        {
            // This method will run on a separate thread until the program ends

                
                    Console.WriteLine("Action J");
                    add_to_log(ref console_log, "Action J", true);
                 

        }

        private static void actionK()
        {
            // This method will run on a separate thread until the program ends
            
                    Console.WriteLine("Action K");
                    add_to_log(ref console_log, "Action K", true);
                   
              
        }

        private static void actionL()
        {
            // This method will run on a separate thread until the program ends
           
                    Console.WriteLine("Action L");
                    add_to_log(ref console_log, "Action L", true);
                    
           
        }

        private static void actionW()
        {
            Console.WriteLine("Action W");
            add_to_log(ref console_log, "Action W", true);
        }

        private static void actionX()
        {
            Console.WriteLine("Action X");
            add_to_log(ref console_log, "Action X", true);
        }

        private static void actionY()
        {
            Console.WriteLine("Action Y");
            add_to_log(ref console_log, "Action Y", true);
        }

        private static void actionZ()
        {
            Console.WriteLine("Action Z");
            add_to_log(ref console_log, "Action Z", true);
        }

        private static void quit()
        {
            bool saved = false;
            string path = "";
            while (!saved)
            {
                Console.WriteLine("Please enter the log save file directory and name (e.g. c:\\temp\\log1.txt)");
                path = Console.ReadLine();
                try
                {
                    if (!path.EndsWith(".txt"))
                    {
                        Console.WriteLine("Error: Invalid filename, please make sure the filename ends with .txt");
                    }
                    else if (!Path.IsPathFullyQualified(path))
                    {
                        Console.WriteLine("Error: Path is not fully qualified, please ensure path is fully qualified (e.g. c:\\temp\\log1.txt)");
                    }
                    else
                    {
                        File.WriteAllText(path, console_log);
                        Console.WriteLine("Log has been saved successfully to: {0}", path);
                        saved = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error saving file, please ensure save directory and path is valid");
                }
            }
            Environment.Exit(0);
        }

        public static void Main(string[] args)
        {
            // Set up a dictionary to map user actions to numbers for ease of implementation later on
            Dictionary<char, int> event_to_num = new Dictionary<char, int>();
            event_to_num['a'] = 0;
            event_to_num['b'] = 1;
            event_to_num['c'] = 2;

            // Create and populate the finite state table from Task 2
            var fstA = new FiniteStateTable(3, 3, 1);
            fstA.SetNextState(0, 0, 1);
            fstA.SetActions(0, 0, new Action[] { actionX, actionY });

            fstA.SetNextState(1, 0, 0);
            fstA.SetActions(1, 0, new Action[] { actionW });

            fstA.SetNextState(1, 1, 2);
            fstA.SetActions(1, 1, new Action[] { actionX, actionZ });

            fstA.SetNextState(2, 2, 1);
            fstA.SetActions(2, 2, new Action[] { actionX, actionY });

            fstA.SetNextState(2, 0, 0);
            fstA.SetActions(2, 0, new Action[] { actionW });

            // Create and populate the new finite state table for Task 3
            var fstB = new FiniteStateTable(2, 3, 1);

            fstB.SetNextState(0, 0, 1);

            // The following state changes only happen when we are in state S1 in fstA
            fstB.SetNextState(1, 0, 0);
            fstB.SetNextState(1, 1, 0);
            fstB.SetNextState(1, 2, 0);

            // Set up actions to be dependent on fstA being in state 1
            fstB.SetActions(1, 0, new Action[] { actionJ, actionK, actionL }, fstA, 1);
            fstB.SetActions(1, 1, new Action[] { actionJ, actionK, actionL }, fstA, 1);
            fstB.SetActions(1, 2, new Action[] { actionJ, actionK, actionL }, fstA, 1);

            // Create three separate threads to carry out actions J, K and L and start them. This will begin an
            // infinite loop that constantly checks if these actions have been triggered to occur, and triggers
            // them when that is the case. 
            

            // Output initial states to the user
            Console.WriteLine("Starting in State {0}", fstA.currentState);
            Console.WriteLine("Starting in State {0}", (char)(fstB.currentState + 'A'));
            while (true)
            {

                // Constantly read user keyboard input, and if it matches with one of our events (or is 'q' for quit), perform the required action
                ConsoleKeyInfo cki;
                cki = Console.ReadKey(true);
                char key_input = char.ToLower(cki.KeyChar);
                add_to_log(ref console_log, "User input: " + Char.ToString(key_input), true);
                if (key_input == 'q')
                {
                    quit();
                }
                else if (event_to_num.ContainsKey(key_input))
                {
                    int event_num = event_to_num[char.ToLower(cki.KeyChar)];

                    // Determine what actions need to be taken as a result of input event
                    Action[] actionsA = fstA.GetActions(fstA.currentState, event_num);
                    Action[] actionsB = fstB.GetActions(fstB.currentState, event_num);
                    Thread[] threads = new Thread[actionsB.Length];

                    // Carry out all required actions
                    for (int i = 0; i < actionsA.Length; i++) { actionsA[i](); }
                    for (int i = 0; i < actionsB.Length; i++)
                    {

                        Action a = actionsB[i];
                        threads[i] = new Thread(() => a());

                    }

                    for (int i = 0; i < actionsB.Length; i++)
                    {
                        threads[i].Start();
                    }
                    for (int i = 0; i < actionsB.Length; i++) { threads[i].Join(); }
                


                    // Prepare to change state - important to not update the actual FST until all FST state changes are resolved, to prevent issues with FST states changing before the next
                    // change is sorted out
                    int newStateA = fstA.currentState;
                    int newStateB = fstB.currentState;
                    
                    // If the current event is associated with a state change from the current state, change the state and inform the user of the new state through console output
                    if (fstA.currentState != fstA.GetNextState(fstA.currentState, event_num))
                    {
                        newStateA = fstA.GetNextState(fstA.currentState, event_num);
                        Console.WriteLine("Now in State {0}", newStateA);
                    }

                    // Now do the same for the other FST
                    if (fstB.currentState != fstB.GetNextState(fstB.currentState, event_num))
                    {
                        newStateB = fstB.GetNextState(fstB.currentState, event_num);
                        Console.WriteLine("Now in State {0}", (char)(newStateB + 'A'));
                    }

                    // Only update the current state of both FSTs once all state changes are resolved to prevent errors due to one state changing before another.
                    fstA.currentState = newStateA;
                    fstB.currentState = newStateB;

                }
            }
        }
    }
}
