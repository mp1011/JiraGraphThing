using Atlassian.Jira;
using JiraDataLayer.Services.CustomFieldReaders;
using System;
using System.Linq;

namespace JiraDataLayer.Services
{
    public class CustomFieldReader
    {
        private readonly ICustomFieldReader[] _readers;

        public CustomFieldReader(ICustomFieldReader[] readers)
        {
            _readers = readers;
        }

        public T ReadCustomField<T>(Issue issue)
        {
            var reader = _readers.OfType<ICustomFieldReader<T>>().FirstOrDefault();
            if (reader == null)
                return default(T);

            var field = issue.CustomFields.FirstOrDefault(f => f.Name == reader.Field);
            if (field == null)
                return default(T);

            return reader.ReadValue(field);
        }
    }
}
