namespace LearnOpenTK.utils
{
    public class Queue
    {
        private List<Action> actions = new List<Action>();

        public List<Action> GetActions() { return actions; }

        public void AddAction(Action job) { actions.Add(job); }

        public void RemoveAction(Action job) { actions.Remove(job); }
    }    
}
