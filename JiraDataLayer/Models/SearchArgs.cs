namespace JiraDataLayer.Models
{
    public class SearchArgs
    {
        public string Project { get; }
        public int Take { get; }
        public string Key { get; }
        public string ParentKey { get; }

        public SearchArgs(string project=null, int take=10, string key=null, string parentKey=null)
        {
            Project = project;
            Take = take;
            Key = key;
            ParentKey = parentKey; 
        }
    }
}
