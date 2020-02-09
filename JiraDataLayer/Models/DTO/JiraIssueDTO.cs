﻿using Dapper.Contrib.Extensions;

namespace JiraDataLayer.Models.DTO
{
    [Table("JiraIssue")]
    public class JiraIssueDTO : IDTO
    {
        public string Key { get; set; }

        public string EpicKey { get; set; }

        public string ParentKey { get; set; }

        public string Project { get; set; }

        public string TypeName { get; set; }

        public string Sprint { get; set; }

        public decimal StoryPoints { get; set; }
    }
}