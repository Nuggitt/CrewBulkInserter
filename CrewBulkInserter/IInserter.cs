using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrewBulkInserter
{
    public interface IInserter
    {
        void Insert(List<Title> titles, SqlConnection sqlConn, SqlTransaction sqlTransaction);
    }
}
