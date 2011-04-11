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
using System.Collections.Generic;
using KingsDamageMeter.Localization;

namespace KingsDamageMeter
{
    public static class SkillDictionary
    {
        private static Dictionary<string, ClassType> _Skills = new Dictionary<string, ClassType>();
        private static char[] _Numerals = { ' ', 'I', 'V', 'X' };

        public static ClassType GetClass(string skill)
        {
            if (_Skills.Count < 1)
            {
                PopulateDictionary();
            }

            skill = skill.TrimEnd(_Numerals);

            if (_Skills.ContainsKey(skill))
            {
                return _Skills[skill];
            }
            else
            {
                return ClassType.알수없음;
            }
        }

        private static void PopulateFromArray(string[] skills, ClassType classType)
        {
            foreach(string skill in skills)
            {
                if (!_Skills.ContainsKey(skill))
                {
                    string s = skill.Trim();
                    _Skills.Add(s, classType);
                }
            }
        }

        private static void PopulateDictionary()
        {
            char[] sep = new char[] { ',' };
            PopulateFromArray(SkillLists.치유성.Split(sep, StringSplitOptions.RemoveEmptyEntries), ClassType.치유성);
            PopulateFromArray(SkillLists.호법성.Split(sep, StringSplitOptions.RemoveEmptyEntries), ClassType.호법성);
            PopulateFromArray(SkillLists.살성.Split(sep, StringSplitOptions.RemoveEmptyEntries), ClassType.살성);
            PopulateFromArray(SkillLists.궁성.Split(sep, StringSplitOptions.RemoveEmptyEntries), ClassType.궁성);
            PopulateFromArray(SkillLists.수호성.Split(sep, StringSplitOptions.RemoveEmptyEntries), ClassType.수호성);
            PopulateFromArray(SkillLists.검성.Split(sep, StringSplitOptions.RemoveEmptyEntries), ClassType.검성);
            PopulateFromArray(SkillLists.마도성.Split(sep, StringSplitOptions.RemoveEmptyEntries), ClassType.마도성);
            PopulateFromArray(SkillLists.정령성.Split(sep, StringSplitOptions.RemoveEmptyEntries), ClassType.정령성);
        }
    }
}

