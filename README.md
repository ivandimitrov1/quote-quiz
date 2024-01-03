# Quote quiz application
- an application where users can create quize and take quizes (something like questions/answers)
- a quiz contains quote or many quotes with possible answers
- a user can create multiple quizes
- a quote could be Yes or No or a quote with multiple choice quote
- a user can take multiple quizes and can take a quiz more than once
- admin can review all user attemps, a user can see only their results

# How to run
## start with the database
- you will need Sql Server
- then you can download the backup database from backups folder and restore it
- adjust the database user access, see the appsettings.json files in the solution

Or you can apply the migrations:
- create manually a database with name "frt_quote_quiz"
- make sure that the user access, see the appsettings.json files
- open PowerShell with current folder: QuoteQuiz.Api and execute "dotnet ef database update"

And you can create migration scripts by open solution folder and execute in PowerShell 
- example how to add a migration script with name "Init"

dotnet ef --startup-project QuoteQuiz.Api/QuoteQuiz.Api.csproj migrations add Init --output-dir Migrations --project QuoteQuiz.Infrastructure/QuoteQuiz.Infrastructure.csproj --verbose

## open the BE solution
- you will need Visual Studio 2022
- buid and start, that should launch a server on https://localhost:7197 ,
and you should see a swagger UI with the endpoints

## FE part
- you will need Nodejs, then go to frontend folder
- run "npm install", then run "npm start"
- that should launch a server on https://localhost:3000

Thats all. If you use the backup, then you have two users created already
- user with pass: user
- admin with pass: admin 
  
 
