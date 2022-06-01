namespace Assignment2_MECHENG313
{
    class FiniteStateTable
{
        public int currentState;
    private struct cell_FST
    {
        public int nextState;
        public Action[] actions;
        public int dependent_state;
        public FiniteStateTable dependent_FST;
    }
    private cell_FST[,] FST;
    private static void do_nothing() { }

    public FiniteStateTable(int num_states, int num_events, int init_state)
    {
        this.FST = new cell_FST[num_states, num_events];
        for (int i = 0; i < num_states; i++)
        {
            for (int j = 0; j < num_events; j++)
            {
                this.FST[i, j].nextState = i;
                this.FST[i, j].actions = new Action[] { do_nothing };
                this.FST[i, j].dependent_state = -1;
                this.FST[i, j].dependent_FST = null;
            }
        }
        this.currentState = init_state; 
    }

    public void SetNextState(int state, int event_num, int next_state_num)
    {
        this.FST[state, event_num].nextState = next_state_num;
    }


    public void SetActions(int state_num, int event_num, Action[] actions)
    {
        this.FST[state_num, event_num].actions = actions;
    }

    public void SetActions(int state_num, int event_num, Action[] actions, FiniteStateTable dependent_fst, int dependent_state_num)
    {
            this.FST[state_num, event_num].actions = actions;
            this.FST[state_num, event_num].dependent_state = dependent_state_num;
            this.FST[state_num, event_num].dependent_FST = dependent_fst;
    }
    public int GetNextState(int state, int event_num)
    {
            // Check for dependency; if the FST state+event is dependent on the state of another FST, check the state of that FST and proceed accordingly
            if (this.FST[state, event_num].dependent_state != -1)
            {
                if (this.FST[state, event_num].dependent_FST.currentState == this.FST[state, event_num].dependent_state)
                {
                    return this.FST[state, event_num].nextState;
                } else
                {
                    return state;
                }
            } else
            {
                return this.FST[state, event_num].nextState;
            }
    }

    public Action[] GetActions(int state, int event_num)
    {
        // Check for dependency; if the FST state+event is dependent on the state of another FST, check the state of that FST and proceed accordingly
        if (this.FST[state, event_num].dependent_state != -1)
        {
            if (this.FST[state, event_num].dependent_FST.currentState == this.FST[state, event_num].dependent_state)
                {
                    return this.FST[state, event_num].actions;
                }
                return new Action[] { do_nothing };
        } 
        else { return this.FST[state, event_num].actions; } 
    }
}

}
