using Newtonsoft.Json;
using Npgsql;
using RestSharp;
using System;
using System.Data;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GeminiPostSQL
{
    public class GeminiPostSQLClient
    {
        // Attributes
        public string apikey { get; set; }
        public string database { get; set; }
        public string jsonMap { get; set; }
        public bool mapOnGenerateSQL { get; set; }
        public bool runOnGenerateSQL { get; set; }

        // Builders
        
        /// <summary>
        /// Empty Client Builder (You'll need to fill all its parameters in order to use its methods)
        /// </summary>
        /// <returns>Empty Client object</returns>
        public GeminiPostSQLClient()
        {
            apikey = "";
            database = "";
            jsonMap = "";
        }

        /// <summary>
        /// Client Builder adding the Gemini API Key (You'll need to fill all its parameters in order to use its methods)
        /// </summary>
        /// <param name="apikey">Your Gemini API Key</param>
        /// <returns>Client object with the API Key <paramref name="apikey"/></returns>
        public GeminiPostSQLClient(string apikey) 
        { 
            this.apikey = apikey;
            database = "";
            jsonMap = "";
        }

        /// <summary>
        /// Client Builder adding the Gemini API Key and database connection string
        /// </summary>
        /// <param name="apikey">Your Gemini API Key</param>
        /// <param name="database">Connection string for the database, it should have this structure: Host=X;Username=X;Password=X;Database=X (The 'X' letters must be replaced with the actual data)</param>
        /// <returns>Client object with the API Key <paramref name="apikey"/> and the database connection string <paramref name="database"/></returns>
        public GeminiPostSQLClient(string apikey, string database) { 
            this.apikey = apikey;
            this.database = database;
            jsonMap = "";
        }

        /// <summary>
        /// Client Builder adding the Gemini API Key and database connection data
        /// </summary>
        /// <param name="apikey">Your Gemini API Key</param>
        /// <param name="databaseIp">Ip Adress of the server where the database is hosted</param>
        /// <param name="databaseUser">User with permissions to access the database</param>
        /// <param name="databasePass">Password of the User to access the database</param>
        /// <param name="databaseName">Name of the database where you want to connect</param>
        /// <returns>Client object with the API Key <paramref name="apikey"/> and the database connection data: <paramref name="databaseIp"/>, <paramref name="databaseUser"/>, <paramref name="databasePass"/>, <paramref name="databaseName"/></returns>
        public GeminiPostSQLClient(string apikey, string databaseIp, string databaseUser, string databasePass, string databaseName)
        {
            this.apikey = apikey;
            database = "Host=" + databaseIp + ";Username=" + databaseUser + ";Password=" + databasePass + ";Database=" + databaseName;
            jsonMap = "";
        }

        /// <summary>
        /// Client Builder adding the Gemini API Key, database connection string and the database already mapped in JSON
        /// </summary>
        /// <param name="apikey">Your Gemini API Key</param>
        /// <param name="database">Connection string for the database, it should have this structure: Host=X;Username=X;Password=X;Database=X (The 'X' letters must be replaced with the actual data)</param>
        /// <param name="jsonMap">Serialized database into a JSON, which Gemini will use to learn its structure</param>
        /// <returns>Client object with the API Key <paramref name="apikey"/>, the database connection string <paramref name="database"/> and mapped database in JSON <paramref name="jsonMap"/></returns>
        public GeminiPostSQLClient(string apikey, string database, string jsonMap)
        {
            this.apikey = apikey;
            this.database = database;
            this.jsonMap = jsonMap;
        }

        /// <summary>
        /// Client Builder adding the Gemini API Key, database connection data and the database already mapped in JSON
        /// </summary>
        /// <param name="apikey">Your Gemini API Key</param>
        /// <param name="databaseIp">Ip Adress of the server where the database is hosted</param>
        /// <param name="databaseUser">User with permissions to access the database</param>
        /// <param name="databasePass">Password of the User to access the database</param>
        /// <param name="databaseName">Name of the database where you want to connect</param>
        /// <param name="jsonMap">Serialized database into a JSON, which Gemini will use to learn its structure</param>
        /// <returns>Client object with the API Key <paramref name="apikey"/>, the database connection data: <paramref name="databaseIp"/>, <paramref name="databaseUser"/>, <paramref name="databasePass"/>, <paramref name="databaseName"/> and mapped database in JSON <paramref name="jsonMap"/></returns>
        public GeminiPostSQLClient(string apikey, string databaseIp, string databaseUser, string databasePass, string databaseName, string jsonMap)
        {
            this.apikey = apikey;
            database = "Host=" + databaseIp + ";Username=" + databaseUser + ";Password=" + databasePass + ";Database=" + databaseName;
            this.jsonMap = jsonMap;
        }

        // Methods

        /// <summary>
        /// Translates natural language to SQL queries.
        /// In case the Client's parameter 'mapOnGenerateSQL' is activated, if 'jsonMap' is empty, it will automatically map the database into JSON
        /// </summary>
        /// <param name="input">Rquest that Gemini will try to fulfill</param>
        /// <returns>Returns the generated SQL in case of success, else will return an error message (they all start with '[ERROR]'). In case the Client's parameter 'runOnGenerateSQL' is activated, if succeeds, the method will return the a serialized JSON of the result table</returns>
        public async Task<string> GenerateSQL(string input)
        {
            string result = "";
            if (apikey != "" && database != "" && input != "")
            {
                // Connects to the database
                var connection = new NpgsqlConnection(database);

                if (connection != null && connection.ConnectionString != "")
                {
                    try
                    {
                        connection.Open();

                        bool jsonMapSuccess = true;
                        if (mapOnGenerateSQL && jsonMap == "")
                            jsonMapSuccess = await MapDatabase();

                        if (jsonMapSuccess)
                        {
                            // Creates context to modify AI's behavior
                            string context = "You're a database assistant, I'll send you requests and you'll return a PostgeSQL query to do my request and if what I request can't be found on the database, tell me, but don't use more words. " +
                                                "This is the database: " +
                                                jsonMap +
                                                "\nAnd this is my request: " +
                                                input;

                            // I create the request
                            var Client = new RestClient("https://generativelanguage.googleapis.com");
                            var request = new RestRequest("/v1beta/models/gemini-2.0-flash:generateContent?key=" + apikey, Method.Post);
                            request.AddHeader("Content-Type", "application/json");

                            var body = new GeminiRequest();
                            body.contents = new Content[] { new Content() { parts = new Part[] { new Part() { text = context } } } };

                            var jsonstring = JsonConvert.SerializeObject(body);

                            request.AddJsonBody(jsonstring);
                            // Sends the request to the service
                            string generatedSql = "";
                            try
                            {
                                var response = Client.Post(request);
                                var resp = JsonDocument.Parse(response.Content);
                                // It extracts the AI's response from the 'Text' field                                                                                      and I remove the SQL Code style the AI adds
                                generatedSql = resp.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString().Replace("```sql", "").Replace("```", "").Replace('\n', ' ').Trim();
                                result = Regex.Replace(generatedSql, @"\s+", " ");

                                if (runOnGenerateSQL && result.Contains("select"))
                                    result = JsonConvert.SerializeObject(RunQuery(result).Result);
                            }
                            catch (HttpRequestException ex)
                            {
                                result = "The provided Gemini API Key has failed to access the endpoint, make sure the API Key or Service is functional (" + ex.StatusCode + ")";

                            }
                            catch (Exception ex)
                            {
                                result = "The provided Gemini API Key has failed to access the endpoint, make sure the API Key or Service is functional (" + ex.Message + ")";
                            }
                        }
                        else
                            result = "The mapping process has failed";

                            connection.Close();
                    }
                    catch (Exception ex)
                    {
                        result = "The configured Database couldn't be reached, please check if the Database can still be connected with this configuration.";
                    }
                }
                else
                    result = "You need to set up a proper Database connection first!";
            }
            else
                result = "All the Client's parameters must be filled.";

            return result;
        }

        /// <summary>
        /// Maps the database into a serialized JSON
        /// </summary>
        /// <returns>Returns a boolean value, depending if the mapping process were successful or not, if it is, the JSON is saved in the Client's parameter 'jsonMap'</returns>
        public async Task<bool> MapDatabase()
        {
            bool result = true;
            if (database != "")
            {
                // Connects to the database
                var connection = new NpgsqlConnection(database);

                connection.Open();
                // OBTAIN DB
                // Get the quantity of tables and columns for the loading bar
                var tableQuantity = new NpgsqlCommand("SELECT (" +
                                                        "SELECT COUNT(*) FROM information_schema.tables " +
                                                        "WHERE table_type = 'BASE TABLE' AND table_name NOT LIKE 'pg_%' AND table_name NOT LIKE 'sql_%') +" +
                                                        "(SELECT COUNT(*) FROM information_schema.columns " +
                                                        "WHERE table_schema NOT LIKE 'pg_%' AND table_name NOT LIKE 'sql_%')", connection).ExecuteReader();
                tableQuantity.Close();
                // Tables
                var tablesDB = new NpgsqlCommand("SELECT CONCAT(table_schema, '.', table_name) AS full_table_name " +
                                                    "FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_name NOT LIKE 'pg_%' AND table_name NOT LIKE 'sql_%' " +
                                                    "ORDER BY full_table_name;", connection).ExecuteReader();
                // Table           Column(Type)
                Dictionary<string, List<string>> tables = new Dictionary<string, List<string>>();

                while (tablesDB.Read())
                {
                    if (!tables.ContainsKey(tablesDB.GetString(0)))
                        //         Name                   Columns
                        tables.Add(tablesDB.GetString(0), null);
                }
                tablesDB.Close();
                // Columns
                foreach (string tableName in tables.Keys)
                {
                    var columnsDB = new NpgsqlCommand("SELECT c.column_name, c.data_type, CASE WHEN tc.constraint_type = 'PRIMARY KEY' THEN 'PK' WHEN tc.constraint_type = 'FOREIGN KEY' THEN 'FK' ELSE '' END AS key_type " +
                                                        "FROM information_schema.columns c " +
                                                        "LEFT JOIN information_schema.key_column_usage kcu ON c.table_schema = kcu.table_schema AND c.table_name = kcu.table_name AND c.column_name = kcu.column_name " +
                                                        "LEFT JOIN information_schema.table_constraints tc ON kcu.constraint_name = tc.constraint_name AND kcu.table_schema = tc.table_schema AND kcu.table_name = tc.table_name " +
                                                        "WHERE c.table_schema = '" + tableName.Substring(0, tableName.IndexOf('.')) + "' AND c.table_name = '" + tableName.Remove(0, tableName.IndexOf('.') + 1) + "'" +
                                                        "ORDER BY c.column_name;", connection).ExecuteReader();

                    List<string> columns = new List<string>();

                    while (columnsDB.Read())
                    {
                        string columnInfo = columnsDB.GetString(0) + "(" + columnsDB.GetString(1) + ")";
                        if (!columnsDB.GetString(2).Equals(""))
                            columnInfo = columnsDB.GetString(0) + "(" + columnsDB.GetString(1) + ") (" + columnsDB.GetString(2) + ")";

                        if (!columns.Contains(columnInfo))
                        {   //      Name(Type)(Key)
                            columns.Add(columnInfo);

                            tables[tableName] = columns;
                        }
                    }
                    columnsDB.Close();
                }

                var opcions = new JsonSerializerOptions
                {
                    WriteIndented = true // JSON format
                };

                jsonMap = System.Text.Json.JsonSerializer.Serialize(tables, opcions);
                connection.Close();
            }
            else
                result = false;

            return result;
        }

        /// <summary>
        /// Runs the generated query (or any) in the database server
        /// </summary>
        /// <param name="generatedSql">The SQL sentence the server will run</param>
        /// <returns>Return the result table for the generated SQL sentence <paramref name="generatedSql"/>. If it fails, it will return a table with only one column named 'Error' with the description of the error.</returns>
        public async Task<DataTable> RunQuery(string generatedSql)
        {
            DataTable result = new DataTable();
            if (database != "" && generatedSql != "")
            {
                try
                {
                    // Connects to the database
                    var connection = new NpgsqlConnection(database);

                    connection.Open();
                    try
                    {
                        var resultBBDD = new NpgsqlCommand(generatedSql, connection).ExecuteReader();

                        result.Load(resultBBDD);
                    }
                    catch (Exception ex)
                    {
                        result.Columns.Add("Error", typeof(string));
                        result.Rows.Add("An error was thrown while running the generated query (" + generatedSql + "): " + ex.Message);
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    result.Columns.Add("Error", typeof(string));
                    result.Rows.Add("The configured Database couldn't be reached, please check if the Database can still be connected with this configuration.");
                }
            }
            else 
            {
                result.Columns.Add("Error", typeof(string));
                result.Rows.Add("The Client's database must be filled");
            }

            return result;
        }
    }
}
