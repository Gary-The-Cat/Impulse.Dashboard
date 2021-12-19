namespace Impulse.TestGenerators
{
    public static class JiraProjectIdGenerator
    {
        public static string CreateFromName(string projectName)
        {
            return (projectName.Length > 2)
                ? projectName.Substring(0, 3).ToUpper()
                : projectName.ToUpper();
        }
    }
}
