using System.Threading;

namespace Assignment2_MECHENG313
{
    class Task3
    {

        static string console_log = "";

        static bool jIsTriggered = false;
        static bool kIsTriggered = false;
        static bool lIsTriggered = false;

        private static void add_to_log(ref string log, string line, bool timestamp)
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

        private static void actionJ()
        {
            while (true)
            {
                if (jIsTriggered)
                {
                    Console.WriteLine("Action J");
                    add_to_log(ref console_log, "Action J", true);
                    jIsTriggered = false;
                }
            }
            
        }

        private static void actionK()
        {
            while (true)
            {
                if (kIsTriggered)
                {
                    Console.WriteLine("Action K");
                    add_to_log(ref console_log, "Action K", true);
                    kIsTriggered = false;
                }
            }
        }

        private static void actionL()
        {
            while (true)
            {
                if (lIsTriggered)
                {
                    Console.WriteLine("Action L");
                    add_to_log(ref console_log, "Action L", true);
                    lIsTriggered = false;
                }
            }
        }

        private static void triggerJ()
        {
            jIsTriggered = true;
        }

        private static void triggerK()
        {
            kIsTriggered = true;
        }

        private static void triggerL()
        {
            lIsTriggered = true;
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
            Dictionary<char, int> event_to_num = new Dictionary<char, int>();
            event_to_num['a'] = 0;
            event_to_num['b'] = 1;
            event_to_num['c'] = 2;

            var fstA = new FiniteStateTable(3, 3, 0);
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


            var fstB = new FiniteStateTable(2, 3, 1);

            fstB.SetNextState(0, 0, 1);

            // Following are only true when we are in state SB (stateB == 1)
            fstB.SetNextState(1, 0, 0);
            fstB.SetNextState(1, 1, 0);
            fstB.SetNextState(1, 2, 0);

            var thrJ = new Thread(actionJ);
            var thrK = new Thread(actionK);
            var thrL = new Thread(actionL);

            thrJ.Start();
            thrK.Start();
            thrL.Start();

            fstB.SetActions(1, 0, new Action[] { triggerJ, triggerK, triggerL }, fstA, 1);
            fstB.SetActions(1, 1, new Action[] { triggerJ, triggerK, triggerL }, fstA, 1);
            fstB.SetActions(1, 2, new Action[] { triggerJ, triggerK, triggerL }, fstA, 1);

            
           
            Console.WriteLine("State: {0}", fstA.currentState);
            Console.WriteLine("State: {0}", (char)(fstB.currentState + 'A'));
            while (true)
            {
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
                    Action[] actionsA = fstA.GetActions(fstA.currentState, event_num);
                    Action[] actionsB = fstB.GetActions(fstB.currentState, event_num);
                    for (int i = 0; i < actionsA.Length; i++) { actionsA[i](); }
                    for (int i = 0; i < actionsB.Length; i++) { actionsB[i](); }
                    int newStateA = fstA.currentState;
                    int newStateB = fstB.currentState;
                    if (fstA.currentState != fstA.GetNextState(fstA.currentState, event_num))
                    {
                        newStateA = fstA.GetNextState(fstA.currentState, event_num);
                        Console.WriteLine("Now in State {0}", newStateA);
                    }
                    if(fstB.currentState != fstB.GetNextState(fstB.currentState, event_num))
                    {
                        newStateB = fstB.GetNextState(fstB.currentState, event_num);
                        Console.WriteLine("Now in State {0}", (char)(newStateB + 'A'));
                    }
                    fstA.currentState = newStateA;
                    fstB.currentState = newStateB; 
                    
                }
            }
        }
      
    }
}
