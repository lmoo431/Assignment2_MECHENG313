namespace Assignment2_MECHENG313 {
    class FiniteStateTable
    {
        private struct cell_FST
        {
            public int nextState;
            public Action[] actions;
        }
        private cell_FST[,] FST;
        private static void do_nothing() { }

        public FiniteStateTable(int num_states, int num_events)
        {
            this.FST = new cell_FST[num_states, num_events];
            for (int i = 0; i < num_states; i++)
            {
                for (int j = 0; j < num_events; j++)
                {
                    this.FST[i, j].nextState = i;
                    this.FST[i, j].actions = new Action[] { do_nothing };
                }
            }

        }

        public void SetNextState(int state, int event_num, int next_state_num)
        {
            this.FST[state, event_num].nextState = next_state_num;
        }

        public void SetActions(int state_num, int event_num, Action[] actions)
        {
            this.FST[state_num, event_num].actions = actions;
        }

        public int GetNextState(int state, int event_num)
        {
            return this.FST[state, event_num].nextState;
        }

        public Action[] GetActions(int state, int event_num)
        {
            return this.FST[state, event_num].actions;
        }
    }

}
