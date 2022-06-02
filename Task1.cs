using System;

namespace Assignment2_MECHENG313
{

    // This class is used to create a finite state table which can be implemented to create a finite state machine
    class FiniteStateTable
    {
        public int currentState; // Current state of the FST

        // Cell information associated with each state and event combination
        private struct cell_FST
        {
            public int nextState;
            public Action[] actions;
            public int dependent_state;
            public FiniteStateTable dependent_FST;
        }
        private cell_FST[,] FST; // 2D FST

        // Initialise the FST with a known number of states, events, and starting state
        public FiniteStateTable(int num_states, int num_events, int init_state)
        {
            this.FST = new cell_FST[num_states, num_events];

            // Set default values for each cell in the FST
            for (int i = 0; i < num_states; i++)
            {
                for (int j = 0; j < num_events; j++)
                {
                    this.FST[i, j].nextState = i;
                    this.FST[i, j].actions = Array.Empty<Action>();
                    this.FST[i, j].dependent_state = -1; // A value of -1 for the dependent state means this cell is not dependent on another FST
                    this.FST[i, j].dependent_FST = null; // A null value for this means there is no dependent FST
                }
            }
            this.currentState = init_state; // Set the current state as the initial state
        }

        // Sets the next state for a given state and event
        public void SetNextState(int state_num, int event_num, int next_state_num)
        {
            this.FST[state_num, event_num].nextState = next_state_num;
        }

        // Sets the actions for a given state and event
        public void SetActions(int state_num, int event_num, Action[] actions)
        {
            this.FST[state_num, event_num].actions = actions;
        }

        // Sets any dependencies for a given state and event. If no dependencies are set then the FST is independent by default
        public void SetDependencies(int state_num, int event_num, FiniteStateTable dependent_fst, int dependent_state_num)
        {
            this.FST[state_num, event_num].dependent_state = dependent_state_num;
            this.FST[state_num, event_num].dependent_FST = dependent_fst;
        }

        // Gets the next state from the current state and an event
        public int GetNextState(int event_num)
        {
            // Check for dependency; if the FST state+event is dependent on the state of another FST, check the state of that FST and proceed accordingly
            if (this.FST[this.currentState, event_num].dependent_state != -1)
            {
                // Check that the dependent FST is in the correct state
                if (this.FST[this.currentState, event_num].dependent_FST.currentState == this.FST[this.currentState, event_num].dependent_state)
                {
                    return this.FST[this.currentState, event_num].nextState;
                }
                else
                {
                    // If not then return the current state, i.e. no state change
                    return this.currentState;
                }
            }
            else
            {
                // If the FST cell is independent then just return the next state
                return this.FST[this.currentState, event_num].nextState;
            }
        }

        // Get the actions for the current state and event
        public Action[] GetActions(int event_num)
        {
            // Check for dependency; if the FST state+event is dependent on the state of another FST, check the state of that FST and proceed accordingly
            if (this.FST[this.currentState, event_num].dependent_state != -1)
            {
                // Check that the dependent FST is in the correct state
                if (this.FST[this.currentState, event_num].dependent_FST.currentState == this.FST[this.currentState, event_num].dependent_state)
                {
                    return this.FST[this.currentState, event_num].actions;
                }
                else
                {
                    // If not then return an empty set of actions, i.e. do nothing
                    return Array.Empty<Action>();
                }
            }
            else 
            { 
                // If the FST cell is independent then just return the set of actions relating to it
                return this.FST[this.currentState, event_num].actions;
            }
        }
    }

}
