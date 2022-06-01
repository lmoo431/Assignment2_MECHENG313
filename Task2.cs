namespace Assignment2_MECHENG313
{
    class Task2
    {
        static string console_log = "";

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

        private static void actionW()
        {
            Console.WriteLine("Action W");
            add_to_log(ref console_log, "Action W", true);
        }

        private static void actionX()
        {
            Console.WriteLine("Action X");
            add_to_log(ref console_log, "Action W", true);
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

            var fst = new FiniteStateTable(3, 3, 0);
            fst.SetNextState(0, 0, 1);
            fst.SetActions(0, 0, new Action[] { actionX, actionY });

            fst.SetNextState(1, 0, 0);
            fst.SetActions(1, 0, new Action[] { actionW });

            fst.SetNextState(1, 1, 2);
            fst.SetActions(1, 1, new Action[] { actionX, actionZ });

            fst.SetNextState(2, 2, 1);
            fst.SetActions(2, 2, new Action[] { actionX, actionY });

            fst.SetNextState(2, 0, 0);
            fst.SetActions(2, 0, new Action[] { actionW });

            Console.WriteLine("Now in State {0}", fst.currentState);
            add_to_log(ref console_log, String.Format("Now in State {0}", fst.currentState), false);

            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                char key_input = Char.ToLower(cki.KeyChar);
                add_to_log(ref console_log, "User input: " + Char.ToString(key_input), true);
                if (key_input == 'q')
                {
                    quit();
                }
                else if (event_to_num.ContainsKey(key_input))
                {
                    int event_num = event_to_num[key_input];
                    Action[] actions = fst.GetActions(fst.currentState, event_num);
                    for (int i = 0; i < actions.Length; i++) { actions[i](); }
                    if (fst.currentState != fst.GetNextState(fst.currentState, event_num))
                    {
                        fst.currentState = fst.GetNextState(fst.currentState, event_num);
                        Console.WriteLine("State: {0}", fst.currentState);
                        add_to_log(ref console_log, String.Format("Now in State {0}", fst.currentState), false);
                    }
                }
        
            }
        }
        
    }
}

