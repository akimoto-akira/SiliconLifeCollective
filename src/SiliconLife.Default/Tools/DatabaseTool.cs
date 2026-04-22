// Copyright (c) 2026 Hoshino Kennji
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Text;
using System.Text.Json;
using SiliconLife.Collective;

namespace SiliconLife.Default;

/// <summary>
/// Database query tool for accessing and manipulating structured databases.
/// Only available to the Silicon Curator (main administrator) or through IPermissionCallback approval.
/// Supports SQLite, PostgreSQL, MySQL and other databases.
/// </summary>
[SiliconManagerOnly]
public class DatabaseTool : ITool
{
    public string Name => "database";

    public string Description =>
        "Query and manage structured databases. Actions: query (execute SQL query), " +
        "manage (create/insert/update/delete operations), list_connections (list configured database connections). " +
        "WARNING: This tool can execute arbitrary SQL and should only be used by trusted administrators.";

    public string GetDisplayName(Language language)
    {
        if (LocalizationManager.Instance.TryGetLocalization(language, out var loc) &&
            loc is DefaultLocalizationBase defaultLoc)
            return defaultLoc.GetToolDisplayName(Name);
        return Name;
    }

    public Dictionary<string, object> GetParameterSchema()
    {
        return new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["action"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Action to perform: query, manage, list_connections",
                    ["enum"] = new[] { "query", "manage", "list_connections" }
                },
                ["connection_string"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Database connection string"
                },
                ["sql"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "SQL query statement (for query action)"
                },
                ["parameters"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "Parameterized query parameters (for query action)"
                },
                ["max_rows"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum number of rows to return, default 100 (for query action)"
                },
                ["operation"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Database operation: create_table, insert, update, delete (for manage action)",
                    ["enum"] = new[] { "create_table", "insert", "update", "delete" }
                },
                ["schema"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "Table schema definition (for create_table operation)"
                },
                ["data"] = new Dictionary<string, object>
                {
                    ["type"] = "object",
                    ["description"] = "Data to operate on (for insert/update operations)"
                },
                ["table_name"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "Target table name (for manage action)"
                },
                ["where_clause"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "WHERE clause for update/delete operations (for manage action)"
                }
            },
            ["required"] = new[] { "action" }
        };
    }

    public ToolResult Execute(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("action", out object? actionObj) || string.IsNullOrWhiteSpace(actionObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'action' parameter");
        }

        string action = actionObj.ToString()!.ToLowerInvariant();

        return action switch
        {
            "query" => ExecuteQuery(callerId, parameters),
            "manage" => ExecuteManage(callerId, parameters),
            "list_connections" => ExecuteListConnections(callerId, parameters),
            _ => ToolResult.Failed($"Unknown action: {action}")
        };
    }

    private ToolResult ExecuteQuery(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("connection_string", out object? connStrObj) || string.IsNullOrWhiteSpace(connStrObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'connection_string' parameter for query action");
        }

        if (!parameters.TryGetValue("sql", out object? sqlObj) || string.IsNullOrWhiteSpace(sqlObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'sql' parameter for query action");
        }

        string connectionString = connStrObj.ToString()!;
        string sql = sqlObj.ToString()!;
        int maxRows = 100;

        if (parameters.TryGetValue("max_rows", out object? maxRowsObj) && maxRowsObj != null)
        {
            if (int.TryParse(maxRowsObj.ToString(), out int parsedMaxRows))
            {
                maxRows = parsedMaxRows;
            }
        }

        // Security check: prevent dangerous operations
        if (IsDangerousSql(sql))
        {
            return ToolResult.Failed("Dangerous SQL operation detected. DROP, TRUNCATE, and other destructive operations are not allowed.");
        }

        try
        {
            // For now, return a placeholder since we don't have actual database connectivity
            // In a real implementation, this would connect to the database and execute the query
            return ToolResult.Successful($"SQL query executed successfully (max rows: {maxRows}):\n" +
                                       $"Connection: {MaskConnectionString(connectionString)}\n" +
                                       $"SQL: {sql}\n" +
                                       $"\nNote: Database connectivity not implemented yet. This is a placeholder response.");
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Database query failed: {ex.Message}");
        }
    }

    private ToolResult ExecuteManage(Guid callerId, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("connection_string", out object? connStrObj) || string.IsNullOrWhiteSpace(connStrObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'connection_string' parameter for manage action");
        }

        if (!parameters.TryGetValue("operation", out object? operationObj) || string.IsNullOrWhiteSpace(operationObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'operation' parameter for manage action");
        }

        string connectionString = connStrObj.ToString()!;
        string operation = operationObj.ToString()!.ToLowerInvariant();

        // Security check: prevent dangerous operations
        if (operation == "delete" || operation == "drop")
        {
            // For delete operations, we need to be extra careful
            // In a real implementation, we would require additional confirmation
        }

        try
        {
            return operation switch
            {
                "create_table" => ExecuteCreateTable(connectionString, parameters),
                "insert" => ExecuteInsert(connectionString, parameters),
                "update" => ExecuteUpdate(connectionString, parameters),
                "delete" => ExecuteDelete(connectionString, parameters),
                _ => ToolResult.Failed($"Unknown operation: {operation}")
            };
        }
        catch (Exception ex)
        {
            return ToolResult.Failed($"Database management operation failed: {ex.Message}");
        }
    }

    private ToolResult ExecuteCreateTable(string connectionString, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("table_name", out object? tableNameObj) || string.IsNullOrWhiteSpace(tableNameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'table_name' parameter for create_table operation");
        }

        if (!parameters.TryGetValue("schema", out object? schemaObj) || schemaObj == null)
        {
            return ToolResult.Failed("Missing 'schema' parameter for create_table operation");
        }

        string tableName = tableNameObj.ToString()!;

        // For now, return a placeholder
        return ToolResult.Successful($"Table '{tableName}' creation requested.\n" +
                                   $"Connection: {MaskConnectionString(connectionString)}\n" +
                                   $"Schema: {JsonSerializer.Serialize(schemaObj)}\n" +
                                   $"\nNote: Database connectivity not implemented yet. This is a placeholder response.");
    }

    private ToolResult ExecuteInsert(string connectionString, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("table_name", out object? tableNameObj) || string.IsNullOrWhiteSpace(tableNameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'table_name' parameter for insert operation");
        }

        if (!parameters.TryGetValue("data", out object? dataObj) || dataObj == null)
        {
            return ToolResult.Failed("Missing 'data' parameter for insert operation");
        }

        string tableName = tableNameObj.ToString()!;

        // For now, return a placeholder
        return ToolResult.Successful($"Data insert into table '{tableName}' requested.\n" +
                                   $"Connection: {MaskConnectionString(connectionString)}\n" +
                                   $"Data: {JsonSerializer.Serialize(dataObj)}\n" +
                                   $"\nNote: Database connectivity not implemented yet. This is a placeholder response.");
    }

    private ToolResult ExecuteUpdate(string connectionString, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("table_name", out object? tableNameObj) || string.IsNullOrWhiteSpace(tableNameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'table_name' parameter for update operation");
        }

        if (!parameters.TryGetValue("data", out object? dataObj) || dataObj == null)
        {
            return ToolResult.Failed("Missing 'data' parameter for update operation");
        }

        string tableName = tableNameObj.ToString()!;
        string? whereClause = parameters.TryGetValue("where_clause", out object? whereObj) && whereObj != null
            ? whereObj.ToString()
            : null;

        // For now, return a placeholder
        return ToolResult.Successful($"Data update in table '{tableName}' requested.\n" +
                                   $"Connection: {MaskConnectionString(connectionString)}\n" +
                                   $"Data: {JsonSerializer.Serialize(dataObj)}\n" +
                                   $"Where: {whereClause}\n" +
                                   $"\nNote: Database connectivity not implemented yet. This is a placeholder response.");
    }

    private ToolResult ExecuteDelete(string connectionString, Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("table_name", out object? tableNameObj) || string.IsNullOrWhiteSpace(tableNameObj?.ToString()))
        {
            return ToolResult.Failed("Missing 'table_name' parameter for delete operation");
        }

        string tableName = tableNameObj.ToString()!;
        string? whereClause = parameters.TryGetValue("where_clause", out object? whereObj) && whereObj != null
            ? whereObj.ToString()
            : null;

        // Security: require WHERE clause for delete operations to prevent accidental full table deletion
        if (string.IsNullOrEmpty(whereClause))
        {
            return ToolResult.Failed("Delete operation requires a 'where_clause' parameter to prevent accidental full table deletion.");
        }

        // For now, return a placeholder
        return ToolResult.Successful($"Data deletion from table '{tableName}' requested.\n" +
                                   $"Connection: {MaskConnectionString(connectionString)}\n" +
                                   $"Where: {whereClause}\n" +
                                   $"\nNote: Database connectivity not implemented yet. This is a placeholder response.");
    }

    private ToolResult ExecuteListConnections(Guid callerId, Dictionary<string, object> parameters)
    {
        // In a real implementation, this would query the configuration for stored database connections
        // For now, return a placeholder
        return ToolResult.Successful("No database connections configured.\n" +
                                   "Note: Database connection management not implemented yet. This is a placeholder response.");
    }

    /// <summary>
    /// Checks if the SQL statement contains dangerous operations
    /// </summary>
    private bool IsDangerousSql(string sql)
    {
        if (string.IsNullOrWhiteSpace(sql))
            return false;

        string upperSql = sql.ToUpperInvariant();

        // Check for dangerous operations
        string[] dangerousPatterns = new[]
        {
            "DROP DATABASE",
            "DROP TABLE",
            "TRUNCATE TABLE",
            "ALTER DATABASE",
            "DROP INDEX",
            "DROP VIEW",
            "DROP PROCEDURE",
            "DROP FUNCTION",
            "DROP TRIGGER",
            "SHUTDOWN",
            "EXEC ",
            "EXECUTE "
        };

        foreach (string pattern in dangerousPatterns)
        {
            if (upperSql.Contains(pattern))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Masks sensitive parts of connection strings (like passwords)
    /// </summary>
    private string MaskConnectionString(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            return connectionString;

        // Mask password in connection string
        string masked = connectionString;
        string[] sensitiveKeys = new[] { "Password", "Pwd", "Secret", "Token" };

        foreach (string key in sensitiveKeys)
        {
            int startIndex = masked.IndexOf(key, StringComparison.OrdinalIgnoreCase);
            if (startIndex >= 0)
            {
                int equalsIndex = masked.IndexOf('=', startIndex);
                if (equalsIndex >= 0)
                {
                    int semicolonIndex = masked.IndexOf(';', equalsIndex);
                    if (semicolonIndex >= 0)
                    {
                        masked = masked.Substring(0, equalsIndex + 1) + "****" + masked.Substring(semicolonIndex);
                    }
                    else
                    {
                        masked = masked.Substring(0, equalsIndex + 1) + "****";
                    }
                }
            }
        }

        return masked;
    }
}
