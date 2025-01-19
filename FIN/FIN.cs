using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace FIN
{
    public partial class FIN : Form
    {
        public FIN()
        {
            InitializeComponent();
            btnGet.Enabled = false;
            btnExport.Enabled = false;
            btnClear.Enabled = false;
            rbNames.Enabled = false;
            rbPath.Enabled = false;
            cbExtension.Enabled = false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    tbPath.Text = folderDialog.SelectedPath;
                    btnGet.Enabled = !string.IsNullOrWhiteSpace(tbPath.Text);
                    btnClear.Enabled = true;
                    btnClear.BackColor = Color.FromArgb(255, 128, 128);

                    // Enable checkboxes and radio buttons after selecting a folder
                    cbExtension.Enabled = true;
                    rbPath.Enabled = true;
                    rbNames.Enabled = true;
                    rbNames.Checked = true;
                    rbNames.Enabled = true;
                    rbPath.Enabled = true;
                    cbExtension.Enabled = true;
                }
            }
        }


        private void btnGet_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(tbPath.Text))
            {
                string[] items = Directory.GetFileSystemEntries(tbPath.Text);
                if (items.Length > 0)
                {
                    List<string> fileNames = new List<string>();

                    foreach (var item in items)
                    {
                        string fileName = "";  // Initialize the fileName variable

                        // Check if user selected to extract full path or just file names
                        if (rbPath.Checked)
                        {
                            // If rbPath is checked, add the full file path
                            fileName = item;

                            if (!cbExtension.Checked)
                            {
                                // If cbExtension is unchecked, remove the extension from the full path
                                fileName = Path.Combine(Path.GetDirectoryName(item), Path.GetFileNameWithoutExtension(item));
                            }
                        }
                        else if (rbNames.Checked)
                        {
                            // If rbNames is checked, get just the file name (without path)
                            fileName = Path.GetFileName(item);

                            if (!cbExtension.Checked)
                            {
                                // If cbExtension is unchecked, remove the extension
                                fileName = Path.GetFileNameWithoutExtension(item);
                            }
                        }

                        fileNames.Add(fileName);  // Add the processed file name/path to the list
                    }

                    // Set the lines of the rich text box with the processed file names/paths
                    rtbNames.Lines = fileNames.ToArray();

                    // Enable buttons after processing
                    btnExport.Enabled = true;
                    btnClear.Enabled = true;
                    btnClear.BackColor = Color.FromArgb(255, 128, 128);
                }
                else
                {
                    rtbNames.Text = "<EMPTY FOLDER>";
                    btnExport.Enabled = false;
                    btnClear.Enabled = true;
                    btnClear.BackColor = Color.FromArgb(255, 128, 128);
                }
            }
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(rtbNames.Text) && rtbNames.Text != "<EMPTY FOLDER>")
            {
                string folderName = new DirectoryInfo(tbPath.Text).Name;
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Text Files (*.txt)|*.txt";
                    saveDialog.FileName = folderName + "Items.txt";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        List<string> fileNamesToExport = new List<string>();

                        // Iterate over the file names in the rich text box and apply the logic for extensions and paths
                        foreach (var line in rtbNames.Lines)
                        {
                            string fullPath = Path.Combine(tbPath.Text, line); // Rebuild the full path

                            if (rbPath.Checked)
                            {
                                // If rbPath is checked, keep the full path
                                if (!cbExtension.Checked)
                                {
                                    // If cbExtension is unchecked, remove the extension from the full path
                                    fullPath = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath));
                                }
                            }
                            else if (rbNames.Checked)
                            {
                                // If rbNames is checked, get the file name only
                                string fileName = Path.GetFileName(fullPath);

                                if (!cbExtension.Checked)
                                {
                                    // If cbExtension is unchecked, remove the extension from the file name
                                    fileName = Path.GetFileNameWithoutExtension(fullPath);
                                }

                                fullPath = fileName;  // Set fullPath to just the file name
                            }

                            fileNamesToExport.Add(fullPath);  // Add the processed path/name to the list
                        }

                        // Write the processed file names/paths to the export file
                        File.WriteAllLines(saveDialog.FileName, fileNamesToExport);
                        MessageBox.Show("File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbNames.Clear();
            tbPath.Clear();
            btnGet.Enabled = false;
            btnExport.Enabled = false;
            btnClear.Enabled = false;
            btnClear.BackColor = Color.FromArgb(255, 192, 192);
            rbNames.Checked = false;
            rbPath.Checked = false;
            cbExtension.Checked = false;
            cbExtension.Enabled = false;
            rbPath.Enabled = false; 
            rbNames.Enabled = false; 
        }


        private void pbGithub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/muhameho",
                UseShellExecute = true
            });
        }

    }
}
