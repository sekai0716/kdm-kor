/**************************************************************************\
 * 
    This file is part of KingsDamageMeter.

    KingsDamageMeter is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    KingsDamageMeter is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with KingsDamageMeter. If not, see <http://www.gnu.org/licenses/>.
 * 
\**************************************************************************/

using System;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using KingsDamageMeter.Forms;
using KingsDamageMeter.Localization;
using KingsDamageMeter.Properties;
using WPFLocalizeExtension.Engine;
using KingsDamageMeter.Windows;

namespace KingsDamageMeter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            DebugLogger.Write(Environment.NewLine + "Started KDM " + DateTime.Now.ToString());

            NotifyIcon.Icon = KingsDamageMeter.Properties.Resources.Lion;
            //NotifyIcon.Show();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;

                ExceptionForm f = new ExceptionForm();
                string exception = ex.Message + Environment.NewLine + ex.StackTrace;
                DebugLogger.Write("Unhandled Exception: " + exception);
                f.Exception = exception;
                f.ShowDialog();
            }

            finally
            {
                Application.Current.Shutdown();
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if DEBUG
            Settings.Default.Debug = true;
#endif

            if (Settings.Default.IgnoreList == null)
            {
                Settings.Default.IgnoreList = new ObservableCollection<string>();
            }
            if (Settings.Default.FriendList == null)
            {
                Settings.Default.FriendList = new ObservableCollection<string>();
            }

            Settings.Default.PropertyChanged += SettingsChanged;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //NotifyIcon.Hide();
            Settings.Default.Save();

            // 환경변수 저장
            classINI clsINI = new classINI();
            clsINI.SetInIValue("viewContents", "YouAlias", Settings.Default.YouAlias, System.Environment.CurrentDirectory + "\\SDM.cfg");
            clsINI.SetInIValue("viewContents", "SortType", Settings.Default.SortType.ToString(), System.Environment.CurrentDirectory + "\\SDM.cfg");
            clsINI.SetInIValue("viewContents", "DisplayType", Settings.Default.DisplayType.ToString(), System.Environment.CurrentDirectory + "\\SDM.cfg");
            
            // system.ovr파일 0으로 변경
            try
            {
                string straionpath = Settings.Default.AionLogPath;
                straionpath = straionpath.Substring(0, straionpath.Length - 9);
                string strsystemovr = straionpath + "\\system.ovr";
                if (File.Exists(strsystemovr)) File.Delete(strsystemovr);
                FileStream fs = new FileStream(strsystemovr, FileMode.Create);
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.ASCII);
                writer.Write("g_chatlog = \"0\"");
                writer.WriteLine();
                writer.Write("log_IncludeTime = \"0\"");
                writer.WriteLine();
                writer.Write("log_Verbosity = \"0\"");
                writer.WriteLine();
                writer.Write("log_FileVerbosity = \"0\"");
                writer.Close();
                fs.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show("system.ovr파일을 변경하지 못했습니다.\n\n" +
                    "혹시 윈도우7인 경우에는 프로그램에 아이콘에서\n\n" +
                    "[마우스 우측 클릭 우측버튼 클릭]-[관리자 권한으로 실행]을 선택해주시면 해결됩니다.\n\n\n\n" + e1.Message);
            }
            try
            {
                if (File.Exists(Settings.Default.AionLogPath)) File.Delete(Settings.Default.AionLogPath);
            }
            catch (Exception e1)
            {
                MessageBox.Show("chat.log 파일을 삭제하지 못했습니다.\n\n" +
                    "혹시 윈도우7인 경우에는 프로그램에 아이콘에서\n\n" +
                    "[마우스 우측 클릭 우측버튼 클릭]-[관리자 권한으로 실행]을 선택해주시면 해결됩니다.\n\n\n\n" + e1.Message);
            }
            base.OnExit(e);
        }

        private void SettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsHideOthers")
            {
                if(Settings.Default.IsHideOthers)
                {
                    Settings.Default.IsGroupOnly = false;
                }
            }
            else if (e.PropertyName == "IsGroupOnly")
            {
                if (Settings.Default.IsGroupOnly)
                {
                    Settings.Default.IsHideOthers = false;
                }
            }
        }

    }
}
