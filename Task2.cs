namespace Assignment2_MECHENG313
{
    class Task2
    {

        public static void actionW()
        {
            Console.WriteLine("Action W");
        }

        public static void actionX()
        {
            Console.WriteLine("Action X");
        }

        public static void actionY()
        {
            Console.WriteLine("Action Y");
        }

        public static void actionZ()
        {
            Console.WriteLine("Action Z");
        }

        public static void quit()
        {   
            Console.WriteLine("Please enter the log save file directory and name (e.g. c:\\temp\\log1.txt");
            string dir_input = Console.ReadLine();
            Console.WriteLine("Output has been successfully logged to ...");
            Environment.Exit(0);
        }

        public static void Main(string[] args)
        {
            Dictionary<char, int> event_to_num = new Dictionary<char, int>();
            event_to_num['a'] = 0;
            event_to_num['b'] = 1;
            event_to_num['c'] = 2;
            event_to_num['q'] = 3;

            var fst = new FiniteStateTable(3, 4);
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

            fst.SetActions(0, 3, new Action[] { quit });
            fst.SetActions(1, 3, new Action[] { quit });
            fst.SetActions(2, 3, new Action[] { quit });

            int state = 0;
            Console.WriteLine("State: {0}", state);
            while (true)
            {
                string event_input = Console.ReadLine();
                if (event_input.Length == 1 && event_to_num.ContainsKey(event_input[0]))
                {
                    int event_num = event_to_num[event_input[0]];
                    Action[] actions = fst.GetActions(state, event_num);
                    for (int i = 0; i < actions.Length; i++){actions[i]();}
                    if (state != fst.GetNextState(state, event_num))
                    {
                        state = fst.GetNextState(state, event_num);
                        Console.WriteLine("State: {0}", state);
                    }
                }
            }
        }
    }
}
