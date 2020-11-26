using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SkunkworksDebugger.SteamIDUK_API;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace SkunkworksDebugger
{
    public partial class SteamIDHistoryRetriever : Form
    {
        public SteamIDHistoryRetriever()
        {
            InitializeComponent();
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string URL = "https://steamidapi.uk/request.php?api=RVK7QL4DQ3R7L87U6T16" + "&player=" + txtID.Text + "&request_type=1&format=json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.ContentType = "application/json; charset=utf-8";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            string data;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                data = reader.ReadToEnd();

            }
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(data);
            foreach (Namehistory nh in myDeserializedClass.namehistory)
            {
                listNames.Items.Add(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(nh.date)) + " " + nh.name);
            }
            pbAvatar.ImageLocation = myDeserializedClass.profile.avatar;
            pbAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
            pbAvatar.Refresh();
            lblCurrentName.Text = ("Current Name: " + myDeserializedClass.profile.playername);
            lblVacBans.Text = ("VAC Bans: " + myDeserializedClass.profile_status.vac);
            
            

        }
    }
}
