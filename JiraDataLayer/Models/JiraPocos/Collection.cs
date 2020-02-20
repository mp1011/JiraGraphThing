using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDataLayer.Models.JiraPocos
{
    class CollectionAPIModel<T>
    {
        public int maxResults { get; set; }
        public int startAt { get; set; }
        public int total { get; set; }
        public T[] values { get; set; }
    }

    class WorkLogsCollection : CollectionAPIModel<WorkLogV2>
    {
        public WorkLogV2[] worklogs { get; set; }
    }
}
