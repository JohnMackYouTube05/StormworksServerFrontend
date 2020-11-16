//
//Stormworks Dedicated Server Frontend by JohnMack05 and Mondo445
//October 25, 2020
//v3.2
//
using System;
using System.Text;
using System.Windows.Forms;
using System.IO;            //for Streams
using System.Threading;     //to run commands concurrently
using System.Diagnostics;
using System.Linq;
using System.ComponentModel;

namespace SkunkworksDebugger
{
    public partial class Form1 : Form
    {
        StreamReader streamReader;
        StringBuilder strInput;
        Thread th_StartListen, th_RunClient;
        Process proc;
        //public int fucc = 0;

        public bool init = false;
        public int tries = 0;
        public int ticks;
        public Form1()
        {
            InitializeComponent();
        }


        public string fileName1;
        private void Log(string message)
        {
            //toolStripStatusLabel1.Text = message;
        }
        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private async void StartListen()
        {
            using (FileStream stream = File.Open(fileName1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                        Log("Creating streamReader.");
                string path = @"C:\swds\test.txt";
                FileInfo fi = new FileInfo(fileName1);
                StreamReader streamReader = new StreamReader(stream);
                Log("Creating stringBuilder.");
                string strInput;
                strInput = "";

                while (checkBox1.Checked == true)
                {
                    long size = fi.Length;
                    textBox1.TopIndex = textBox1.Items.Count - 1;
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    listBox1.SelectedIndex = -1;
                    timer1.Start();
                    timer3.Start();
                    //fucc += 1;
                    //if(fucc == 250)
                    //{

                    //}



                    try
                    {

                        tries = tries + 1;
                        Log("try " + tries.ToString());
                        strInput = (await streamReader.ReadLineAsync());
                    }
                    catch (Exception err)
                    {
                        Log("catch, Error: " + err.Message);
                        break;
                        
                    }

                    if (strInput != null)
                    {
                        //DisplayMessage(strInput);
                        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                        double len = fi.Length;
                        int order = 0;
                        while (len >= 1024 && order < sizes.Length - 1)
                        {
                            order++;
                            len = len / 1024;
                        }

                        // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
                        // show a single decimal place, and no space.
                        string result = String.Format("{0:0.##} {1}", len, sizes[order]);
                        toolStripStatusLabel1.Text = result;
                        statusStrip1.Update();

                        if (strInput.Contains("Uptime   : ") == true)
                        {
                            Log("uptime tag found. Extracting...");
                            lblUptime.Text = strInput.ToString();
                            lblUptime.Refresh();
                            Replace(5, strInput.ToString());
                        }
                        else if (strInput.Contains("Server Version  : ")==true)
                        {
                            Replace(0, strInput.ToString());
                        }
                        else if (strInput.Contains("Server Name") == true)
                        {
                            Replace(1, strInput.ToString());
                        }
                        else if (strInput.Contains("Tickrate : ") == true)
                        {

                            Log("TPS Tag found. Extracting...");
                            lblTPS.Text = strInput.ToString();
                            lblTPS.Refresh();
                            Replace(6, strInput.ToString());

                            
                        }
                        else if (strInput.Contains("Server Ports") == true)
                        {

                            Log("Ports found. Extracting...");
                            Replace(3, strInput.ToString());

                        }
                        else if (strInput.Contains("Server Config   : ") == true)
                        {

                            Log("config found. Extracting...");
                            Replace(4, strInput.ToString());


                        }
                        else if (strInput.Contains("Network  : ") == true)
                        {

                            Log("network found. Extracting...");
                            Replace(7, strInput.ToString());

                        }
                        else if (strInput.Contains("seed     : ") == true)
                        {

                            Log("Seed found. Extracting...");
                            Replace(9, strInput.ToString());


                        }
                        else if (strInput.Contains("vehicles : ") == true)
                        {

                            Log("vehicles found. Extracting...");
                            Replace(10, strInput.ToString());

                        }
                        else if (strInput.Contains("objects  : ") == true)
                        {

                            Log("objects line found. Extracting...");
                            Replace(11, strInput.ToString());

                        }
                        else if (strInput.Contains("Anti-Lag | More+") == true)
                        {

                            Log("title found. Extracting...");
                            Replace(2, strInput.ToString());

                        }
                        else if (strInput.Contains("No Workshop") == true)
                        {
                            Log("title found. Extracting...");
                            Replace(2, strInput.ToString());
                        }
                        else if (strInput.Contains("tiles    : ") == true)
                        {

                            Log("tiles found. Extracting...");
                            Replace(12, strInput.ToString());

                        }
                        else if (strInput.Contains("time     : ") == true)
                        {

                            Log("time found. Extracting...");
                            Replace(13, strInput.ToString());

                        }
                        else if (strInput.Contains("weather  : ") == true)
                        {

                            Log("weather found. Extracting...");
                            Replace(14, strInput.ToString());

                        }
                        else if (strInput.Contains("fog      : ") == true)
                        {

                            Log("fog found. Extracting...");
                            Replace(15, strInput.ToString());

                        }
                        else if (strInput.Contains("wind     : ") == true)
                        {

                            Log("wind found. Extracting...");
                            Replace(16, strInput.ToString());

                        }
                        else if (strInput.Contains("rain     : ") == true)
                        {

                            Log("rain found. Extracting...");
                            Replace(17, strInput.ToString());

                        }

                        else if (strInput.Contains("Players  : ") == true)
                        {
                            Log("Players tag found. Extracting...");
                            lblPlayers.Text = strInput.ToString();
                            Replace(8, strInput.ToString());
                        }
                        else if(strInput.Contains(""))
                        {
                           
                        }

                        else
                        {
                            DisplayInfo(strInput.ToString());
                        }
                    }
                    else
                    {
                        timer1.Stop();
                        checkBox1.Checked = false;


                    }
                    
                }
            }


            
        }



        private void Replace(int index, string message)
        {
            textBox1.Items.RemoveAt(index);
            textBox1.Items.Insert(index, message);
            textBox1.Refresh();

        }

        private delegate void DisplayDelegate(string message);

        private void DisplayMessage(string message)
        {
            textBox1.Items.Add(message);
            if (textBox1.Items.Count > 18)
            {
                textBox1.Items.RemoveAt(0);
            }

        }

        private void DisplayInfo(string message)
        {
            listBox1.Items.Add(message);
        }

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClosePrcoess_Click(object sender, EventArgs e)
        {   

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (init == false)
            {
                for (int i = 0; i < 18; i += 1)
                {
                    textBox1.Items.Add("0");
                    textBox1.Refresh();
                }
                init = true;
            }
            listBox1.Items.Clear();
            StartListen();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            timer1.Stop();
            checkBox1.Checked = false;
            ticks += 1;
            if(ticks >= 5)
            {
                checkBox1.Checked = true;
                ticks = 0;
                timer1.Start();
                timer2.Stop();
            }

            
            
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\swds",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                fileName1 = openFileDialog1.FileName;
                btnGo.Enabled = true;
                
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            timer1.Start();
            btnOpenFile.Enabled = false;
            btnGo.Enabled = false;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            int lines = 0;
            string path2 = @"C:\swds\export-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + ".txt";
            File.WriteAllLines(path2, listBox1.Items.Cast<string>());
            MessageBox.Show("Export complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEnableWeapons_Click(object sender, EventArgs e)
        {
            //Get trolled
            Process.Start("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                btnGo.Enabled = true;
            }
            else
            {
                btnGo.Enabled = false;
                timer3.Stop();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].ToString().Length == 0) // error here
                {
                    listBox1.Items.RemoveAt(i);// error here
                }
            }
        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                string s = listBox1.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
        }




    }
}