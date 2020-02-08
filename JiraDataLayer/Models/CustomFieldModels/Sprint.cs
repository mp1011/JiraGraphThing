using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDataLayer.Models.CustomFieldModels
{
    class Sprint
    {
        public string Name { get; }

        public Sprint(string name)
        {
            Name = name;
        }
    }
}
