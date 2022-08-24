using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/* Chris Prickett, 27/07/2022
   Data definitions project build, taking input from user in the designated fields, adding that data to the array and
   then displaying two categories in the list view which can be clicked to fill text fields with all the information
   for that specific array point.*/

namespace DataDefinitions
{
    public partial class FormDataDefinitions : Form
    {
        public FormDataDefinitions()
        {
            InitializeComponent();
        }
        // 9.1 Create a global 2D string array, use static variables for the dimensions (row, column)
        private static int row = 12;
        private static int col = 4;
        private string[,] ArrayDefinitions = new string[row, col];
        private int ptr = 0;
        private string initialFileName = "definitions.bin";
        #region Utilities
        private void InitialiseArray()
        {
            for (int x = 0; x < row; x++)
            {
                ArrayDefinitions[x, 0] = "~";
                ArrayDefinitions[x, 1] = " ";
                ArrayDefinitions[x, 2] = " ";
                ArrayDefinitions[x, 3] = " ";
            }
            DisplayArray();
        }
        // 9.5 Create a CLEAR method to clear the four text boxes so a new definition can be added
        private void ClearTextBoxes()
        {
            TextBoxName.Clear();
            TextBoxCategory.Clear();
            TextBoxStructure.Clear();
            RichTextBoxDefinition.Clear();
            TextBoxName.Focus();
        }
        private void FormDataDefinitions_Load(object sender, EventArgs e)
        {
            InitialiseArray();
        }
        /* 9.6 Write the code for a Bubble Sort method to sort the 2D array by Name ascending,
         ensure you use a separate swap method that passes the array element to be swapped (do not use any built-in array methods) */
        private void SortDisplay()
        {
            for (int a = 1; a < row; a++)
            {            
                for (int b = 0; b < row -1; b++)
                {
                    if (!string.IsNullOrEmpty(ArrayDefinitions[b + 1, 0]))
                    {
                        if (string.Compare(ArrayDefinitions[b, 0], ArrayDefinitions[b + 1, 0]) == 1)
                        {
                            for (int c = 0; c < col; c++)
                            {
                                string temp = ArrayDefinitions[b, c];
                                ArrayDefinitions[b, c] = ArrayDefinitions[b + 1, c];
                                ArrayDefinitions[b + 1, c] = temp;
                            }
                        }
                    }
                }
            }
        }
        // 9.8 Create a display method that will show the following information in a ListView: Name and Category       
        private void DisplayArray()
        {
            int size = ArrayDefinitions.GetLength(0);
            ListViewDisplay.Items.Clear();
            for (int x = 0; x < size; x++)
            {
                ListViewItem item = new ListViewItem(ArrayDefinitions[x, 0]);
                item.SubItems.Add(ArrayDefinitions[x, 1]);
                ListViewDisplay.Items.Add(item);
            }
        }
        /* 9.9 Create a method so the user can select a definition (Name) from the ListView
         and all the information is displayed in the appropriate Textboxes */
        private void ListViewDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            int cntItem = ListViewDisplay.SelectedIndices[0];
            TextBoxName.Text = ArrayDefinitions[cntItem, 0];
            TextBoxCategory.Text = ArrayDefinitions[cntItem, 1];
            TextBoxStructure.Text = ArrayDefinitions[cntItem, 2];
            RichTextBoxDefinition.Text = ArrayDefinitions[cntItem, 3];
        }
        /* 9.12 All code is required to be adequately commented,
         and each interaction must have suitable error trapping and/or feedback */
        private void ErrorMessage(int error)
        {
            StatusStripMessage.Items.Clear();
            switch (error)
            {
                case 1:
                    StatusStripMessage.Items.Add("Text boxes have been cleared.");
                    break;
                case 2:
                    StatusStripMessage.Items.Add("All rows have some data added.");
                    break;
                case 3:
                    StatusStripMessage.Items.Add("There was no data in any field to add.");
                    break;
                case 4:
                    StatusStripMessage.Items.Add("Edit Complete.");
                    break;
                case 5:
                    StatusStripMessage.Items.Add("Edit Unsuccessful.");
                    break;
                case 6:
                    StatusStripMessage.Items.Add("Save Complete.");
                    break;
                case 7:
                    StatusStripMessage.Items.Add("Save Unsuccessful.");
                    break;
                case 8:
                    StatusStripMessage.Items.Add("Open Complete.");
                    break;
                case 9:
                    StatusStripMessage.Items.Add("Open Unsuccessful.");
                    break;
                case 10:
                    StatusStripMessage.Items.Add("Search successful.");
                    break;
                case 11:
                    StatusStripMessage.Items.Add("Search unsuccessful. Try again");
                    break;
                case 12:
                    StatusStripMessage.Items.Add("Delete Complete.");
                    break;
                case 13:
                    StatusStripMessage.Items.Add("Delete Unsuccessful.");
                    break;
            }
        }
        private void SaveRecord(string saveFileName)
        {
            try
            {
                using (Stream stream = File.Open(saveFileName, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        for (int x = 0; x < row; x++)
                        {
                            for (int y = 0; y < col; y++)
                            {
                                writer.Write(ArrayDefinitions[x, y]);
                            }
                        }
                    }
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString());
                ErrorMessage(7);
            }
        }
        private void OpenRecord(string loadFileName)
        {
            try
            {
                using (Stream stream = File.Open(loadFileName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        int x = 0;
                        Array.Clear(ArrayDefinitions, 0, ArrayDefinitions.Length);
                        while (stream.Position < stream.Length)
                        {
                            for (int y = 0; y < col; y++)
                            {
                                ArrayDefinitions[x, y] = reader.ReadString();
                            }
                            x++;
                        }
                    }
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.ToString());
                ErrorMessage(9);
            }
            DisplayArray();
        }
        #endregion Utilities
        #region Buttons
        // 9.2 Create an ADD button that will store the information from the 4 text boxes into the 2D array
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                for (int x = 0; x < row; x++)
                {
                    if (ptr < row)
                    {
                        ArrayDefinitions[x, 0] = TextBoxName.Text;
                        ArrayDefinitions[x, 1] = TextBoxCategory.Text;
                        ArrayDefinitions[x, 2] = TextBoxStructure.Text;
                        ArrayDefinitions[x, 3] = RichTextBoxDefinition.Text;
                        break;
                    }
                    if (x == row - 1)
                    {
                        MessageBox.Show("The array is full.", "Array is FULL");
                    }
                }
            }
            catch
            {
                ErrorMessage(2);
            }
            ClearTextBoxes();
            SortDisplay();
            DisplayArray();
        }
        // 9.3 Create an EDIT button that will allow the user to modify any information from the 4 text boxes into the 2D array
        private void ButtonEdit_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int edit = ListViewDisplay.SelectedIndices[0];
                if (edit >= 0)
                {
                    if (!string.Equals(ArrayDefinitions[edit, 0], TextBoxName.Text)
                        || !string.Equals(ArrayDefinitions[edit, 1], TextBoxCategory.Text)
                        || !string.Equals(ArrayDefinitions[edit, 2], TextBoxStructure.Text)
                        || !string.Equals(ArrayDefinitions[edit, 3], RichTextBoxDefinition.Text))
                    {
                        var result = MessageBox.Show("Edit Entry?", "Edit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.OK)
                        {
                            ArrayDefinitions[edit, 0] = TextBoxName.Text;
                            ArrayDefinitions[edit, 1] = TextBoxCategory.Text;
                            ArrayDefinitions[edit, 2] = TextBoxStructure.Text;
                            ArrayDefinitions[edit, 3] = RichTextBoxDefinition.Text;
                            DisplayArray();
                            ClearTextBoxes();
                            ErrorMessage(4);
                        }
                        else
                        {
                            ErrorMessage(5);
                        }
                    }
                    else
                    {
                        ErrorMessage(5);
                    }
                }
            }
            catch
            {
                ErrorMessage(5);
            }
            ClearTextBoxes();
            SortDisplay();
            DisplayArray();
        }
        // 9.4 Create a DELETE button that removes all the information from a single entry of the array; the user must be prompted before the final deletion occurs
        private void ButtonDelete_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int deleteEntry = ListViewDisplay.SelectedIndices[0];
                if (deleteEntry >= 0 && !string.Equals(ArrayDefinitions[deleteEntry, 0], "~"))
                {
                    DialogResult delName = MessageBox.Show("Delete Entry?",
                    "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (delName == DialogResult.Yes)
                    {
                        ListViewItem lvi = new ListViewItem(ArrayDefinitions[deleteEntry, 0]);
                        ArrayDefinitions[deleteEntry, 0] = ("~");
                        ArrayDefinitions[deleteEntry, 1] = (" ");
                        ArrayDefinitions[deleteEntry, 2] = (" ");
                        ArrayDefinitions[deleteEntry, 3] = (" ");
                        ClearTextBoxes();
                        DisplayArray();
                        ErrorMessage(12);
                    }
                    else
                    {
                        MessageBox.Show("Item NOT Deleted", "Delete Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                ErrorMessage(13);
            }
            ClearTextBoxes();
            SortDisplay();
            DisplayArray();
        }
        /* 9.7 Write the code for a Binary Search for the Name in the 2D array and
           display the information in the other textboxes when found, add suitable feedback
           if the search in not successful and clear the search textbox (do not use any built-in array methods) */
        private void ButtonSearch_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TextBoxName.Text))
                {
                    int start = 0;
                    int mid = 0;
                    int end = 12;
                    bool flag = false;
                    string target = TextBoxName.Text;

                    while (!flag && !((end - start) <= 0))
                    {
                        mid = (end + start) / 2;
                        if (target.CompareTo(ArrayDefinitions[mid, 0]) == 0)
                        {
                            flag = true;
                            break;
                        }
                        else
                        {
                            if (target.CompareTo(ArrayDefinitions[mid, 0]) < 0)
                                end = mid;
                            else if (target.CompareTo(ArrayDefinitions[mid, 0]) > 0)
                                start = mid + 1;
                            else
                                break;
                        }
                    }
                    if (flag)
                    {
                        TextBoxCategory.Text = ArrayDefinitions[mid, 1];
                        TextBoxStructure.Text = ArrayDefinitions[mid, 2];
                        RichTextBoxDefinition.Text = ArrayDefinitions[mid, 3];
                    }
                    else
                    {
                        MessageBox.Show("Search target not found", "Search failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ErrorMessage(11);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ErrorMessage(11);
            }
        }
        private void ButtonClear_MouseClick(object sender, MouseEventArgs e)
        {
            ClearTextBoxes();
            ErrorMessage(1);
        }
        /* 9.10 Create a SAVE button so the information from the 2D array can be written into a binary file
         called definitions.dat which is sorted by Name, ensure the user has the option to select an alternative file.
         Use a file stream and BinaryWriter to create the file.*/
        private void ButtonSave_MouseClick(object sender, MouseEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "bin file|*.bin";
            saveFileDialog.Title = "Save a BIN file";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.DefaultExt = "bin";
            saveFileDialog.ShowDialog();
            string fileName = saveFileDialog.FileName;
            if (saveFileDialog.FileName != "")
            {
                SaveRecord(fileName);
                ErrorMessage(6);
            }
            else
            {
                SaveRecord(initialFileName);
                ErrorMessage(6);
            }
        }
        /* 9.11 Create a LOAD button that will read the information from a binary file called
        definitions.dat into the 2D array, ensure the user has the option to select an alternative file.
        Use a file stream and BinaryReader to complete this task.*/
        private void ButtonOpen_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "BIN FILES|*.bin";
            openFileDialog.Title = "Open a BIN file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenRecord(openFileDialog.FileName);
                ErrorMessage(8);
            }
        }
        #endregion Buttons
    }
}
