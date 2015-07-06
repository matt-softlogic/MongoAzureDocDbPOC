using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace WebApi_Mvc5_MongoDb_POC
{
    public class Settings
    {
        public string Database { get; set; }
        public string MongoConnection { get; set; }
        public string DocDbConnection    { get; set; }
        public string DbAuthKey { get; set; }
        public string Repository { get; set; }
    }
}
