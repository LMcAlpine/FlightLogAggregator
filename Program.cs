using Microsoft.Data.Sqlite;

class Program
{
    public static void Main(string[] args)
    {
        const string CSVFile = "T_ONTIME_REPORTING.csv";
        if (!File.Exists(CSVFile))
        {
            Console.WriteLine($"CSV file '{CSVFile}' not found.");
            return;
        }

        string[] headers;
        var values = new List<string[]>();
        using (var reader = new StreamReader(CSVFile))
        {
            var headerLine = reader.ReadLine()!;
            headers = headerLine.Split(",", StringSplitOptions.RemoveEmptyEntries);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                values.Add(line.Split(",", StringSplitOptions.None));
            }

        }
        foreach (var header in headers)
        {
            Console.WriteLine($"{header}");
        }


    }
}