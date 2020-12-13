﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;


namespace AeroMaterialHandlingDatabaseApplication
{
    public partial class fViewPage : Form
    {


        public fViewPage()
        {
            InitializeComponent();
            this.AcceptButton = btSearch;

        }

        private void ViewPage_Load(object sender, EventArgs e)
        {
            //Pulls entryTitle and tagName from database and adds it to the searchbar autocomplete collection.
            OleDbConnection con = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=D:\\School\\Capstone\\Repository\\AMHDataBase\\Aero_Material_Handling.accdb");
            string accessQuery = "select AMH_Entries.entryTitle, AMH_Tags.tagName " +
                                 "from AMH_Tags inner join(AMH_Entries inner join AMH_Tag_Entry on AMH_Entries.entryID = AMH_Tag_Entry.entryID) " +
                                 "on AMH_Tags.tagID = AMH_Tag_Entry.tagID";
            OleDbCommand com = new OleDbCommand(accessQuery, con);
            con.Open();
            OleDbDataReader dr = com.ExecuteReader();
            AutoCompleteStringCollection autotext = new AutoCompleteStringCollection();
            while (dr.Read())
            {
                autotext.Add(dr.GetString(0));
                autotext.Add(dr.GetString(1));
            }
            tbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
            tbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tbSearch.AutoCompleteCustomSource = autotext;
            con.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void scDivide_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void btNewEntry_Click(object sender, EventArgs e)
        {
            //Opens EditPage
            fEditPage f = new fEditPage();
            f.ShowDialog();
        }

        private void pbUser_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pnSearchResult_MouseClick(object sender, MouseEventArgs e)
        {
            //This Code will set the splitter distance split panal2
            scDivide.SplitterDistance = 200;
        }

        private void flowLayoutPanel_MouseClick(object sender, MouseEventArgs e)
        {
            //This Code will set the splitter distance to focus on split panal1
            scDivide.SplitterDistance = 800;
        }
        private void scDivide_Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            //This Code will set the splitter distance to focus on split panal1
            scDivide.SplitterDistance = 800;
        }

        private void btSearch_Click(object sender, EventArgs e)
        {

            lbViewTitle.Text = "";
            tbViewShortDescription.Text = "";
            tbViewLongDescription.Text = "";
            lblTags.Text = "";

            flp1.Controls.Clear();
            //Establish connection to DB
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\School\\Capstone\\Repository\\AMHDataBase\\Aero_Material_Handling.accdb");

            try
            {
                con.Open();
                //Create query to search entry title and tags from DB
                string searchQuery = "select AMH_Entries.entryTitle, AMH_Entries.entryDescShort, AMH_Entries.entryDescLong, AMH_Tags.tagName, AMH_Attachments.attachmentFile " +
                                     "from AMH_Attachments inner join((AMH_Tags inner join (AMH_Entries inner join AMH_Tag_Entry on AMH_Entries.entryID = AMH_Tag_Entry.entryID)" +
                                     "on AMH_Tags.tagID = AMH_Tag_Entry.tagID) inner join AMH_Attachment_Entry on AMH_Entries.entryID = AMH_Attachment_Entry.entryID) " +
                                     "on AMH_Attachments.attachmentID = AMH_Attachment_Entry.attachmentID where entryTitle = '" + tbSearch.Text + "'";

                OleDbCommand com = new OleDbCommand(searchQuery, con);

                OleDbDataReader accessReader = com.ExecuteReader();
                //Populates the broad view(right side) with data related to the search
                while (accessReader.Read())
                {
                    lbViewTitle.Text = accessReader.GetValue(0).ToString();
                    tbViewShortDescription.Text = accessReader.GetValue(1).ToString();
                    tbViewLongDescription.Text = accessReader.GetValue(2).ToString();
                    lblTags.Text = accessReader.GetValue(3).ToString();
                }

                accessReader.Close();
                com.Dispose();
                con.Close();
            }//End try
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }//End catch
            Console.Read();

            //Automatically start populating the flow planel
            populateItems();


        }



        //Poplulate the flowpanel left side
        private void populateItems()
        {
            ListItem[] listItems = new ListItem[10];

            //Create connection to DB
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\School\\Capstone\\Repository\\AMHDataBase\\Aero_Material_Handling.accdb");
            //Create query to pull data from DB
            string searchQuery = "select AMH_Entries.entryTitle, AMH_Entries.entryDescShort, AMH_Tags.tagName " +
                                "from AMH_Tags inner join (AMH_Entries inner join AMH_Tag_Entry on AMH_Entries.entryID = AMH_Tag_Entry.entryID)" +
                                "on AMH_Tags.tagID = AMH_Tag_Entry.tagID  " +
                                "where entryTitle LIKE '%" + tbSearch.Text + "%'";

            con.Open();
            OleDbCommand com = new OleDbCommand(searchQuery, con);
            OleDbDataAdapter da = new OleDbDataAdapter(searchQuery, con);
            OleDbDataReader accessReader = com.ExecuteReader();

            string builder = "";

            //This is where you could add more than one results I think
            //The search Query only pulls one result and we havent figured out the way to add different results into different list items and insert them into flp1
            for (int i = 0; i < listItems.Length - 1; i++)
            {
                while (accessReader.Read())
                {

                    listItems[i] = new ListItem();
                    listItems[i].Title = accessReader[0].ToString();
                    listItems[i].shortDesc = accessReader[1].ToString();
                    builder += "  " + accessReader[2].ToString();//string builder holds the tags as they come in from the accessReader
                    listItems[i].Tags = builder;
                }
                lblTags.Text = builder;


                if (flp1.Controls.Count < 0)
                {
                    flp1.Controls.Clear();
                }
                else
                {
                    flp1.Controls.Add(listItems[i]);
                }
                accessReader.NextResult(); //This line is what grabs the next tag
            }
            accessReader.Close();
            com.Dispose();
            con.Close();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {



        }

        private void btLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            fLogIn f = new fLogIn();
            f.Show();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void lblTags_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}