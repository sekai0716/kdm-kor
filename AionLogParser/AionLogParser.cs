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
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using KingsDamageMeter.Properties;

namespace KingsDamageMeter
{
    /// <summary>
    /// 
    /// </summary>
    public class AionLogParser : IAionLogParser
    {
        public AionLogParser()
        {
            Initialize();
            _OldYouAlias = Settings.Default.YouAlias;
            Settings.Default.PropertyChanged += SettingsChanged;
        }

        private void SettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "YouAlias")
            {
                //Here we change name of old YouAlias
                foreach (var pet in _Pets)
                {
                    if (pet.Value == _OldYouAlias)
                    {
                        _Pets[pet.Key] = Settings.Default.YouAlias;
                        _OldYouAlias = Settings.Default.YouAlias;
                        break;
                    }
                }
            }
        }

        private bool _Running = false;
        private FileStream _FileStream;
        private StreamReader _StreamReader;

        private string _TimeFormat = KingsDamageMeter.Properties.Settings.Default.LogTimeFormat;

        private Dictionary<string, string> _Dots = new Dictionary<string, string>();
        private Dictionary<string, string> _Pets = new Dictionary<string, string>();
        private Dictionary<string, string> _Effects = new Dictionary<string, string>();
        private Dictionary<string, string> _Energy = new Dictionary<string, string>();

        private Thread _Worker;
        private object _LockObject = new object();

        private string _OldYouAlias;

        private string _LogPath = String.Empty;

        private string _NameGroupName = "name";
        private string _DamageGroupName = "damage";
        private string _SkillGroupName = "skill";
        private string _PetGroupName = "pet";
        private string _TimeGroupName = "time";
        private string _TargetGroupName = "target";
        private string _EffectGroupName = "effect";
        private string _ExpGroupName = "exp";
        private string _KinahGroupName = "kinah";
        private string _ApGroupName = "ap";

        private string _TimestampRegex;
        private Regex _ChatRegex;

        // 대미지 관련
        private Regex _GodStoneAttrDamageRegex;     // 신석대미지 - 자신,타인 대미지
        private Regex _GodStonePoisonRegex;         // 신석대미지 - 중독
        private Regex _CommandRegex;                // 정령성 : 정령의 명령 스킬 대미지
        private Regex _EnergySummonedRegex;
        private Regex _CommonSummonedRegex;         // 정령류
        private Regex _CommonSummonedSumAtk;
        private Regex _InflictedSkillRegex;         // 스킬 사용 대미지
        private Regex _InflictedRegex;              // 일반적인 대미지
        private Regex _ContinuousRegex;             // 일반적인 도트 추가
        private Regex _AddDamageRegex;              // 추가 대미지
        private Regex _ContinuousDamage;            // 일반적인 도트 대미지
        private Regex _DelayedRegex;                // 지연폭발, 태풍소환류 도트 추가
        private Regex _EffectRegex;                 // 자체 공격대미지 주는 버프 메세지 (폭약,바람의약속 등)
        private Regex _EffectDamageRegex;           // ex)호법성 자체 버프-바람의 약속

        // 기타
        private Regex _CommonSummonedOffRegex;      // 정령해제
        private Regex _KickedFromGroupRegex;
        private Regex _JoinedGroupRegex;
        private Regex _LeftGroupRegex;
        private Regex _YouGainedExpRegex;
        private Regex _YouEarnedKinahRegex;
        private Regex _YouSpentKinahRegex;
        private Regex _YouGainedApRegex;

        /// <summary>
        /// Occurs when the parser is starting.
        /// </summary>
        public event EventHandler Starting;

        /// <summary>
        /// Occurs when the parser has started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when the parser is stopping.
        /// </summary>
        public event EventHandler Stopping;

        /// <summary>
        /// Occurs when the parser has stopped.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Occurs when KingsDamageMeter.AionLogParser is unable to find the specified log file.
        /// </summary>
        public event EventHandler FileNotFound;

        /// <summary>
        /// Occurs when the parser finds damage inflicted.
        /// </summary>
        public event DamageInflictedEventHandler DamageInflicted;

        /// <summary>
        /// Occurs when the parser finds critical damage inflicted.
        /// </summary>
        public event DamageInflictedEventHandler CriticalInflicted;

        /// <summary>
        /// Occurs when a player deals damage with a particular skill.
        /// </summary>
        public event SkillDamageInflictedEventHandler SkillDamageInflicted;

        /// <summary>
        /// Occurs when a player joins the group.
        /// </summary>
        public event PlayerEventHandler PlayerJoinedGroup;

        /// <summary>
        /// Occurs when a player leaved the group.
        /// </summary>
        public event PlayerEventHandler PlayerLeftGroup;

        /// <summary>
        /// Occurs when a player receives damage.
        /// </summary>
        public event DamageInflictedEventHandler DamageReceived;

        /// <summary>
        /// Occurs when you gain experience.
        /// </summary>
        public event ExpEventHandler ExpGained;

        /// <summary>
        /// Occurs when you earn kinah.
        /// </summary>
        public event KinahEventHandler KinahEarned;

        /// <summary>
        /// Occurs when you spend kinah.
        /// </summary>
        public event KinahEventHandler KinahSpent;

        /// <summary>
        /// Occurs when you gain abyss points.
        /// </summary>
        public event AbyssPointsEventHandler AbyssPointsGained;

        public void setEffect(string name, string effect)
        {
            if (_Effects.ContainsKey(effect))
            {
                if (_Effects[effect] != name)
                {
                    _Effects[effect] = name;
                }
            }
            else
            {
                _Effects.Add(effect, name);
            }
        }

        public void setPet(string name, string pet, string time)
        {
            if (_Pets.ContainsKey(pet))
            {
                if (!_Pets[pet].Contains(name))
                {
                    _Pets[pet] = name + "^" + time;
                }
            }
            else
            {
                _Pets.Add(pet, name + "^" + time);
            }
        }

        public void Initialize()
        {
            _TimestampRegex = Localization.Regex.TimestampRegex;
            _ChatRegex = new Regex(Localization.Regex.Chat, RegexOptions.Compiled);
            // 대미지 관련
            _GodStoneAttrDamageRegex = new Regex(_TimestampRegex + Localization.Regex.GodStoneAttrDamageRegex, RegexOptions.Compiled);
            _GodStonePoisonRegex = new Regex(_TimestampRegex + Localization.Regex.GodStonePoisonRegex, RegexOptions.Compiled);
            _CommandRegex = new Regex(_TimestampRegex + Localization.Regex.CommandRegex, RegexOptions.Compiled);
            _EnergySummonedRegex = new Regex(_TimestampRegex + Localization.Regex.EnergySummonedRegex, RegexOptions.Compiled);
            _CommonSummonedRegex = new Regex(_TimestampRegex + Localization.Regex.CommonSummonedRegex, RegexOptions.Compiled);
            _CommonSummonedSumAtk = new Regex(_TimestampRegex + Localization.Regex.CommonSummonedSumAtk, RegexOptions.Compiled);
            _InflictedSkillRegex = new Regex(_TimestampRegex + Localization.Regex.InflictedSkillRegex, RegexOptions.Compiled);
            _InflictedRegex = new Regex(_TimestampRegex + Localization.Regex.InflictedRegex, RegexOptions.Compiled);
            _ContinuousRegex = new Regex(_TimestampRegex + Localization.Regex.ContinuousRegex, RegexOptions.Compiled);
            _ContinuousDamage = new Regex(_TimestampRegex + Localization.Regex.ContinuousDamage, RegexOptions.Compiled);
            _AddDamageRegex = new Regex(_TimestampRegex + Localization.Regex.AddDamageRegex, RegexOptions.Compiled);
            _DelayedRegex = new Regex(_TimestampRegex + Localization.Regex.DelayedRegex, RegexOptions.Compiled);
            _EffectRegex = new Regex(_TimestampRegex + Localization.Regex.EffectRegex, RegexOptions.Compiled);
            _EffectDamageRegex = new Regex(_TimestampRegex + Localization.Regex.EffectDamageRegex, RegexOptions.Compiled);            
            // 기타
            _CommonSummonedOffRegex = new Regex(_TimestampRegex + Localization.Regex.CommonSummonedOffRegex, RegexOptions.Compiled);
            _JoinedGroupRegex = new Regex(_TimestampRegex + Localization.Regex.JoinedGroupRegex, RegexOptions.Compiled);
            _LeftGroupRegex = new Regex(_TimestampRegex + Localization.Regex.LeftGroupRegex, RegexOptions.Compiled);
            _KickedFromGroupRegex = new Regex(_TimestampRegex + Localization.Regex.KickedFromGroupRegex, RegexOptions.Compiled);
            _YouGainedExpRegex = new Regex(_TimestampRegex + Localization.Regex.YouGainedExpRegex, RegexOptions.Compiled);
            _YouEarnedKinahRegex = new Regex(_TimestampRegex + Localization.Regex.YouEarnedKinahRegex, RegexOptions.Compiled);
            _YouSpentKinahRegex = new Regex(_TimestampRegex + Localization.Regex.YouSpentKinahRegex, RegexOptions.Compiled);
            _YouGainedApRegex = new Regex(_TimestampRegex + Localization.Regex.YouGainedApRegex, RegexOptions.Compiled);
        }

        /// <summary>
        /// Gets the running status of the parser.
        /// </summary>
        public bool Running
        {
            get
            {
                return _Running;
            }
        }

        /// <summary>
        /// Start the parser.
        /// </summary>
        public void Start(string file)
        {
            if (_Running)
            {
                return;
            }

            if (Starting != null)
            {
                Starting(this, EventArgs.Empty);
            }

            if ((_FileStream = OpenFileStream(file)) != null)
            {
                _Running = true;

                // Skip the stuff in the file from the last session.
                _FileStream.Position = _FileStream.Length;

                _StreamReader = GetStreamReader(_FileStream);
                StartWorker();
            }

            if (Started != null)
            {
                Started(this, EventArgs.Empty);
            }

            DebugLogger.Write("Log parser initialized: \"" + _LogPath + "\"");
        }

        /// <summary>
        /// Stop the parser.
        /// </summary>
        public void Stop()
        {
            if (!_Running)
            {
                return;
            }
            else
            {
                _Running = false;
            }

            if (Stopping != null)
            {
                Stopping(this, EventArgs.Empty);
            }

            // Working out how to avoid Abort()
            if (_Worker != null)
            {
                _Worker.Abort();
                _Worker = null;
            }

            if (_StreamReader != null)
            {
                _StreamReader.Close();
                _StreamReader = null;
            }

            if (_FileStream != null)
            {
                _FileStream.Close();
                _FileStream = null;
            }

            if (Stopped != null)
            {
                Stopped(this, EventArgs.Empty);
            }

            DebugLogger.Write("Log parser stopped.");
        }

        /// <summary>
        /// Open a System.IO.FileStream for the specified file with the FileMode Open, FileAccess Read and FileShare ReadWrite
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private FileStream OpenFileStream(string file)
        {
            FileStream stream = null;
            _LogPath = file;

            try
            {
                stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    if (FileNotFound != null)
                    {
                        FileNotFound(this, EventArgs.Empty);
                    }
                }

                DebugLogger.Write("Error opening Chat.Log: " + Environment.NewLine + e.Message);
            }

            return stream;
        }

        /// <summary>
        /// Initialize a new instance of System.IO.StreamReader for the specified stream.
        /// </summary>
        /// <param name="stream">The file stream.</param>
        /// <returns></returns>
        private StreamReader GetStreamReader(FileStream stream)
        {
            if (stream != null)
            {
                return new StreamReader(stream, System.Text.Encoding.Default);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Start the parser's worker thread.
        /// </summary>
        private void StartWorker()
        {
            _Worker = new Thread
            (
                delegate()
                {
                    lock (_LockObject)
                    {
                        while (_Running)
                        {
                            string line = _StreamReader.ReadLine();

                            if (!String.IsNullOrEmpty(line))
                            {
                                ParseLine(line);
                            }

                            // Oops yah, it was eating up cpu without this.
                            Thread.Sleep(1);
                        }
                    }
                }
            );

            _Worker.IsBackground = true;
            _Worker.Start();
        }

        private void ParseLine(string line)
        {
            if (String.IsNullOrEmpty(line))
            {
                return;
            }

            MatchCollection matches, matches2;
            matches = _ChatRegex.Matches(line);
            string debugprint = ">>>>>";
            if (matches.Count > 0)
            {
                return;
            }

            bool matched = false;
            string regex = String.Empty;

            try
            {
                // 신석 대미지 추가
                matches = _GodStoneAttrDamageRegex.Matches(line);
                if (matches.Count > 0)
                {
                    if (Settings.Default.IsGodStone == false) return;
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    int damage = matches[0].Groups[_DamageGroupName].Value.GetDigits();
                    string target = matches[0].Groups[_TargetGroupName].Value;
                    string effect = matches[0].Groups[_EffectGroupName].Value;
                    string name = "";
                    if (1 <= damage & damage <= 150)
                    {
                        name = "신석" + effect + "150";
                    }
                    else if (151 <= damage & damage <= 500)
                    {
                        name = "신석" + effect + "500";
                    }
                    else if (501 <= damage & damage <= 900)
                    {
                        name = "신석" + effect + "900";
                    }
                    else if (901 <= damage & damage <= 1800)
                    {
                        name = "신석" + effect + "1800";
                    }
                    else if (1801 <= damage & damage <= 2000)
                    {
                        name = "신석" + effect + "2000";
                    }
                    else if (2001 <= damage)
                    {
                        name = "신석" + effect + "2000~";
                    }
                    else
                    {
                        return;
                    }

                    if (DamageInflicted != null)
                    {
                        DamageInflicted(this, new PlayerDamageEventArgs(time, name, damage));
                    }
                    debugprint += "유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]] - _GodStoneAttrDamageRegex:";
                    matched = true;
                    return;
                }

                matches = _GodStonePoisonRegex.Matches(line);
                if (matches.Count > 0)  // 중독신석
                {
                    if (Settings.Default.IsGodStone == false) return;
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    int damage = matches[0].Groups[_DamageGroupName].Value.GetDigits();
                    string target = matches[0].Groups[_TargetGroupName].Value;
                    string name = "신석(중독)";                    
                    if (DamageInflicted != null)
                    {
                        DamageInflicted(this, new PlayerDamageEventArgs(time, name, damage));
                    }
                    debugprint += "유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]] - _GodStoneAttrDamageRegex:";
                    matched = true;
                    return;
                }

                matches = _CommandRegex.Matches(line);
                if (matches.Count > 0)
                {   // 정령 명령류 대미지 처리
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string pet = matches[0].Groups[_PetGroupName].Value;
                    string skill = matches[0].Groups[_SkillGroupName].Value;
                    string target = matches[0].Groups[_TargetGroupName].Value;
                    int damage = matches[0].Groups[_DamageGroupName].Value.GetDigits();
                    string effect = matches[0].Groups[_EffectGroupName].Value;

                    if (_Pets.ContainsKey(pet))
                    {
                        if (SkillDamageInflicted != null)
                        {
                            string name = _Pets[pet].Split('^')[0];
                            debugprint += "유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + skill + "]] - CommandRegex:";
                            SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, name, damage, skill));
                        }
                    }
                    debugprint += "_CommandRegex";
                    matched = true;
                    return;
                }

                matches = _EnergySummonedRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string name = matches[0].Groups[_NameGroupName].Value;
                    string target = matches[0].Groups[_TargetGroupName].Value;
                    string skill = matches[0].Groups[_SkillGroupName].Value;
                    string pet = matches[0].Groups[_PetGroupName].Value;

                    if (name.Trim().Length == 0) name = Settings.Default.YouAlias;
                    if (_Energy.ContainsKey(pet + "^" + target))
                    {
                        _Energy[pet + "^" + target] = name + "^" + time.ToString() + "^" + skill;
                    }
                    else
                    {
                        _Energy.Add(pet + "^" + target, name + "^" + time.ToString() + "^" + skill);
                    }
                    debugprint += "유저:[[" + name + "]], 타겟:[[" + target + "]], 소환:[[" + pet + "]], 스킬:[[" + skill + "]] - 기운,소환:_EnergySummonedRegex";
                    matched = true;
                    return;
                }

                matches = _CommonSummonedRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string name = matches[0].Groups[_NameGroupName].Value;
                    string skill = matches[0].Groups[_SkillGroupName].Value;
                    string pet = matches[0].Groups[_PetGroupName].Value;
                    string strkey = pet;
                    if (pet.Contains("덫")) strkey = skill + pet;
                    if (name.Trim().Length == 0) name = Settings.Default.YouAlias;
                    if (_Pets.ContainsKey(strkey))
                    {
                        if (!_Pets[strkey].Contains(name))
                        {
                            _Pets[strkey] = name + "^" + time.ToString();
                        }
                    }
                    else
                    {
                        _Pets.Add(strkey, name + "^" + time.ToString());
                    }
                    debugprint += "유저:[[" + name + "]], 소환:[[" + pet + "]], 스킬:[[" + skill + "]] - 소환:_CommonSummonedRegex";
                    matched = true;
                    return;
                }

                // 트랩보다 상위에 있어야함
                matches = _CommonSummonedSumAtk.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string pet = matches[0].Groups[_PetGroupName].Value;
                    string skill = matches[0].Groups[_SkillGroupName].Value;
                    int damage = matches[0].Groups[_DamageGroupName].Value.GetDigits();
                    string target = matches[0].Groups[_TargetGroupName].Value;

                    // 소환수가 소환하여 공격함
                    if (_Energy.ContainsKey(pet + "^" + target))
                    {
                        string[] strvalues = _Energy[pet + "^" + target].Split('^');
                        string strusername = strvalues[0];
                        string strskill = "";
                        if (strvalues.Length == 3)
                        {
                            strskill = strvalues[2];
                        }
                        else
                        {
                            strskill = "뭔가오류";
                        }

                        if (SkillDamageInflicted != null)
                        {
                            SkillDamageInflicted(this,
                                                 new PlayerSkillDamageEventArgs(time, strusername, damage,
                                                                                strskill));
                        }
                    }
                    matched = true;
                    regex = "_CommonSummonedSumAtk";
                    return;
                }

                matches = _InflictedSkillRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string name = matches[0].Groups[_NameGroupName].Value;
                    string skill = matches[0].Groups[_SkillGroupName].Value;
                    string target = matches[0].Groups[_TargetGroupName].Value;
                    int damage = matches[0].Groups[_DamageGroupName].Value.GetDigits();

                    if (skill.Contains(" 효과")) skill = skill.Substring(0, skill.IndexOf(" 효과"));
                    if (name.Trim().Length == 0) name = Settings.Default.YouAlias;
                    if (name.Contains(" "))
                    {
                        if (_Pets.ContainsKey(skill + name))
                        {   // 덫 스킬(폭발의 덫)
                            name = _Pets[skill + name].Split('^')[0];
                            if (SkillDamageInflicted != null)
                            {
                                debugprint += "유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + skill + "]] - 덫스킬:";
                                SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, name, damage, skill));
                            }
                        }
                        else if (_Dots.ContainsKey(skill))
                        {   // 태풍소환
                            string[] strvalue = _Dots[skill].Split('^');
                            if (SkillDamageInflicted != null)
                            {
                                debugprint += "유저:[[" + strvalue[0] + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + skill + "]] - 태풍소환:";
                                SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, strvalue[0], damage, skill));
                            }
                        }
                        else if (_Energy.ContainsKey(name + "^" + target)) {
                            // 고결한 기운
                            string[] strvalue = _Energy[name + "^" + target].Split('^');
                            if( strvalue.Length == 3 ) {
                                skill = strvalue[2];
                            } else {
                                skill = "스킬오류";
                            }
                            debugprint += "유저:[[" + strvalue[0] + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + skill + "]] - 기운:";
                                SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, strvalue[0], damage, skill));
                        }
                    }
                    else
                    {
                        if (SkillDamageInflicted != null)
                        {
                            debugprint += "유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + skill + "]] - 일반스킬:";
                            SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, name, damage, skill));
                        }
                        if (line.Contains("일부 강화 마법이 제거됐습니다") | skill.Contains("고갈의 문양 폭발"))
                        {   //마법역류 스킬류는 도트 스킬임, 고갈 문양 폭발 추가 대미지 발생
                            if (_Dots.ContainsKey(skill + "^" + target))
                            {
                                _Dots[skill + "^" + target] = name + "^" + time.ToString();
                            }
                            else
                            {
                                _Dots.Add(skill + "^" + target, name + "^" + time.ToString());
                            }
                            DebugLogger.Write(debugprint + "_InflictedSkillRegex");
                            debugprint = ">>>>>유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 스킬명[[" + skill + "]] - 도트추가:";
                        }
                    }
                    debugprint += "_InflictedSkillRegex";
                    matched = true;
                    return;
                }

                matches = _InflictedRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string name = matches[0].Groups[_NameGroupName].Value;
                    string target = matches[0].Groups[_TargetGroupName].Value;
                    int damage = matches[0].Groups[_DamageGroupName].Value.GetDigits();

                    if (name.Trim().Length == 0) name = Settings.Default.YouAlias;
                    if (_Energy.ContainsKey(name + "^" + target))
                    {   // 정령의 기운류
                        string[] strvalues = _Energy[name + "^" + target].Split('^');
                        string strusername = strvalues[0];
                        string strskill = "";
                        if (strvalues.Length == 3)
                        {
                            strskill = strvalues[2];
                        }
                        else
                        {
                            strskill = "뭔가오류";
                        }

                        if (SkillDamageInflicted != null)
                        {
                            debugprint += "유저:[[" + strusername + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + strskill + "]] - EnergySummonedAttack:";
                            SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, strusername, damage, strskill));
                        }
                    }
                    else if (_Pets.ContainsKey(name))
                    {
                        string pet = name;
                        name = _Pets[pet].Split('^')[0];

                        if (SkillDamageInflicted != null)
                        {
                            debugprint += "유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + pet + "]] - PetAttack:";
                            SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, name, damage, pet));
                        }
                    }
                    else
                    {
                        if (DamageInflicted != null)
                        {
                            debugprint += "유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]] - Attack:";
                            DamageInflicted(this, new PlayerDamageEventArgs(time, name, damage));
                        }
                    }
                    debugprint += "_InflictedRegex";
                    matched = true;
                    return;
                }

                matches = _ContinuousRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string name = matches[0].Groups[_NameGroupName].Value;
                    string skill = matches[0].Groups[_SkillGroupName].Value;
                    string target = matches[0].Groups[_TargetGroupName].Value;

                    if (name.Contains(" "))
                    {
                        if (skill.Contains(" 효과")) skill = skill.Substring(0, skill.IndexOf(" 효과"));
                        if (_Pets.ContainsKey(skill + name)) name = _Pets[skill + name].Split('^')[0];
                    }
                    if (name.Trim().Length == 0) name = Settings.Default.YouAlias;
                    if (_Dots.ContainsKey(skill + "^" + target))
                    {
                        _Dots[skill + "^" + target] = name + "^" + time.ToString();
                    }
                    else
                    {
                        _Dots.Add(skill + "^" + target, name + "^" + time.ToString());
                    }
                    debugprint += "유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 스킬 [[" + skill + "]] - 도트추가:";
                    debugprint += "_ContinuousRegex";
                    matched = true;
                    return;
                }

                matches = _ContinuousDamage.Matches(line);
                matches2 = _AddDamageRegex.Matches(line);
                if (matches.Count > 0)
                {
                    if (matches2.Count > 0) matches = matches2;
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string target = matches[0].Groups[_TargetGroupName].Value;
                    string skill = matches[0].Groups[_SkillGroupName].Value;
                    int damage = matches[0].Groups[_DamageGroupName].Value.GetDigits();

                    if (skill.Contains(" 효과")) skill = skill.Substring(0, skill.IndexOf(" 효과"));
                    if (_Dots.ContainsKey(skill + "^" + target))
                    {   // 궁성 덫 스킬, skill + target
                        string[] strdotnametime = _Dots[skill + "^" + target].Split('^');
                        if (SkillDamageInflicted != null)
                        {
                            debugprint += "유저:[[" + strdotnametime[0] + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + skill + "]] - 도트대미지:";
                            SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, strdotnametime[0], damage, skill));
                        }
                    }
                    else if (_Effects.ContainsKey(skill))
                    {
                        // 독바르기류
                        if (SkillDamageInflicted != null)
                        {
                            debugprint += "유저:[[" + _Effects[skill] + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + skill + "]] - 도트대미지:";
                            SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, _Effects[skill], damage, skill));
                        }
                    }
                    else
                    {
                        debugprint += "유저:[[1인1직업]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + skill + "]] - 도트대미지:";
                        SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, "%%OneClass%%", damage, skill));
                    }
                    debugprint += "_ContinuousDamage";
                    matched = true;
                    return;
                }

                matches = _DelayedRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string name = matches[0].Groups[_NameGroupName].Value;
                    string skill = matches[0].Groups[_SkillGroupName].Value;
                    string target = matches[0].Groups[_TargetGroupName].Value;

                    if (name.Trim().Length == 0) name = Settings.Default.YouAlias;
                    string strKey = "";
                    if (target.Trim().Length == 0)
                    {
                        strKey = skill;
                    }
                    else
                    {
                        strKey = skill + "^" + target;
                    }
                    if (_Dots.ContainsKey(strKey))
                    {
                        _Dots[strKey] = name + "^" + time.ToString();
                    }
                    else
                    {
                        _Dots.Add(strKey, name + "^" + time.ToString());
                    }
                    debugprint += "유저:[[" + name + "]], 타겟 [[" + target +
                                        "]], 스킬 [[" + skill + "]] - 도트추가:";
                    debugprint += "_DelayedRegex";
                    matched = true;
                    return;
                }

                matches = _EffectRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string name = matches[0].Groups[_NameGroupName].Value;
                    string effect = matches[0].Groups[_EffectGroupName].Value;

                    if (name.Trim().Length == 0) name = Settings.Default.YouAlias;
                    if (_Effects.ContainsKey(effect))
                    {
                        if (_Effects[effect] != name)
                        {
                            _Effects[effect] = name;
                        }
                    }
                    else
                    {
                        _Effects.Add(effect, name);
                    }
                    debugprint += "유저:[[" + name + "]], 효과 [[" + effect + "]] - 효과:_EffectRegex";
                    matched = true;
                    return;
                }

                matches = _EffectDamageRegex.Matches(line);
                if (matches.Count > 0)
                {   // 대표적 스킬 바람의 약속
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    int damage = matches[0].Groups[_DamageGroupName].Value.GetDigits();
                    string target = matches[0].Groups[_TargetGroupName].Value;
                    string effect = matches[0].Groups[_EffectGroupName].Value;

                    if (_Effects.ContainsKey(effect))
                    {
                        if (SkillDamageInflicted != null)
                        {
                            debugprint += "유저:[[" + _Effects[effect] + "]], 타겟 [[" + target +
                                        "]], 대미지 [[" + damage.ToString() + "]], 스킬명[[" + effect + "]] - 효과대미지:";
                            SkillDamageInflicted(this, new PlayerSkillDamageEventArgs(time, _Effects[effect], damage, effect));
                        }
                    }
                    debugprint += "_EffectDamageRegex";
                    matched = true;
                    return;
                }

                matches = _CommonSummonedOffRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    string pet = matches[0].Groups[_PetGroupName].Value;

                    if (_Pets.ContainsKey(pet))
                    {
                        debugprint += "유저:[[" + _Pets[pet] + "]], 소환:[[" + pet + "]] - 소환해제:";
                        _Pets.Remove(pet);
                    }
                    debugprint += "_CommonSummonedOffRegex";
                    matched = true;
                    return;
                }

                matches = _JoinedGroupRegex.Matches(line);
                if (matches.Count > 0)
                {
                    string name = matches[0].Groups[_NameGroupName].Value;
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);

                    if (PlayerJoinedGroup != null)
                    {
                        PlayerJoinedGroup(this, new PlayerEventArgs(time, name));
                    }

                    matched = true;
                    regex = "_JoinedGroupRegex";
                    return;
                }

                matches = _LeftGroupRegex.Matches(line);
                if (matches.Count > 0)
                {
                    string name = matches[0].Groups[_NameGroupName].Value;
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);

                    if (PlayerLeftGroup != null)
                    {
                        PlayerLeftGroup(this, new PlayerEventArgs(time, name));
                    }

                    matched = true;
                    regex = "_LeftGroupRegex";
                    return;
                }

                matches = _KickedFromGroupRegex.Matches(line);
                if (matches.Count > 0)
                {
                    string name = matches[0].Groups[_NameGroupName].Value;
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);

                    if (PlayerLeftGroup != null)
                    {
                        PlayerLeftGroup(this, new PlayerEventArgs(time, name));
                    }

                    matched = true;
                    regex = "_KickedFromGroupRegex";
                    return;
                }

                matches = _YouGainedExpRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    int exp = matches[0].Groups[_ExpGroupName].Value.GetDigits();
                    string target = matches[0].Groups[_TargetGroupName].Value;

                    if (ExpGained != null)
                    {
                        ExpGained(this, new ExpEventArgs(time, exp));
                    }

                    matched = true;
                    regex = "_YouGainedExpRegex";
                    return;
                }

                matches = _YouEarnedKinahRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    int kinah = matches[0].Groups[_KinahGroupName].Value.GetDigits();

                    if (KinahEarned != null)
                    {
                        KinahEarned(this, new KinahEventArgs(time, kinah));
                    }

                    matched = true;
                    regex = "_YouEarnedKinahRegex";
                    return;
                }

                matches = _YouSpentKinahRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    int kinah = matches[0].Groups[_KinahGroupName].Value.GetDigits();

                    if (KinahSpent != null)
                    {
                        KinahSpent(this, new KinahEventArgs(time, kinah));
                    }

                    matched = true;
                    regex = "_YouSpentKinahRegex";
                    return;
                }

                matches = _YouGainedApRegex.Matches(line);
                if (matches.Count > 0)
                {
                    DateTime time = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);
                    int ap = matches[0].Groups[_ApGroupName].Value.GetDigits();

                    if (AbyssPointsGained != null)
                    {
                        AbyssPointsGained(this, new AbyssPointsEventArgs(time, ap));
                    }

                    matched = true;
                    regex = "_YouGainedApRegex";
                    return;
                }
            }
            finally
            {
                if (!matched)
                {
                    DebugLogger.Write(line);
                }
                else
                {
                    if (debugprint.Trim().Length < 6)
                    {
                        DebugLogger.Write(line);
                    }
                    else
                    {
                        DebugLogger.Write(line);
                        DebugLogger.Write(debugprint);
                    }
                    // Dots 스킬 정리 : 2분 지난 스킬은 Dic에서 삭제
                    foreach (KeyValuePair<string, string> each in _Dots)
                    {
                        string strkey = each.Key;
                        string strvalue = each.Value;
                        string[] strvaluesplit = strvalue.Split('^');
                        DateTime chktime = Convert.ToDateTime(strvaluesplit[1]);
                        DateTime chknow = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);

                        chktime = chktime.AddMinutes(2);
                        if (chktime < chknow)
                        {
                            _Dots.Remove(strkey);
                            break;
                        }
                    }

                    // Pets 정리 : 덫종류는 1분이 지나면 Dic에서 삭제
                    foreach (KeyValuePair<string, string> each in _Pets)
                    {
                        string strkey = each.Key;
                        string strvalue = each.Value;
                        string[] strvaluesplit = strvalue.Split('^');
                        if (strkey.Substring(strkey.Length - 1, 1) == "덫")
                        {
                            DateTime chktime = Convert.ToDateTime(strvaluesplit[1]);
                            DateTime chknow = matches[0].Groups[_TimeGroupName].Value.GetTime(_TimeFormat);

                            chktime = chktime.AddMinutes(1);
                            if (chktime < chknow)
                            {
                                _Dots.Remove(strkey);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}