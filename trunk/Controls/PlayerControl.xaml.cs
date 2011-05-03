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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.ComponentModel;
using KingsDamageMeter.Forms;
using KingsDamageMeter.Localization;
using KingsDamageMeter.Properties;

namespace KingsDamageMeter.Controls
{
    /// <summary>
    /// A class that represents a player control.
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        /// <summary>
        /// A class that represents a player control.
        /// </summary>

        static SkillsForm skilldlg;

        public PlayerControl()
        {
            InitializeComponent();
        }

        private void MenuItemViewSkills_Click(object sender, RoutedEventArgs e)
        {
            if (skilldlg == null)
            {
                skilldlg = new SkillsForm();
            }
            else
            {
                skilldlg.Dispose();
                skilldlg = new SkillsForm();
            }

            skilldlg.columnHeader1.Width = Settings.Default.WindowSkillListColumn1Width;
            skilldlg.columnHeader2.Width = Settings.Default.WindowSkillListColumn2Width;
            skilldlg.columnHeader3.Width = Settings.Default.WindowSkillListColumn3Width;
            skilldlg.columnHeader4.Width = Settings.Default.WindowSkillListColumn4Width;
            skilldlg.Size = new System.Drawing.Size(Settings.Default.WindowSkillListWidth, Settings.Default.WindowSkillListHeight);
            skilldlg.Text = string.Format(SkillsFormRes.WindowTitle, ((Player)DataContext).PlayerName);
            skilldlg.Populate(((Player)DataContext).Skills, ((Player)DataContext).Damage);
            skilldlg.Show();
        }
    }
}
