using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkunkworksDebugger
{
    class SteamIDUK_API
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Auth
        {
            public string auth { get; set; }
            public string lookups { get; set; }
            public string day_limit { get; set; }
        }

        public class Profile
        {
            public string steamid64 { get; set; }
            public string steamid { get; set; }
            public string steam3 { get; set; }
            public string steamidurl { get; set; }
            public string playername { get; set; }
            public string profile_bans { get; set; }
            public string avatar { get; set; }
        }

        public class ProfileStatus
        {
            public string vac { get; set; }
            public string tradeban { get; set; }
            public string communityban { get; set; }
            public string ammount_game_bans { get; set; }
        }

        public class Namehistory
        {
            public string name { get; set; }
            public string date { get; set; }
            public string request_time { get; set; }
        }

        public class Root
        {
            public Auth auth { get; set; }
            public Profile profile { get; set; }
            public ProfileStatus profile_status { get; set; }
            public List<Namehistory> namehistory { get; set; }
        }

    }
}
