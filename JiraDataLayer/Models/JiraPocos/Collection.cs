using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDataLayer.Models.JiraPocos
{
    class CollectionAPIModel<T>
    {
        public int maxResults { get; set; }
        public int startAt { get; set; }
        public T[] values { get; set; }
    }
}
