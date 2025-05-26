using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.FileIO;

class Program
{
    public static void Main(string[] args)
    {
        const string csvFile = "T_ONTIME_REPORTING.csv";

        const string TableName = "OnTimeFlightPerformance";
        const string DBName = "FlightStatistics.db";


        try
        {
            var rows = LoadCsv(csvFile);
            var header = rows[0];

            var parameterNames = CreateParameterNames(header);

            List<string> columns = CreateColumns(header);

            using (var connection = new SqliteConnection($"Data Source={DBName}"))
            {

                connection.Open();

                // Drop existing table
                DropTable(TableName, connection);

                // creating a basic table
                CreateTable(TableName, connection, columns);

                InsertRow(TableName, connection, parameterNames, rows);

                using SqliteCommand selectCommand = SelectData(TableName, connection);
                ReadData(selectCommand, header);

            }

        }
        catch (FileNotFoundException ex)
        {
            Console.Error.WriteLine(ex.Message);
        }

    }

    private static List<string[]> LoadCsv(string csvFile)
    {
        if (!File.Exists(csvFile))
        {
            throw new FileNotFoundException($"CSV file '{csvFile}' not found.");
        }


        var rows = new List<string[]>();
        using var parser = new TextFieldParser(csvFile);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");

        while (!parser.EndOfData)
        {
            rows.Add(parser.ReadFields()!);
        }


        return rows;
    }

    private static void DisplayHeaders(string[] headers)
    {
        foreach (var header in headers)
        {
            Console.WriteLine($"{header}");
        }
    }

    private static void DisplayValues(List<string[]> rows)
    {
        for (int i = 1; i < rows.Count(); i++)
        {
            foreach (var column in rows[i])
            {
                Console.WriteLine(column);
            }

        }
    }

    private static List<string> CreateColumns(string[] header)
    {
        var columns = new List<string>();
        foreach (var col in header)
        {
            columns.Add($"[{col}] TEXT");
        }

        return columns;
    }

    private static List<string> CreateParameterNames(string[] header)
    {

        var parameterNames = new List<string>();
        foreach (var columnName in header)
        {
            parameterNames.Add("@" + columnName);
        }
        return parameterNames;
    }
    private static void DropTable(string TableName, SqliteConnection connection)
    {
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = $"DROP TABLE IF EXISTS {TableName};";
            cmd.ExecuteNonQuery();
        }
    }


    private static void CreateTable(string TableName, SqliteConnection connection, List<String> columns)
    {

        using (var createCommand = connection.CreateCommand())
        {
            createCommand.CommandText = $"CREATE TABLE IF NOT EXISTS {TableName} ({string.Join(", ", columns)});";
            createCommand.ExecuteNonQuery();
        }
    }

    private static void InsertRow(string TableName, SqliteConnection connection, List<string> parameterNames, List<string[]> rows)
    {
        using (var insertCommand = connection.CreateCommand())
        {

            using var transaction = connection.BeginTransaction();
            insertCommand.Transaction = transaction;


            // set empty parameters.
            // need to add the parameters and then the values later...
            var header = rows[0];
            foreach (var columnName in header)
            {
                insertCommand.Parameters.Add(new SqliteParameter("@" + columnName, DbType.String));
            }

            insertCommand.CommandText = $"INSERT INTO {TableName} ({string.Join(", ", header)}) VALUES ({string.Join(", ", parameterNames)});";

            insertCommand.Prepare();
            for (int i = 1; i < rows.Count(); i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    var value = rows[i][j];
                    insertCommand.Parameters[j].Value = value;
                }
                insertCommand.ExecuteNonQuery();

            }
            transaction.Commit();

        }

    }

    private static SqliteCommand SelectData(string TableName, SqliteConnection connection)
    {
        var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = $"SELECT TAIL_NUM, COUNT(*) AS FlightCount FROM {TableName} GROUP BY TAIL_NUM;";
        return selectCommand;
    }

    private static void ReadData(SqliteCommand selectCommand, string[] headers)
    {
        var reader = selectCommand.ExecuteReader();
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                object columnValue = reader.GetValue(i);
                Console.WriteLine($"{columnName}: {columnValue}");
            }
            Console.WriteLine();
        }

    }


}




