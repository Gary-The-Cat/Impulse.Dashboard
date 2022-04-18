using Atlassian.Jira;
using Impulse.Shared.ExtensionMethods;

namespace Impulse.Jira;

public class JiraJob
{
    public JiraJob(Issue issue)
    {
        JobId = issue.Key.Value;
        BuildId = issue.CustomFields.FirstOrDefault(f => f.Name.Equals("Build ID"))?.Values?.FirstOrDefault();

        var repository = issue.CustomFields.FirstOrDefault(f => f.Name.Equals("Repository"))?.Values;

        if (repository != null && repository.Length > 1)
        {
            Repository = repository[0].FirstCharToUpper();
            Branch = repository[1];
        }
    }

    public string JobId { get; set; }

    public string BuildId { get; set; }

    public string Repository { get; set; }

    public string Branch { get; set; }
}
