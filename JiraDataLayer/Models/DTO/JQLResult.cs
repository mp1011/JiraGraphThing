using Dapper.Contrib.Extensions;

namespace JiraDataLayer.Models.DTO
{
    [Table("JQLSearch")]
    class JQLSearchDTO : IDTO
    {
        public int ROWID { get; set; }

        public string JQL { get; set; }
        public int Take { get; set; }
    }

    [Table("JQLSearchItem")]
    class JQLSearchItemDTO : IDTO
    {
        public int SearchID { get; set; }
        public string IssueKey { get; set; }
    }
}
