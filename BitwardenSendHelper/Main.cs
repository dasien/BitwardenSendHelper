using System.Diagnostics.Eventing.Reader;
using BitwardenSendHelper.Utils;
using BitwardenSendHelper.Config;
using BitwardenSendHelper.Models;
using System.IO.Compression;

namespace BitwardenSendHelper
{
    public partial class Main : Form
    {
        // Vault proxy reference.
        private readonly CLIProxy _cliProxy;

        public Main(string[] args)
        {
            InitializeComponent();
            
            // Load the configuration file.
            ConfigurationManager manager = ConfigurationManager.Instance;

            // Initialize cli proxy.
            _cliProxy = new CLIProxy(manager.GetConfig().CLILocation);

            // Event handlers.
            this.DragEnter += new DragEventHandler(frmMain_DragEnter);
            this.DragDrop += new DragEventHandler(frmMain_DragDrop);
            this.loginToolStripMenuItem.Click += LoginToolStripMenuItemOnClick;
            this.quitToolStripMenuItem.Click += QuitToolStripMenuItemOnClick;
            
            // Check to see if a file was passed as a parameter.
            if (args.Length == 1)
            {
                // Get the file name.
                string fileName = args[0];
                
                // Try to create the send for the file.
                SendResponse response = _cliProxy.SendFile(fileName, null, null, null, false, "Test");

                // Check to see if it worked.
                if (_cliProxy.ExitCode == "0")
                {
                    // Copy the link to the clipboard.
                    Clipboard.SetText(response.AccessUrl);
                    MessageBox.Show("Link to Send has been copied to clipboard");
                }

                else
                {
                    using (StreamWriter writer = File.AppendText("\"C:\\Users\\bgent\\Downloads\\log.txt"))
                    {
                        writer.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                        writer.WriteLine("  :");
                        writer.WriteLine($"  :{_cliProxy.ErrorMessage}");
                        writer.WriteLine ("-------------------------------");
                    }
                }
            }
        }

        private void QuitToolStripMenuItemOnClick(object? sender, EventArgs e)
        { 
            // Check to see if we are logged in.
            if (_cliProxy.IsLoggedIn)
            {
                // Call logout.
                _cliProxy.Logout();
                
                // Check to see if the login was successful.
                if (_cliProxy.ExitCode == "0")
                {
                    MessageBox.Show("Logout Successful");
                }
            }
            
            Application.Exit();
        }

        private void LoginToolStripMenuItemOnClick(object? sender, EventArgs e)
        {
            // Create login form.
            Login frmLogin = new Login();
            frmLogin.ShowDialog();
            
            // Check to see if they pressed ok.
            if (frmLogin.DialogResult == DialogResult.OK)
            {
                // Try to login.
                _cliProxy.Login(frmLogin.UserId, frmLogin.Password);
                
                // Check to see if the login was successful.
                if (_cliProxy.ExitCode == "0")
                {
                    MessageBox.Show("Login Successful");
                }
            }
        }

        void frmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        void frmMain_DragDrop(object sender, DragEventArgs e)
        {
            // The name of the file to send.
            string fileName = "";
            
            // Check to see if the user is logged in.
            if (_cliProxy.IsLoggedIn)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                
                // Check to see if more than one file was dropped.
                if (files.Length > 1)
                {
                    // Get the path of the file.
                    string directoryName = Path.GetDirectoryName(files[1]);

                    // Zip files and save to directory.
                    using (ZipArchive archive = ZipFile.Open(directoryName + "\\FilesToSend.zip", ZipArchiveMode.Create))
                    {
                        // Loop through the files.
                        foreach (string file in files)
                        {
                            archive.CreateEntryFromFile(file, Path.GetFileName(file));
                        }
                    }
                    
                    // Set the file name.
                    fileName = directoryName + "\\FilesToSend.zip";
                }

                else
                {
                    // Get the file name.
                    fileName = files[0];

                    // Check to see if this is a file or a directory.
                    FileAttributes attrs = File.GetAttributes(fileName);

                    if (attrs.HasFlag(FileAttributes.Directory))
                    {
                        DirectoryInfo d = new DirectoryInfo(fileName);
                    
                        // Zip files and save to directory.
                        using (ZipArchive archive = ZipFile.Open(d.Parent.FullName + "\\FilesToSend.zip", ZipArchiveMode.Create))
                        {
                            FileInfo[] finfo = d.GetFiles("*");

                            // Loop through the files.
                            foreach (FileInfo file in finfo)
                            {
                                archive.CreateEntryFromFile(file.FullName, file.Name);
                            }
                        }
                        
                        // Update file name to be sent.
                        fileName = d.Parent.FullName + "\\FilesToSend.zip";
                    }
                }
                
                // Try to create the send for the file.
                SendResponse response = _cliProxy.SendFile(fileName, null, null, null, false, "Test");

                // Check to see if it worked.
                if (_cliProxy.ExitCode == "0")
                {
                    // Copy the link to the clipboard.
                    Clipboard.SetText(response.AccessUrl);
                    MessageBox.Show("Link to Send has been copied to clipboard");
                }
            }

            else
            {
                MessageBox.Show("You must be logged in to drop files.");
            }
        }
    }
}
