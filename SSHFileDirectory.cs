using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSHConnector
{
    public class SSHFileDirectory
    {
        public string Path { get; set; }
        public string ModifiedDate { get; set; }

        public override string ToString()
        {
            return $"{Path}     {ModifiedDate}";
        }
    }
}
