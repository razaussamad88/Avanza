using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordBox
{
    public partial class MainUI : Form
    {
        #region Members

        private const string c_DEFAULT_DESCRIPTION = "[ please select a password tool name ]";

        private static Dictionary<string, KeyValuePair<string, string>> m_Description = new Dictionary<string, KeyValuePair<string, string>>();
        private static Dictionary<string, ProcessStartInfo> m_StartedProcesses = new Dictionary<string, ProcessStartInfo>();
        #endregion


        #region Properties

        private string SelectedAppID
        {
            get
            {
                return (this.cmbxToolName.SelectedIndex > -1) ?
                    this.cmbxToolName.Items[this.cmbxToolName.SelectedIndex]?.ToString() :
                    "NULL";
            }
        }
        #endregion


        #region Constructor

        public MainUI()
        {
            InitializeComponent();
            this.setDefaultDescription();
        }
        #endregion


        #region Form Events

        private void MainUI_Load(object sender, EventArgs e)
        {
            ResourceManager resMan = new ResourceManager("PasswordBox.AppResources", Assembly.GetExecutingAssembly());
            AppResources.ResourceManager.IgnoreCase = true;

            ResourceSet resourceSet = resMan.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            this.cmbxToolName.Items.Clear();
            this.cmbxToolName.Items.Add("- SELECT -");

            m_Description.Add("NULL", new KeyValuePair<string, string>("NULL", "NULL"));

            Dictionary<string, object> tmpDict = new Dictionary<string, object>();

            foreach (DictionaryEntry entry in resourceSet)
            {
                string resxKey = entry.Key.ToString();
                object resxValue = entry.Value;

                tmpDict.Add(resxKey, resxValue);
            }

            var keys_sorted = tmpDict.Keys.ToList();
            keys_sorted.Sort();


            foreach (var resxKey in keys_sorted)
            {
                this.cmbxToolName.Items.Add(addItem(resxKey, tmpDict[resxKey]));
            }
        }
        #endregion


        #region Control Events

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (this.cmbxToolName.SelectedIndex > 0)
            {
                //string root = @"D:\$Avanza.Utilities\Avanza.PasswordBox\Avanza.PasswordBox\Password.Binaries\";
                string root = Path.Combine(Directory.GetCurrentDirectory(), @"Password.Binaries");
                string appId = m_Description[SelectedAppID].Key;

                string appPath = Path.Combine(root, appId);
                DirectoryInfo di = new DirectoryInfo(appPath);

                if (!di.Exists)
                {
                    MessageBox.Show(String.Format("Directory Path not found [{0}]", root), "Path Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var files = di.GetFiles("*.exe", SearchOption.AllDirectories);

                string appFullName = null;

                if (files.Length > 0)
                {
                    appFullName = files[0].FullName;
                }
                else
                {
                    MessageBox.Show(String.Format("No executable found for [{0}]", appId), "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    startProcess(appId, appFullName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show(String.Format("{0}", ex.Message), "Exception Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BgWrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs eventArgs)
        {
            // First, handle the case where an exception was thrown.
            if (eventArgs.Error != null)
            {
                MessageBox.Show(eventArgs.Error.Message);
            }
            else if (eventArgs.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.
                //resultLabel.Text = "Canceled";
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                //resultLabel.Text = eventArgs.Result.ToString();

                var args = (string[])eventArgs.Result;
                string appId = args[0];
                string appFullName = args[1];

                if (m_StartedProcesses.ContainsKey(appId))
                {
                    //m_StartedProcesses[appId].FileName;
                    //Path.GetFileName(FileName);
                    var allProc = Process.GetProcesses();

                    m_StartedProcesses.Remove(appId);
                }
            }
        }

        private void BgWrk_DoWork(object sender, DoWorkEventArgs eventArgs)
        {
            string appId = null;
            string appFullName = null;

            try
            {
                var args = (string[])eventArgs.Argument;

                appId = args[0];
                appFullName = args[1];

                using (Process proc = new Process())
                {
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.FileName = appFullName;
                    proc.StartInfo.CreateNoWindow = true;

                    proc.StartInfo.Domain = "";
                    proc.StartInfo.LoadUserProfile = false;
                    proc.StartInfo.Password = null;
                    proc.StartInfo.StandardErrorEncoding = null;
                    proc.StartInfo.StandardOutputEncoding = null;
                    proc.StartInfo.UserName = "";
                    proc.SynchronizingObject = this;

                    if (proc.Start())
                    {
                        if (!m_StartedProcesses.ContainsKey(appId))
                        {
                            m_StartedProcesses.Add(appId, proc.StartInfo);
                        }

                        proc.WaitForExit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //var obj = new String[2];
                //obj[0] = appId;
                //obj[1] = appFullName;
                eventArgs.Result = eventArgs.Argument;
            }
        }

        private void cmbxToolName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.txtDesc.Font.Italic)
            {
                this.txtDesc.Font = new Font(this.txtDesc.Font, FontStyle.Regular);
            }

            this.btnRun.Enabled = this.cmbxToolName.SelectedIndex > 0;

            if (this.btnRun.Enabled)
                this.txtDesc.Text = m_Description[SelectedAppID].Value.ToString();
            else
                this.setDefaultDescription();
        }

        private void tsMnuAbout_Click(object sender, EventArgs e)
        {
            frmAboutBox frm1 = new frmAboutBox();
            frm1.ShowDialog();
        }
        #endregion


        #region Private Methods

        private string addItem(string resxKey, object resxValue)
        {
            int index = m_Description.Keys.Count;
            string cmbxKey = String.Format("{0:00}. {1}", index, resxKey);

            string txtDesc = (resxValue == null) ? String.Empty : String.Concat(Environment.NewLine, resxValue.ToString().Replace(@"\r\n", Environment.NewLine));

            if (!m_Description.ContainsKey(cmbxKey))
                m_Description.Add(cmbxKey, new KeyValuePair<string, string>(resxKey, txtDesc));

            return cmbxKey;
        }

        private void startProcess(string appId, string appFullName)
        {
            if (m_StartedProcesses.ContainsKey(appId))
            {
                return;
            }


            Task.Run(() =>
            {

            });

            BackgroundWorker bgWrk = new BackgroundWorker();
            bgWrk.DoWork += BgWrk_DoWork;
            bgWrk.RunWorkerCompleted += BgWrk_RunWorkerCompleted;

            try
            {
                var obj = new String[2];
                obj[0] = appId;
                obj[1] = appFullName;
                bgWrk.RunWorkerAsync(obj);
            }
            catch (Exception ex)
            {
                //ActivityLogger.Instance.Log(LogLevel.Error, MethodBase.GetCurrentMethod(), typeof(EmailSender), ex.Message, ex);
            }
        }

        private void setDefaultDescription()
        {
            this.txtDesc.Font = new Font(this.txtDesc.Font, FontStyle.Italic);
            this.txtDesc.Text = String.Concat(Environment.NewLine, c_DEFAULT_DESCRIPTION);
        }
        #endregion
    }
}
