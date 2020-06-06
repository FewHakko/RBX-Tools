using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBX_Tools
{
    public class Owner
    {
        public string buildersClubMembershipType { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public string displayName { get; set; }
    }

    public class Example
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Owner owner { get; set; }
        public object shout { get; set; }
        public int memberCount { get; set; }
        public bool isBuildersClubOnly { get; set; }
        public bool publicEntryAllowed { get; set; }
    }
}
