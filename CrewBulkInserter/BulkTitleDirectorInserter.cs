using CrewBulkInserter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CrewBulkInserter
{
    public class BulkTitleDirectorInserter : IInserter
    {
        public void Insert(List<TitleCrew> titleCrew, SqlConnection sqlConn, SqlTransaction sqlTransaction)
        {
            DataTable titleDirectorTable = new DataTable();

            DataColumn tconst = new DataColumn("tconst", typeof(string));
            DataColumn nconst = new DataColumn("nconst", typeof(string));

            titleDirectorTable.Columns.Add(tconst);
            titleDirectorTable.Columns.Add(nconst);

            foreach (TitleCrew title in titleCrew)
            {
                if (title.Directors != null)
                {
                    string[] directors = title.Directors.Split(',');

                    foreach (var director in directors)
                    {
                        DataRow row = titleDirectorTable.NewRow();
                        FillParameter(row, "tconst",  title.Tconst);
                        FillParameter(row, "nconst", director);
                        titleDirectorTable.Rows.Add(row);
                    }
                }
            }

            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTransaction);
            bulkCopy.DestinationTableName = "TitleDirectors";
            bulkCopy.WriteToServer(titleDirectorTable);




        }

        public void FillParameter(DataRow row, string columnName, object? value)
        {
            if (value != null)
            {
                row[columnName] = value;
            }
            else
            {
                row[columnName] = DBNull.Value;
            }
        }
    }
}
