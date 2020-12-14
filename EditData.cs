using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Threading;
using System.IO;
using System.Diagnostics;
/*
 * This page is designed to make a new entry and store it onto the database as of now it properly stores the information and tags.
 * it also store attachments in the database but does not properly tie them to the entrey
 */


namespace AeroMaterialHandlingDatabaseApplication
{
    public partial class fEditPage : Form
    {
        protected bool validData;
        string path;
        protected Image image;
        protected Thread getImageThread;        
        public fEditPage()
        {
            InitializeComponent();
            //Lets you add a tag by pressing ENTER key.
            this.AcceptButton = btTagAdd;
            //Lets you close the edit page by pressing ESC key.
            this.CancelButton = btEditExit;
            gbDragDrop.AllowDrop = true;
            pbRegister.AllowDrop = true;
            lbDragDrop.AllowDrop = true;
            this.lbDragDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbDragDrop_DragDrop);
            this.lbDragDrop.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbDragDrop_DragEnter);
        }
                        
        private bool GetFilename(out string filename, DragEventArgs e)
        {
            //This code reads the file extension of the image being dropped into picture box (pbRegister)
            bool ret = false;
            filename = String.Empty;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        string ext = Path.GetExtension(filename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp") || (ext == ".tif"))
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

        //this saves the entry when the user choses to save the file
        private void btSave_Click(object sender, EventArgs e)
        {
            

            //Establishing a connection to the database to enter new entry data.
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\pc\\OneDrive\\Aero_Material_Handling.accdb");
            OleDbCommand cmd = new OleDbCommand("select AMH_Entries.entryTitle, AMH_Entries.entryDescShort, AMH_Entries.entryDescLong, AMH_Tags.tagName, AMH_Attachments.attachmentFile.FileData, AMH_Attachments.attachmentFile.FileName, AMH_Attachments.attachmentFile.FileType " +
                                                "from(AMH_Tags inner join(AMH_Entries inner join AMH_Tag_Entry on AMH_Entries.entryID = AMH_Tag_Entry.entryID) on AMH_Tags.tagID = AMH_Tag_Entry.tagID) " +
                                                "inner join(AMH_Attachments inner join AMH_Attachment_Entry on AMH_Attachments.attachmentID = AMH_Attachment_Entry.attachmentID) " +
                                                "on AMH_Entries.entryID = AMH_Attachment_Entry.entryID where entryTitle = @entryTitle", con);


           //This gets all the text and attachmentes entered by the user.
            cmd.Parameters.AddWithValue("@entryTitle", tbEditTitle.Text.ToLower());
            cmd.Parameters.AddWithValue("@entryDescShort", tbEditShortDesc.Text.ToLower());
            cmd.Parameters.AddWithValue("@entryDescLong", tbEditLongDesc.Text.ToLower());
            cmd.Parameters.AddWithValue("@tagName", lbTagList.Items.ToString());           
            cmd.Parameters.AddWithValue("@attachmentFile.FileData", lbDragDrop.Items.ToString());
            cmd.Parameters.AddWithValue("@attachmentFile.FileName", lbDragDrop.Items.ToString());
            cmd.Parameters.AddWithValue("@attachmentFile.FileType", lbDragDrop.Items.ToString());

            con.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            
            //This section is to check if the entry already exist by finding a title with a matching name compared to the one the user types in the tilte text box
            try
            {
                if (dr.HasRows)
                {
                    MessageBox.Show("Record(s) already exists in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    con.Close();
                    tbEditAddTags.Clear();
                    tbEditLongDesc.Clear();
                    tbEditShortDesc.Clear();
                    tbEditTitle.Clear();
                    lbTagList.Items.Clear();
                }
                else
                {
                    
                    con.Close();
                    con.Open(); 
                    cmd = new OleDbCommand("insert into AMH_Entries(entryTitle,entryDescShort,entryDescLong) values(@entryTitle,@entryDescShort,@entryDescLong)", con);
                    cmd.Parameters.AddWithValue("@entryTitle", tbEditTitle.Text);
                    cmd.Parameters.AddWithValue("@entryDescShort", tbEditShortDesc.Text);
                    cmd.Parameters.AddWithValue("@entryDescLong", tbEditLongDesc.Text);
                    cmd.ExecuteNonQuery();
                   

                    //For loop that grabs each tag from listbox and stores it in database
                    for (int x = 0; x < lbTagList.Items.Count -1; x++)
                    {
                        cmd = new OleDbCommand("insert into AMH_Tags(tagName) values(@tagName)", con);
                        cmd.Parameters.AddWithValue("@tagName", lbTagList.Items[x]); 
                        cmd.ExecuteNonQuery();
                    }

                    for (int x = 0; x < lbDragDrop.Items.Count - 1; x++)
                    {
                        cmd = new OleDbCommand("insert into AMH_Attachments(attachmentName,attachmentFile.FileData,attachmentFile.FileName,attachmentFile.FileType) values(@attachmentName,@attachmentFile.FileData,@attachmentFile.FileName,@attachmentFile.FileType)", con);                        
                        cmd.Parameters.AddWithValue("@attachmentFile.FileData", lbDragDrop.Items[x]);
                        cmd.Parameters.AddWithValue("@attachmentFile.FileName", lbDragDrop.Items[x]);
                        cmd.Parameters.AddWithValue("@attachmentFile.FileType", lbDragDrop.Items[x]);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            //should there be a issue making a new entry this will inform the user that the entry was not added
            catch (Exception)
            {
                MessageBox.Show("Error inserting records.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MessageBox.Show("Entry saved.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            //This will tie the tags to the entry's title, allowing the entry to be searched via its tags.
            string currentEntryID = "";
            try
            {
                con.Close();
                con.Open();
                //get the most recent title entered and store it
                cmd = new OleDbCommand("Select LAST(entryID) FROM(AMH_Entries)", con);
                OleDbDataReader read = cmd.ExecuteReader();

                while (read.Read())
                {
                    //Store the most current entryID
                    currentEntryID = read.GetValue(0).ToString();
                }

                con.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            if (con.State == ConnectionState.Open)
                con.Close();

            //get tags from last entry
            string[] curTagID = new string[lbTagList.Items.Count];
            try
            {
                con.Close();
                con.Open();
                //get the most recent title entered and store it
                for (int i = 0; i <= lbTagList.Items.Count - 1; i++)
                {
                    cmd = new OleDbCommand("Select (tagID) FROM(AMH_Tags) WHERE tagName = '" + lbTagList.Items[i] + "'", con);
                    OleDbDataReader read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        curTagID[i] = read.GetValue(0).ToString();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

            con.Close();
            con.Open();
            //loop through all the tags and tie them with the entryID
            for (int x = 0; x < lbTagList.Items.Count - 1; x++)
            {
                try
                {

                    cmd = new OleDbCommand("INSERT INTO AMH_Tag_Entry(tagID, entryID) Values(@tagID, @entryID)", con);
                    cmd.Parameters.AddWithValue("@tagID", curTagID[x]);
                    cmd.Parameters.AddWithValue("@entryID", currentEntryID);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
            }
            //clears the text boxes after properly storing the new entry making it easier to add multiple new entries at a time
            tbEditAddTags.Clear();
            tbEditLongDesc.Clear();
            tbEditShortDesc.Clear();
            tbEditTitle.Clear();
            lbTagList.Items.Clear();
            lbDragDrop.Items.Clear();
            if (con.State == ConnectionState.Open)
                con.Close();
            this.Close();
        }
        
       //Allows the user want to clear their entry and start from the begining 
        private void btClear_Click(object sender, EventArgs e)
        {
            tbEditAddTags.Clear();
            tbEditLongDesc.Clear();
            tbEditShortDesc.Clear();
            tbEditTitle.Clear();
            lbTagList.Items.Clear();
            pbRegister.Image = null;
        }
 
        //Allows the user to remove undesired tags with the entry before submission
        private void btEditDeleteTag_Click(object sender, EventArgs e)
        {
            //Deletes tags from listbox
            while (lbTagList.SelectedItems.Count > 0)
            {
                lbTagList.Items.Remove(lbTagList.SelectedItems[0]);
            }

        }

        //Allows the user to add the tag typed in the textbox intothe list box so the user 
        private void btTagAdd_Click(object sender, EventArgs e)
        {          
            string currentTag = tbEditAddTags.Text;
            lbTagList.Items.Add(currentTag);

            tbEditAddTags.Clear(); 
            tbEditAddTags.Focus();

        }
        // all the code bellow here until the next comment are to make drag and drop workallow the user to add attachments via drap and drop method.
        private void gbDragDrop_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void gbDragDrop_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void pbRegister_DragEnter(object sender, DragEventArgs e)
        {
            string filename;
            validData = GetFilename(out filename, e);
            if (validData)
            {
                path = filename;
                getImageThread = new Thread(new ThreadStart(LoadImage));
                getImageThread.Start();
                e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void pbRegister_DragDrop(object sender, DragEventArgs e)
        {
            if (validData)
            {
                while (getImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                pbRegister.Image = image;
            }
        }
        protected void LoadImage()
        {
            image = new Bitmap(path);
        }
              
        private void lbDragDrop_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            for (i = 0; i < s.Length; i++)
                lbDragDrop.Items.Add(Path.GetFileName(s[i]));
        }

        private void lbDragDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        //removes a attachment added to the list by user.
        private void btEditRemove_Click(object sender, EventArgs e)
        {
            //Deletes item from the drag and drop listbox
            while (lbDragDrop.SelectedItems.Count > 0)
            {
                lbDragDrop.Items.Remove(lbDragDrop.SelectedItems[0]);
            }
        }
    }
}
