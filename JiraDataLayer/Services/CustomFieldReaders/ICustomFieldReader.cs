using Atlassian.Jira;

namespace JiraDataLayer.Services.CustomFieldReaders
{
    public interface ICustomFieldReader
    {
        string Field { get; }
    }

    interface ICustomFieldReader<T> : ICustomFieldReader
    {
        T ReadValue(CustomFieldValue cf);
    }
}
