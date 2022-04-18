using Atlassian.Jira;
using CSharpFunctionalExtensions;
using Impulse.Shared.Services;
using System.Security.Authentication;

namespace Impulse.Jira;

public class JiraApiService : IJiraApiService
{
    // "https://adl-jira.maptek.com.au/"
    public async Task<Result<List<string>>> GetAllReadyForDemoJiraIssuesForEmployee(
        string jiraEndpoint,
        string userName,
        string password,
        string employee)
    {
        // create a connection to JIRA using the Rest client
        var jira = Atlassian.Jira.Jira.CreateRestClient(
            jiraEndpoint,
            userName,
            password);

        var options = new IssueSearchOptions($"status = \"Ready for Demo\" AND assignee in ({employee})")
        {
            MaxIssuesPerRequest = 2000
        };

        IPagedQueryResult<Issue> jqlIssues = null;
        var jobLookup = new Dictionary<string, JiraJob>();

        try
        {
            jqlIssues = await jira.Issues.GetIssuesFromJqlAsync(options);
        }
        catch (InvalidOperationException)
        {
            return Result.Failure<List<string>>($"The endpoint '{jiraEndpoint}' cannot be connected to and may be incorrect.");
        }
        catch (AuthenticationException)
        {
            return Result.Failure<List<string>>("The username or password provided are invalid.");
        }

        return jqlIssues.Select(j => j.Summary).ToList();
    }
}
