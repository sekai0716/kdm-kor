﻿/**************************************************************************\
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

            //Settings.Default.PropertyChanged += SettingsChanged;
            // SDM.cfg로 대체
            classINI clsINI = new classINI();
            string strFileName = System.Environment.CurrentDirectory + "\\SDM.cfg";

            try
            {
                Settings.Default.WindowMainHeight = int.Parse(clsINI.GetIniValue("System", "WindowMainHeight", strFileName));
                Settings.Default.WindowMainOpacity = double.Parse(clsINI.GetIniValue("System", "WindowMainOpacity", strFileName));
                Settings.Default.WindowMainTopMost = bool.Parse(clsINI.GetIniValue("System", "WindowMainTopMost", strFileName));
                Settings.Default.WindowMainWidth = int.Parse(clsINI.GetIniValue("System", "WindowMainWidth", strFileName));
                Settings.Default.WindowMainX = int.Parse(clsINI.GetIniValue("System", "WindowMainX", strFileName));
                Settings.Default.WindowMainY = int.Parse(clsINI.GetIniValue("System", "WindowMainY", strFileName));
                Settings.Default.Debug = bool.Parse(clsINI.GetIniValue("System", "Debug", strFileName));
                Settings.Default.IsGroupOnly = bool.Parse(clsINI.GetIniValue("AppData", "IsGroupOnly", strFileName));
                Settings.Default.IsHideOthers = bool.Parse(clsINI.GetIniValue("AppData", "IsHideOthers", strFileName));
                Settings.Default.LogTimeFormat = clsINI.GetIniValue("AppData", "LogTimeFormat", strFileName);
                string strSortType = clsINI.GetIniValue("AppData", "SortType", strFileName);
                if (strSortType == "Damage")
                {
                    Settings.Default.SortType = Controls.PlayerSortType.Damage;
                }
                else if (strSortType == "DamagePerSecond")
                {
                    Settings.Default.SortType = Controls.PlayerSortType.DamagePerSecond;
                }
                else if (strSortType == "Name")
                {
                    Settings.Default.SortType = Controls.PlayerSortType.Name;
                }
                else
                {
                    Settings.Default.SortType = Controls.PlayerSortType.None;
                }
                Settings.Default.YouAlias = clsINI.GetIniValue("AppData", "YouAlias", strFileName);
                string strdisplaytype = clsINI.GetIniValue("AppData", "DisplayType", strFileName);
                foreach (var display in strdisplaytype.Split(','))
                {
                    if (display.Trim() == "Damage")
                    {
                        Settings.Default.DisplayType |= Controls.DisplayType.Damage;
                    }
                    else if (display.Trim() == "AbyssPoints")
                    {
                        Settings.Default.DisplayType |= Controls.DisplayType.AbyssPoints;
                    }
                    else if (display.Trim() == "DamagePerSecond")
                    {
                        Settings.Default.DisplayType |= Controls.DisplayType.DamagePerSecond;
                    }
                    else if (display.Trim() == "Experience")
                    {
                        Settings.Default.DisplayType |= Controls.DisplayType.Experience;
                    }
                    else if (display.Trim() == "Kinah")
                    {
                        Settings.Default.DisplayType |= Controls.DisplayType.Kinah;
                    }
                    else if (display.Trim() == "Percent")
                    {
                        Settings.Default.DisplayType |= Controls.DisplayType.Percent;
                    }
                }


            }
            catch (Exception setError)
            {
                Settings.Default.PropertyChanged += SettingsChanged;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //NotifyIcon.Hide();
            //Settings.Default.Save();
            // 환경변수 저장
            classINI clsINI = new classINI();
            string strFileName = System.Environment.CurrentDirectory + "\\SDM.cfg";

            clsINI.SetInIValue("System", "WindowMainHeight", Settings.Default.WindowMainHeight.ToString(), strFileName);
            clsINI.SetInIValue("System", "WindowMainOpacity", Settings.Default.WindowMainOpacity.ToString(), strFileName);
            clsINI.SetInIValue("System", "WindowMainTopMost", Settings.Default.WindowMainTopMost.ToString(), strFileName);
            clsINI.SetInIValue("System", "WindowMainWidth", Settings.Default.WindowMainWidth.ToString(), strFileName);
            clsINI.SetInIValue("System", "WindowMainX", Settings.Default.WindowMainX.ToString(), strFileName);
            clsINI.SetInIValue("System", "WindowMainY", Settings.Default.WindowMainY.ToString(), strFileName);
            clsINI.SetInIValue("System", "Debug", Settings.Default.Debug.ToString(), strFileName);
            clsINI.SetInIValue("AppData", "IsGroupOnly", Settings.Default.IsGroupOnly.ToString(), strFileName);
            clsINI.SetInIValue("AppData", "IsHideOthers", Settings.Default.IsHideOthers.ToString(), strFileName);
            clsINI.SetInIValue("AppData", "LogTimeFormat", Settings.Default.LogTimeFormat.ToString(), strFileName);
            clsINI.SetInIValue("AppData", "SortType", Settings.Default.SortType.ToString(), strFileName);
            clsINI.SetInIValue("AppData", "YouAlias", Settings.Default.YouAlias.ToString(), strFileName);
            clsINI.SetInIValue("AppData", "DisplayType", Settings.Default.DisplayType.ToString(), strFileName);

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
