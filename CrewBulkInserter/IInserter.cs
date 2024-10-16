using CrewBulkInserter.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrewBulkInserter
{
    public interface IInserter
    {
        void Insert(List<TitleCrew> titles, SqlConnection sqlConn, SqlTransaction sqlTransaction);
    }
}
