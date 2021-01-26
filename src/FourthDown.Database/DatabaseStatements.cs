namespace FourthDown.Database
{
    public class DatabaseStatements
    {
        public static string GetAllChangeScripts = @"
SELECT 
[ChangeScriptId]
,[ChangeScriptName]
,[ChangeScriptDeployStart]
,[ChangeScriptDeployEnd]
,[ChangeScriptDeployDuration]
,[ChangeScriptDeploySuccess]
FROM [dbo].[ChangeScripts]";
    }
}