using DbUp;
using DbUp.Engine;
using DbUp.ScriptProviders;
using DbUp.Support;
using System.Reflection;
using System.Text;

const string connectionString = "host=localhost;port=5432;database=dbuptests;username=postgres;password=1234@Test;pooling=true";

EnsureDatabase.For.PostgresqlDatabase(connectionString);

var options = new FileSystemScriptOptions
{
    // true = scan into subdirectories, false = top directory only
    IncludeSubDirectories = true,

    // Patterns to search the file system for. Set to "*.sql" by default.
    Extensions = ["*.sql"],

    // Type of text encoding to use when reading the files. Defaults to "Encoding.UTF8".
    Encoding = Encoding.UTF8,

    // Pass each file path located to this function and filter based on the result
    Filter = path => path.Contains("Database")
};

string externalScripts = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\Scripts"));

var variables = new Dictionary<string, string>
{
    { "UserTableName", "Users" },
};

var upgrader =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsFromFileSystem(externalScripts, options)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script => script.StartsWith("DbUpTests.Scripts.History.") && !script.Contains(".Ignore."))
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script => script.StartsWith("DbUpTests.Scripts.RunAlways."), new SqlScriptOptions { ScriptType = ScriptType.RunAlways })
        .WithVariables(variables)
        .WithTransaction() // Ensure scripts run in a transaction
        .LogToConsole()
        .LogScriptOutput()
        .Build();

var result = upgrader.PerformUpgrade();
if (result.Successful)
{
    Console.WriteLine($"Scripts executed: {result.Scripts.Count()}");
    Console.WriteLine("Execution completed successfully!");
}
else
{
    Console.Error.WriteLine(result.Error);
}