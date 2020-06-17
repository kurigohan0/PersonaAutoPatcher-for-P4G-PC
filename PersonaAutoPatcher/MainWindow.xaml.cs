using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Security.Principal;


namespace PersonaAutoPatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pacName = "data00004.pac";
        private string gamePath = @"D:\SteamLibrary\steamapps\common\Day";

        private Process quickbmcProcess;

        public MainWindow()
        {
            InitializeComponent();
            CheckFiles();
            try
            {
                using (StreamReader fs = new StreamReader(Environment.CurrentDirectory + @"\settings.txt"))
                {
                    pacName = fs.ReadLine();
                    gamePath = fs.ReadLine();
                }
            }
            catch
            {
                MessageBox.Show("Path or file from settings not found.");
                this.Close();
            }

            CurrentFile.Content = $"Selected .pac file: {pacName}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsAdministrator())
            {
                try
                {
                    StatusLabel.Content = "Creating backup...";
                    File.Copy(gamePath + @"\" + pacName,
                        Environment.CurrentDirectory + @"\files\original_pac\" + pacName, true);
                    StatusLabel.Content = "Backup created.";
                    quickbmcProcess = new Process();
                    quickbmcProcess.StartInfo.FileName =
                        Environment.CurrentDirectory + @"/files/quickbms/quickbms.exe";
                    quickbmcProcess.StartInfo.Arguments =
                        $@"-w -r -r {Environment.CurrentDirectory}\files\quickbms\neptunia_rebirth1.bms {gamePath}\{pacName} {gamePath}\{pacName.Remove(pacName.Length - 4, 4)}\";
                    quickbmcProcess.StartInfo.WindowStyle =
                        ProcessWindowStyle.Normal;
                    quickbmcProcess.Start();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    StatusLabel.Content = "ERROR";
                }
            }
            else
            {
                MessageBox.Show("The program must be run as administrator.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = "Restoring backup...";
            try
            {
                File.Copy(Environment.CurrentDirectory + @"\files\original_pac\" + pacName, gamePath + @"\" + pacName,
                    true);
                StatusLabel.Content = "Backup restored...";
            }
            catch (Exception exception)
            {
                StatusLabel.Content = "Backup not found!";
            }
        }

        static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                .IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void CheckFiles()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\files\original_pac\" + pacName))
            {
                StatusLabel.Content = "Backup not found!";
            }
            else
            {
                StatusLabel.Content = $"Backup of {pacName} found.";
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                quickbmcProcess = new Process();
                quickbmcProcess.StartInfo.FileName =
                    Environment.CurrentDirectory + @"/files/quickbms/quickbms.exe";
                quickbmcProcess.StartInfo.Arguments =
                    $@"-w {Environment.CurrentDirectory}\files\quickbms\neptunia_rebirth1.bms {gamePath}\{pacName} {gamePath}\{pacName.Remove(pacName.Length - 4, 4)}\";
                quickbmcProcess.StartInfo.WindowStyle =
                    ProcessWindowStyle.Normal;
                quickbmcProcess.Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                StatusLabel.Content = "ERROR";
            }
        }
    }
}
