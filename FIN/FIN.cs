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
                    // Extract just the item names (without the path)
                    rtbNames.Lines = items.Select(item => Path.GetFileName(item)).ToArray();
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
                        // Only write item names to the file (not the full path)
                        File.WriteAllLines(saveDialog.FileName, rtbNames.Lines);
                        MessageBox.Show("File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtbNames.Clear();
                        tbPath.Clear();
                        btnGet.Enabled = false;
                        btnExport.Enabled = false;
                        btnClear.Enabled = false;
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
