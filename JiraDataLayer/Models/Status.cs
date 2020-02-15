namespace JiraDataLayer.Models
{
    public class Status
    {
        public string Name { get; }
        public bool IsComplete { get; }

        public Status(string name, bool isComplete)
        {
            Name = name;
            IsComplete = isComplete;
        }
    }
}
