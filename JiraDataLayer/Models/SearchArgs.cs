namespace JiraDataLayer.Models
{
    public class SearchArgs
    {
        public string Project { get; }
        public int Take { get; }
        public string Key { get; }
        public string ParentKey { get; }
        public string Sprint { get; }

        public SearchArgs(string project=null, int take=int.MaxValue, string key=null, string parentKey=null, string sprint=null)
        {
            Project = project;
            Take = take;
            Key = key;
            ParentKey = parentKey;
            Sprint = sprint;
        }
    }
}
