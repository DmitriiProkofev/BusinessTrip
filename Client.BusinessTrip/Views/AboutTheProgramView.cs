using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.BusinessTrip.Views
{
    public partial class AboutTheProgramView : Form
    {
        #region Constructors
        public AboutTheProgramView()
        {
            InitializeComponent();

            ////labelVersion.Text = string.Format("Версия {0}", AssemblyVersion.ToString(4));

            //Assembly ass = Assembly.GetExecutingAssembly();
            //string version;
            //if (ass != null)
            //{
            //    FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(ass.Location);
            //    version = String.Format("{0} Version ({1:0}.{2:0}.{3})",
            //                  FVI.ProductName,
            //                  FVI.FileMajorPart.ToString(),
            //                  FVI.FileMinorPart.ToString(),
            //                  FVI.FileBuildPart.ToString()
            //                  );


            //}

            //else
            //{
            //    version = "Unknown";
            //}

            //var srt = Application.ProductVersion.ToString();

            //var djdjd = VersionLabel;

            //var err = 0;
            labelVersion.Text = string.Format("Версия {0}", VersionLabel);
        }

        #endregion //Constructors

        #region Property

        public string VersionLabel
        {
            get
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    return string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
                }
                else
                {
                    var ver = Assembly.GetExecutingAssembly().GetName().Version;
                    return string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
                }
            }
        }

        #endregion //Property

        #region EventHandlers

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutTheProgramView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        #endregion //EventHandlers
    }
}
