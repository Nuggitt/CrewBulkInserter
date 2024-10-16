using CrewBulkInserter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrewBulkInserter
{
    public class BulkTitleWriterInserter : IInserter
    {
        public void Insert(List<TitleCrew> titleCrew, SqlConnection sqlConn, SqlTransaction sqlTransaction)
        {
            DataTable titleWriterTable = new DataTable();

            DataColumn tconst = new DataColumn("tconst", typeof(string));
            DataColumn nconst = new DataColumn("nconst", typeof(string));

            titleWriterTable.Columns.Add(tconst);
            titleWriterTable.Columns.Add(nconst);

            foreach (TitleCrew title in titleCrew)
            {
                if (title.Writers != null)
                {
                    string[] writers = title.Writers.Split(',');

                    foreach (var writer in writers)
                    {
                        DataRow row = titleWriterTable.NewRow();
                        FillParameter(row, "tconst", title.Tconst);
                        FillParameter(row, "nconst", writer);
                        titleWriterTable.Rows.Add(row);
                    }
                }
            }

            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTransaction);
            bulkCopy.DestinationTableName = "TitleWriters";
            bulkCopy.WriteToServer(titleWriterTable);




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
