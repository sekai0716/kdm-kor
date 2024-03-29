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
using System.Globalization;
using System.Windows.Data;
using KingsDamageMeter.Controls;

namespace KingsDamageMeter.Converters
{
    public class DataToFormattedDataConverter : IValueConverter
    {
        public static DataToFormattedDataConverter Instance { get; private set; }
        static DataToFormattedDataConverter()
        {
            Instance = new DataToFormattedDataConverter();
        }

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is DisplayType)
            {
                var val = System.Convert.ToDouble(value);
                if (val == 0)
                {
                    return string.Empty;
                }
                switch ((DisplayType)parameter)
                {
                    case DisplayType.Damage:
                    case DisplayType.DamagePerSecond:
                    case DisplayType.Experience:
                    case DisplayType.Kinah:
                    case DisplayType.AbyssPoints:
                        return val.ToString("#,#");
                    case DisplayType.Percent:
                        return val.ToString("0%");
                    case DisplayType.FightTime:
                        return val.ToString("#");
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}