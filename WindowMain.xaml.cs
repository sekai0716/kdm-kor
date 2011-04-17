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
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using KingsDamageMeter.Controls;
using KingsDamageMeter.Converters;
using KingsDamageMeter.Forms;
using KingsDamageMeter.Localization;
using KingsDamageMeter.Properties;

namespace KingsDamageMeter
{
    public partial class WindowMain
    {
        public WindowMain()
        {
            InitializeComponent();

            // 이건 윈도7 64bit 아이온 설치 경로입니다.
            string logpath = "";
            try
            {// windows7 64bit
                RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\plaync\\aion_kor");
                logpath = reg.GetValue("basedir").ToString();
                reg.Close();
            }
            catch (Exception e)
            {
                DebugLogger.Write("Win32 Reg 읽는중:오류아님 - " + e.Message);
                try
                {// windows7 32bit, winxp 32bit
                    RegistryKey reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\plaync\\aion_kor");
                    logpath = reg.GetValue("basedir").ToString();
                    reg.Close();
                }
                catch (Exception e1)
                {
                    logpath = "";
                    DebugLogger.Write(e1.Message);
                }
            }
            if( logpath != "") Settings.Default.AionLogPath = logpath + "\\Chat.log";

            // system.ovr 파일 생성
            try
            {
                string straionpath = Settings.Default.AionLogPath;
                straionpath = straionpath.Substring(0, straionpath.Length - 9);
                string strsystemovr = straionpath + "\\system.ovr";
                if (File.Exists(strsystemovr)) File.Delete(strsystemovr);
                FileStream fs = new FileStream(strsystemovr, FileMode.Create);
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.ASCII);
                writer.Write("g_chatlog = \"1\"");
                writer.WriteLine();
                writer.Write("log_IncludeTime = \"1\"");
                writer.WriteLine();
                writer.Write("log_Verbosity = \"1\"");
                writer.WriteLine();
                writer.Write("log_FileVerbosity = \"1\"");
                writer.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("system.ovr파일을 자동으로 생성하지 못하였습니다.\n\n" +
                    "혹시 윈도우7인 경우에는 프로그램에 아이콘에서\n\n" +
                    "[마우스 우측 클릭 우측버튼 클릭]-[관리자 권한으로 실행]을 선택해주시면 해결됩니다.\n\n\n\n" + e.Message);
            }

            // chat.log 새로 생성
            try
            {
                string strsystemovr = Settings.Default.AionLogPath;
                if (File.Exists(strsystemovr))
                {
                } else {
                    FileStream fs = new FileStream(strsystemovr, FileMode.Create);
                    StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.ASCII);
                    writer.Write("SDM Clear-----");
                    writer.WriteLine();
                    writer.Close();
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Chat.log을 자동으로 생성하지 못하였습니다.\n\n" +
                    "혹시 윈도우7인 경우에는 프로그램에 아이콘에서\n\n" +
                    "[마우스 우측 클릭 우측버튼 클릭]-[관리자 권한으로 실행]을 선택해주시면 해결됩니다.\n\n\n\n" + e.Message);
            }
            
            DataContext = new WindowMainData();
        }

        private bool _Dragging;
        private Point _MousePoint;

        #region Window Events

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                CaptureMouse();
                _MousePoint = e.GetPosition(this);
                _Dragging = true;
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
           _Dragging = false;
           ReleaseMouseCapture();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Dragging)
            {
                Point p = e.GetPosition(this);
                Left += p.X - _MousePoint.X;
                Top += p.Y - _MousePoint.Y;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            WindowMainData data = (WindowMainData)DataContext;

            if (data.Players.Count > 0)
            {
                string message = Localization.WindowMainRes.ConfirmCloseMessage;
                string caption = Localization.WindowMainRes.ConfirmCloseCaption;
                MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            data.OnClose();
        }

        #endregion

        #region Control Events

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainContextMenu.IsOpen = true;
        }

        private void ThumbResize_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double width = Width + e.HorizontalChange;
            double height = Height + e.VerticalChange;

            Width = (width < MinWidth) ? MinWidth : width;
            Height = (height < MinHeight) ? MinHeight : height;
        }

        #endregion

        #region MainContextMenu Events

        private void MainContextMenuIgnoreList_Click(object sender, RoutedEventArgs e)
        {
            IgnoreListForm i = new IgnoreListForm();
            i.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            i.TopMost = true;
            i.ShowDialog();
        }

        private void MainContextMenuHelp_Click(object sender, RoutedEventArgs e)
        {
            string path = Settings.Default.HelpFilePath;

            if (File.Exists(path))
            {
                Process.Start(path);
            }

            else
            {
                MessageBox.Show("Cannot find '" + path + "'", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void MainContextMenuAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutForm a = new AboutForm();
            a.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            a.TopMost = true;
            a.ShowDialog();
        }

        private void MainContextMenuLocateLog_Click(object sender, RoutedEventArgs e)
        {
            var d = new System.Windows.Forms.OpenFileDialog();
            //d.InitialDirectory = "C:\\";
            d.Filter = "Chat.log (Chat.log)|Chat.log|All files (*.*)|*.*";

            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ((WindowMainData) DataContext).ChangeLogFile(d.FileName);
            }
        }
        
        private void MenuItemAddGroupMemberByName_Click(object sender, RoutedEventArgs e)
        {
            SetNameDialog d = new SetNameDialog();
            d.Text = WindowMainRes.AddByNameMenuHeader;

            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ((WindowMainData)DataContext).AddPlayer(d.PlayerName, /* isGroupMember = */ true);
            }
        }

        private void MainContextMenuSetYouAlias_Click(object sender, RoutedEventArgs e)
        {
            SetNameDialog d = new SetNameDialog();
            d.Text = WindowMainRes.SetYouAliasMenuHeader;
            d.PlayerName = Settings.Default.YouAlias;

            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (((WindowMainData)DataContext).PlayerExists(d.PlayerName))
                {
                    MessageBox.Show(WindowMainRes.NameTakenMessage, "Oops", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

                else
                {
                    ((WindowMainData)DataContext).Rename(d.PlayerName, Settings.Default.YouAlias);
                    Settings.Default.YouAlias = d.PlayerName;
                }
            }
        }

        private void MainContextMenuAionWebPage_Click(object sender, RoutedEventArgs e) //  아이온 웹사이트 실행
        {
            System.Diagnostics.Process program = System.Diagnostics.Process.Start(@"http://aion.plaync.co.kr");
        }

        #endregion

        private bool isLoaded;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(!isLoaded)
            {
                var menuStyle = (Style) Resources["NotUncheckedMenuItemStyle"];

                //This is for static commands initialization declared in Commands.cs
                playersItemsControl.Focus();
                ////////////////////////////////////////////////////////////////////
                isLoaded = true;
            }
        }

        private void MagicMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary skinDict =
                    Application.LoadComponent(new Uri("/KingsDamageMeter;component/Themes/BlueSky.xaml", UriKind.Relative)) as ResourceDictionary;

            Collection<ResourceDictionary> mergedDicts =
                Application.Current.Resources.MergedDictionaries;

            // Remove the existing skin dictionary, if one exists.

            // NOTE: In a real application, this logic might need

            // to be more complex, because there might be dictionaries

            // which should not be removed.

            if (mergedDicts.Count > 0)
                mergedDicts.Clear();

            // Apply the selected skin so that all elements in the

            // application will honor the new look and feel.

            mergedDicts.Add(skinDict);
        }
    }
}
