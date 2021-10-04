using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Поломка.db
{
    public static class DBInstance
    {
        static DBEntityConnection connection;
        static object objectLock = new object();
        public static DBEntityConnection Get()
        {
            lock (objectLock)
            {
                if (connection == null)
                    connection = new DBEntityConnection();
                return connection;
            }
        }
    }
}
