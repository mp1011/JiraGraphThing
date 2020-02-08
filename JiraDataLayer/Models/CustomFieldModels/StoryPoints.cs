using System;
using System.Collections.Generic;
using System.Text;

namespace JiraDataLayer.Models.CustomFieldModels
{
    class StoryPoints
    {
        public decimal Value { get; }

        public StoryPoints(decimal value)
        {
            Value = value;
        }
    }
}
