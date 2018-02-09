using System;
using System.IO;
using System.Windows.Forms;

namespace VersionChecker
{
    public partial class Form1 : Form
    {
        private const string FileName1 = "AssemblyInfo.cs";
        private const string SearchString = "AssemblyVersion";
        private const string SearchString1 = "AssemblyFileVersion";
        private const string OutputFile = @"listfile.txt";
        private const string PropertiesFld = "properties";
        private const string Tab = "\t";
        private const string Contents = "Project name\tAssembly Versione\tFile Version\tLast mod\r\n";
        private const string FilterPrj = "*.csproj";
        private const string FilterDocument = "*.cs";
        private const string LineFeed = "\r\n";
        private const string Value = "DEBUG";
        private const string Value1 = "RELEASE";

        public Form1()
        {
            InitializeComponent();
            lblPath.Text = Properties.Settings.Default.PathSolution;

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Properties.Settings.Default.PathSolution;
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.PathSolution = fbd.SelectedPath;
                Properties.Settings.Default.Save();
                lblPath.Text = fbd.SelectedPath;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string PathName = lblPath.Text;

            File.WriteAllText(Path.Combine(PathName, OutputFile), Contents);

            var files = Function.IterateDirectories(PathName, FilterPrj);
            pbMain.Maximum = files.Count;
            for (int i = 0; i < files.Count; i++)
            {
                pbMain.Value = i;
                var FilesCS = Function.IterateDirectories(Path.GetDirectoryName(files[i]), FilterDocument);
                DateTime LastWrite = DateTime.MinValue;
                pbSecondary.Maximum = FilesCS.Count;
                for (int j = 0; j < FilesCS.Count; j++)
                {
                    pbSecondary.Value = j;

                    if (FilesCS[j].ToUpper().Contains(Value) || FilesCS[j].ToUpper().Contains(Value1))
                        continue;

                    var FileLastWriteDate = File.GetLastWriteTimeUtc(FilesCS[j]);

                    if (LastWrite < FileLastWriteDate)
                        LastWrite = FileLastWriteDate;
                }

                //esporta il nome del progetto e le versioni
                File.AppendAllText(Path.Combine(PathName, OutputFile), Path.GetFileNameWithoutExtension(files[i]) +
                    Tab + Function.GetVersion(Path.Combine(Path.GetDirectoryName(files[i]), PropertiesFld), FileName1, SearchString) +
                    Tab + Function.GetVersion(Path.Combine(Path.GetDirectoryName(files[i]), PropertiesFld), FileName1, SearchString1) +
                    Tab + LastWrite.ToUniversalTime() +
                    LineFeed);
            }

            pbMain.Value = pbMain.Maximum;
            pbSecondary.Value = pbSecondary.Maximum;

            MessageBox.Show("ok");

            pbMain.Value = 0;
            pbSecondary.Value = 0;
        }

    }
}
