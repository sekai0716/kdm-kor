﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.1
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KingsDamageMeter.Localization {
    using System;
    
    
    /// <summary>
    ///   지역화된 문자열 등을 찾기 위한 강력한 형식의 리소스 클래스입니다.
    /// </summary>
    // 이 클래스는 ResGen 또는 Visual Studio와 같은 도구를 통해 StronglyTypedResourceBuilder
    // 클래스에서 자동으로 생성되었습니다.
    // 멤버를 추가하거나 제거하려면 .ResX 파일을 편집한 다음 /str 옵션을 사용하여 ResGen을
    // 다시 실행하거나 VS 프로젝트를 다시 빌드하십시오.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Regex {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Regex() {
        }
        
        /// <summary>
        ///   이 클래스에서 사용하는 캐시된 ResourceManager 인스턴스를 반환합니다.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KingsDamageMeter.Localization.Regex", typeof(Regex).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   이 강력한 형식의 리소스 클래스를 사용하여 모든 리소스 조회에 대한 현재 스레드의 CurrentUICulture
        ///   속성을 재정의합니다.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   \[charname:과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string Chat {
            get {
                return ResourceManager.GetString("Chat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+)[이가] (?&lt;skill&gt;.+)의 효과로 (?&lt;damage&gt;[^a-zA-Z]+)의 중독 대미지를 받았습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string CommonDelayedPoisonDamageRegex {
            get {
                return ResourceManager.GetString("CommonDelayedPoisonDamageRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+)[이가] (?&lt;skill&gt;.+) 효과의 효과로 (?&lt;damage&gt;[^a-zA-Z]+)의 중독 대미지를 받았습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string CommonDelayedTrapDamageRegex {
            get {
                return ResourceManager.GetString("CommonDelayedTrapDamageRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;pet&gt;.+)[이가] 소환: (?&lt;skill&gt;.+) 효과를 사용해 (?&lt;target&gt;.+)에게 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 줬습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string CommonSummonedSumAtk {
            get {
                return ResourceManager.GetString("CommonSummonedSumAtk", resourceCulture);
            }
        }
        
        /// <summary>
        ///    Effect과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string Effect {
            get {
                return ResourceManager.GetString("Effect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+)[이가] 소환: (?&lt;skill&gt;.+)을 사용해 (?&lt;target&gt;.+)[을를] 공격할 (?&lt;pet&gt;.+)[을를] 소환했습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string EnergySummonedRegex {
            get {
                return ResourceManager.GetString("EnergySummonedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+) 님이 파티에 참가했습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string JoinedGroupRegex {
            get {
                return ResourceManager.GetString("JoinedGroupRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+) has been kicked out of your group\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string KickedFromGroupRegex {
            get {
                return ResourceManager.GetString("KickedFromGroupRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+) 님이 파티를 떠났습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string LeftGroupRegex {
            get {
                return ResourceManager.GetString("LeftGroupRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+)[이가] (?&lt;skill&gt;.+)의 효과로 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 받았습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherContinuousDamage {
            get {
                return ResourceManager.GetString("OtherContinuousDamage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (치명타! )?(?&lt;name&gt;.+)[이가] (?&lt;skill&gt;.+)을 사용해 (?&lt;target&gt;.+)에게 지속적인 대미지 효과를 줬습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherContinuousRegex {
            get {
                return ResourceManager.GetString("OtherContinuousRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+) received the delayed explosion effect because (?&lt;name&gt;.+) used (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherDelayedRegex {
            get {
                return ResourceManager.GetString("OtherDelayedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+) is bleeding because (?&lt;name&gt;.+) used (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherInflictedBleedRegex {
            get {
                return ResourceManager.GetString("OtherInflictedBleedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+)[이가] (?&lt;target&gt;.+)에게 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 줬습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherInflictedRegex {
            get {
                return ResourceManager.GetString("OtherInflictedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+)[이가] (?&lt;skill&gt;.+) 추가 효과의 효과로 (?&lt;damage&gt;[^a-zA-Z]+)의 생명력을 회복했습니다.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherInflictedSkillCureEx {
            get {
                return ResourceManager.GetString("OtherInflictedSkillCureEx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+)[이가] (?&lt;skill&gt;.+) 추가 효과의 효과로 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 받았습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherInflictedSkillEx {
            get {
                return ResourceManager.GetString("OtherInflictedSkillEx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (치명타! )?(?&lt;name&gt;.+)[이가] (?&lt;skill&gt;.+)을 사용해 (?&lt;target&gt;.+)에게 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 줬습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherInflictedSkillRegex {
            get {
                return ResourceManager.GetString("OtherInflictedSkillRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (치명타! )?(?&lt;name&gt;.+)[이가] (?&lt;skill&gt;.+)을 사용해 (?&lt;target&gt;.+)에게 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 주고 (?&lt;effect&gt;.+) 효과가 발생됐습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherInflictedSkillRegex2 {
            get {
                return ResourceManager.GetString("OtherInflictedSkillRegex2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+) became poisoned because (?&lt;name&gt;.+) used (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherPoisonEffectRegex {
            get {
                return ResourceManager.GetString("OtherPoisonEffectRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+) received (?&lt;damage&gt;[^a-zA-Z]+) (bleeding|poisoning) damage after you used (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherReceivedBleedRegex {
            get {
                return ResourceManager.GetString("OtherReceivedBleedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+)[이가] (?&lt;target&gt;.+)에게서 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 받았습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherReceivedRegex {
            get {
                return ResourceManager.GetString("OtherReceivedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+) received (?&lt;damage&gt;[^a-zA-Z]+) damage due to the effect of (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherReceivedSkillRegex {
            get {
                return ResourceManager.GetString("OtherReceivedSkillRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+) has summoned (?&lt;pet&gt;.+) to attack (?&lt;target&gt;.+) by using (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherSummonedAttackRegex {
            get {
                return ResourceManager.GetString("OtherSummonedAttackRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;name&gt;.+)[이가] 소환: (?&lt;skill&gt;.+)을 사용해 (?&lt;pet&gt;.+)을 소환했습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string OtherSummonedRegex {
            get {
                return ResourceManager.GetString("OtherSummonedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;pet&gt;.+)[이가] (?&lt;skill&gt;.+) 효과를 사용해 (?&lt;target&gt;.+)[이가] 중독 상태가 됐습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string SummonedDelayRegex {
            get {
                return ResourceManager.GetString("SummonedDelayRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;pet&gt;.+)[이가] (?&lt;skill&gt;.+) 효과를 사용해 (?&lt;target&gt;.+)에게 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 줬습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string SummonedTrapRegex {
            get {
                return ResourceManager.GetString("SummonedTrapRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;time&gt;[^a-zA-Z]+) : 과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string TimestampRegex {
            get {
                return ResourceManager.GetString("TimestampRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   You inflicted continuous damage on (?&lt;target&gt;.+) by using (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouContinuousRegex {
            get {
                return ResourceManager.GetString("YouContinuousRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   치명타! (?&lt;target&gt;.+)에게 (?&lt;damage&gt;[^a-zA-Z]+)의 치명적인 대미지를 줬습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouCriticalRegex {
            get {
                return ResourceManager.GetString("YouCriticalRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (치명타! )?(?&lt;skill&gt;.+)을 사용해 (?&lt;target&gt;.+)[이가] 중독 상태가 됐습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouDelayedPoisonRegex {
            get {
                return ResourceManager.GetString("YouDelayedPoisonRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+) received the delayed explosion effect as you used (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouDelayedRegex {
            get {
                return ResourceManager.GetString("YouDelayedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;kinah&gt;[^a-zA-Z]+)키나를 얻었습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouEarnedKinahRegex {
            get {
                return ResourceManager.GetString("YouEarnedKinahRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+) received (?&lt;damage&gt;[^a-zA-Z]+)( poisoning)? damage after you used (?&lt;effect&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouEffectDamageRegex {
            get {
                return ResourceManager.GetString("YouEffectDamageRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;ap&gt;[^a-zA-Z]+) 어비스 포인트를 얻었습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouGainedApRegex {
            get {
                return ResourceManager.GetString("YouGainedApRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   You received the effect by using (?&lt;effect&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouGainedEffectRegex {
            get {
                return ResourceManager.GetString("YouGainedEffectRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+)에게 (?&lt;exp&gt;[^a-zA-Z]+)만큼의 경험치를 얻었습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouGainedExpRegex {
            get {
                return ResourceManager.GetString("YouGainedExpRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   You caused (?&lt;target&gt;.+) to bleed by using (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouInflictedBleed2Regex {
            get {
                return ResourceManager.GetString("YouInflictedBleed2Regex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+) is bleeding because You used (?&lt;skill&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouInflictedBleedRegex {
            get {
                return ResourceManager.GetString("YouInflictedBleedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;target&gt;.+)에게 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 줬습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouInflictedRegex {
            get {
                return ResourceManager.GetString("YouInflictedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (치명타! )?(?&lt;skill&gt;.+)을 사용해 (?&lt;target&gt;.+)에게 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 줬습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouInflictedSkillRegex {
            get {
                return ResourceManager.GetString("YouInflictedSkillRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (치명타! )?(?&lt;skill&gt;.+)을 사용해 (?&lt;target&gt;.+)에게 (?&lt;damage&gt;[^a-zA-Z]+)의 대미지를 주고 (?&lt;effect&gt;.+) 효과가 발생했습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouInflictedSkillRegex1 {
            get {
                return ResourceManager.GetString("YouInflictedSkillRegex1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   You received (?&lt;damage&gt;[^a-zA-Z]+) damage from (?&lt;target&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouReceivedRegex {
            get {
                return ResourceManager.GetString("YouReceivedRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;kinah&gt;[^a-zA-Z]+)키나를 사용했습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouSpentKinahRegex {
            get {
                return ResourceManager.GetString("YouSpentKinahRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   You summoned (?&lt;pet&gt;.+) by using (?&lt;skill&gt;.+) to let it attack (?&lt;target&gt;.+)\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouSummonedAttackRegex {
            get {
                return ResourceManager.GetString("YouSummonedAttackRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   (?&lt;skill&gt;.+)을 사용해 (?&lt;pet&gt;.+)을 소환했습니다\.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string YouSummonedRegex {
            get {
                return ResourceManager.GetString("YouSummonedRegex", resourceCulture);
            }
        }
    }
}
