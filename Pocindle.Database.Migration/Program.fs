open System.Reflection

open DbUp

[<EntryPoint>]
let main argv =
    let connectionString = argv.[0]

    let upgrader =
        DeployChanges
            .To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build()

    let result = upgrader.PerformUpgrade()

    if result.Successful then 0 else 1
