using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic.FileIO;

class Program
{
    public static void Main(string[] args)
    {
        const string csvFile = "T_ONTIME_REPORTING.csv";

        try
        {
            var rows = LoadCsv(csvFile);
            DisplayHeaders(rows[0]);
            DisplayValues(rows);
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
}