# Flight Log Aggregator

## Overview

This C# application imports flight performance data from a CSV file into a SQLite database and performs basic data analysis through SQL queries. The application demonstrates CSV parsing, database interaction, and data querying.

## Features

* **CSV Parsing**: Reads and processes flight data from a CSV file (`T_ONTIME_REPORTING.csv`).
* **SQLite Database Integration**: Stores CSV data into a structured SQLite database (`FlightStatistics.db`).
* **Error Handling**: Includes exception handling to manage missing or malformed files.
* **Data Analysis Queries**: Executes aggregate queries to provide insights, such as counting flights by aircraft (`TAIL_NUM`).

## Technologies

* **C#** (.NET)
* **SQLite** (via Microsoft.Data.Sqlite)
* **Microsoft VisualBasic FileIO** (for CSV parsing)

## Getting Started

### Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/download)
* [On-Time: Reporting Carrier On-Time Performance (1987-present) (Bureau of Transportation Statistics)](https://www.transtats.bts.gov/DL_SelectFields.aspx?gnoyr_VQ=FGJ&QO_fu146_anzr=b0-gvzr)
  - Select **Tail_Number** option.

### Setup

1. **Clone the Repository**:

```bash
git clone <https://github.com/LMcAlpine/FlightLogAggregator.git>
```

2. **Prepare CSV File**:

* Place your `T_ONTIME_REPORTING.csv` file in the application's root directory.

3. **Run the Application**:

```bash
dotnet run
```

### Database

* After running, the data will be stored in `FlightStatistics.db`.

## Example Output

Upon successful execution, the program outputs flight counts per aircraft, e.g.:

```
TAIL_NUM: N12345
FlightCount: 52

TAIL_NUM: N54321
FlightCount: 47
...
```

## Author

* **Luke McAlpine**
