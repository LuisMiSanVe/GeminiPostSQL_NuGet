> [See in spanish/Ver en español](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/README.es.md)

# GeminiPostSQL NuGet | AI-Assisted NuGet Package for PostgreSQL
[![image](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://dotnet.microsoft.com/en-us/languages/csharp)
[![image](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet)
[![image](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![image](https://img.shields.io/badge/json-5E5C5C?style=for-the-badge&logo=json&logoColor=white)](https://www.newtonsoft.com/json)
[![image](https://img.shields.io/badge/Google%20Gemini-8E75B2?style=for-the-badge&logo=googlegemini&logoColor=white)](https://aistudio.google.com/app/apikey)
[![image](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual%20studio&logoColor=white)](https://visualstudio.microsoft.com/)
[![NuGet](https://img.shields.io/badge/NuGet-%23004880.svg?style=for-the-badge&logo=nuget&logoColor=white)](https://www.nuget.org/)

>[!NOTE]
> This is the NuGet Package version meant for developer use. There is a [WinForms](https://github.com/LuisMiSanVe/GeminiPostSQL/tree/main) meant for client use, a [REST API](https://github.com/LuisMiSanVe/GeminiPostSQL_API/tree/main) version meant for servers with Swagger and a [ChatBot](https://github.com/LuisMiSanVe/GeminiPostSQL_ChatBot/tree/main) in Blazor for web clients.

This NuGet package uses Google's AI 'Gemini 2.0 Flash' to make queries to PostgreSQL databases.  
The AI interprets natural language into SQL queries using one method, with its pros and cons.

## 📋 Prerequisites
To make this program work, you'll need a PostgreSQL Server and a Gemini API Key.

> [!NOTE]  
> I will be using pgAdmin to build the PostgreSQL Server.

## 🛠️ Setup
If you don't have it, download [pgAdmin 4 from the official website](https://www.pgadmin.org/download/).  
Now, create the PostgreSQL Server and set up a database with a few tables and insert values.

Next, obtain your Gemini API Key by visiting [Google AI Studio](https://aistudio.google.com/app/apikey). Ensure you are logged into your Google account, then press the blue button that says 'Create API key' and follow the steps to set up your Google Cloud Project and retrieve your API key. **Make sure to save it in a safe place**.  
Google allows free use of this API without adding billing information, but there are some limitations.

In Google AI Studio, you can monitor the AI's usage by clicking 'View usage data' in the 'Plan' column where your projects are displayed. I recommend monitoring the 'Quota and system limits' tab and sorting by 'actual usage percentage,' as it provides further more detailed information.

You now have everything needed to make the program work.  
Simply put that data you just got into the Client object.

## 📖 About the NuGet package
To use it, you must initialize the [Client](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/GeminiPostSQL/GeminiPostSQLClient.cs) which has three methods:

**[GenerateSQL(string input):](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/GeminiPostSQL/GeminiPostSQLClient.cs#L115)**
Translates natural language to SQL queries.
In case the Client's parameter 'mapOnGenerateSQL' is activated, if 'jsonMap' is empty, it will automatically map the database into JSON.
Returns the generated SQL in case of success, else will return an error message (they all start with '[ERROR]'). In case the Client's parameter 'runOnGenerateSQL' is activated, if succeeds, the method will return the a serialized JSON of the result table.

**[MapDatabase():](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/GeminiPostSQL/GeminiPostSQLClient.cs#L199)**
Maps the database into a serialized JSON.
Returns a boolean value, depending if the mapping process were successful or not, if it is, the JSON is saved in the Client's parameter 'jsonMap'.

**[RunQuery(string generatedSql):](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGet/blob/main/GeminiPostSQL/GeminiPostSQLClient.cs#L274)**
Runs the generated query (or any) in the database server.
Return the result table for the generated SQL sentence. If it fails, it will return a table with only one column named 'Error' with the description of the error. 

> [!TIP]
> You can test it's functionalities with the [Test Demo](https://github.com/LuisMiSanVe/GeminiPostSQL_NuGetTest/tree/main)

## 🚀 Releases
The version will be released using these versioning policies:\
New major features and critical bug fixes will cause the immediate release of a new version, while other minor changes or fixes will wait one week since the time the change is introduced in the repository before being included in the new version, so that other potential changes can be added.
>[!NOTE]
>These potencial new changes will not increase the wait time for the new version beyond one week.

The version number will follow this format: \
\[Major Feature\].\[Minor Feature\].\[Bug Fixes\]

## 💻 Technologies Used
- Programming Language: [C#](https://dotnet.microsoft.com/en-us/languages/csharp)
- Framework: [.Net](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet) 8.0 Framework
- NuGet Packages:
  - [Npgsql](https://www.npgsql.org/) (8.0.5)
  - [RestSharp](https://restsharp.dev/) (112.1.0)
  - [Newtonsoft.Json](https://www.newtonsoft.com/json) (13.0.3)
- Other:
  - [PostgreSQL](https://www.postgresql.org/) (16.3)
  - [pgAdmin 4](https://www.pgadmin.org/) (8.9)
  - Gemini API Key (2.0 Flash)
  - Images (Icons source, later retouched by me):
    - [FreeIcons](https://freeicons.io/)
- Recommended IDE: [Visual Studio](https://visualstudio.microsoft.com/) 2022
