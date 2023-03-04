Hello:))

Wellcome to my first application! 
This is simple time planning app developed at .NET with Entity Framework core. 

To run the app please proceed with one simple step:

Within the project's production directory create the file appsettings.json (ProjectName.Solution/ProjectName/appsettings.json). 

Please insert the following code in appsettings.json, replacing the uid and pwd values with your own username and password for MySQL. 

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;database=[YOUR-DB-NAME];uid=[YOUR-USER-HERE];pwd=[YOUR-PASSWORD-HERE];"
  }
}

Please make following replacements:

[YOUR-USER-HERE] with your username
[YOUR-PASSWORD-HERE] with your password
[YOUR-DB-NAME] with the name of your database.

Best regards, Kristina
