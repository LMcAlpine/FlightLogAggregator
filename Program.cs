using Microsoft.Data.Sqlite;

class Program
{
    public static void Main(string[] args)
    {
        const string csvFile = "T_ONTIME_REPORTING.csv";

        try
        {
            var (headers, rows) = LoadCsv(csvFile);
            DisplayHeaders(headers);

        }
        catch (FileNotFoundException ex)
        {
            Console.Error.WriteLine(ex.Message);
        }

    }

    private static (string[] headers, List<string[]> rows) LoadCsv(string csvFile)
    {
        if (!File.Exists(csvFile))
        {
            throw new FileNotFoundException($"CSV file '{csvFile}' not found.");
        }

        string[] headers;
        var rows = new List<string[]>();
        using (var reader = new StreamReader(csvFile))
        {
            var headerLine = reader.ReadLine()!;
            headers = headerLine.Split(",", StringSplitOptions.RemoveEmptyEntries);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                rows.Add(line.Split(",", StringSplitOptions.None));
            }

        }
        return (headers, rows);
    }

    private static void DisplayHeaders(string[] headers)
    {
        foreach (var header in headers)
        {
            Console.WriteLine($"{header}");
        }
    }
}