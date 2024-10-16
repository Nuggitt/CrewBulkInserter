using CrewBulkInserter;
using CrewBulkInserter.Models;
using System.Data.SqlClient;

IInserter inserter;

Console.WriteLine("Tast 1 for TitleDirectors, \r\n Tast 2 for TitleWriters");

string input = Console.ReadLine();

switch (input)
{
    case "1":
        inserter = new BulkTitleDirectorInserter();
        break;
    case "2":
        inserter = new BulkTitleWriterInserter();
        break;
    default:
        throw new Exception("Invalid input");

}

int linecount = 0;
List<TitleCrew> titleCrew = new List<TitleCrew>();

string filePath = "C:/IMDBData/title.crew.tsv";

foreach (string line in File.ReadLines(filePath).Skip(1))
{
    if (linecount == 50000)
    {
        break;
    }

    string[] splitLine = line.Split("\t");
    
    if (splitLine.Length != 3)
    {
        throw new Exception("Ikke rigtig antal tabs!" + line);
    }

    string tconst = splitLine[0];
    string Directors = splitLine[1];
    string writers = splitLine[2];



    TitleCrew newTitle = new TitleCrew
    {
        Tconst = tconst,
        Directors = Directors,
        Writers = writers
    };

    titleCrew.Add(newTitle);
    linecount++;
}

Console.WriteLine("List of TitleCrews length: " + titleCrew.Count);

SqlConnection sqlConn = new SqlConnection("server=localhost;database=IMDB;" +
    "user id=sa;password=Holger1208!;TrustServerCertificate=True");

sqlConn.Open();
SqlTransaction transAction = sqlConn.BeginTransaction();

DateTime before = DateTime.Now;

try
{
    inserter.Insert(titleCrew, sqlConn, transAction);
    transAction.Commit();
    //transAction.Rollback();
}
catch (SqlException ex)
{
    Console.WriteLine(ex.Message);
    transAction.Rollback();
}

DateTime after = DateTime.Now;

sqlConn.Close();

Console.WriteLine("milliseconds passed: " + (after - before).TotalMilliseconds);

int? ParseInt(string value)
{
    if (value.ToLower() == "\\n") // checks if it is \n
    {
        return null;
    }

    if (int.TryParse(value, out int result))
    {
        return result;
    }
    else
    {
        // Handle the case where the value is not a valid integer
        return null;
    }
}

