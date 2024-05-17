using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class TimeManager : MonoBehaviour
{
    // 보상 간격 (초 단위)
    public float rewardInterval { get; set; } = 10f;

    public int AttendanceDay
    {
        get
        {
            int savedTime = PlayerPrefs.GetInt("AttendanceDay", 1);
            return savedTime;
        }
        set
        {
            PlayerPrefs.SetInt("AttendanceDay", value);
            Managers.Achievement.Attendance();
            PlayerPrefs.Save();
        }
    }

    public float StaminaTime
    {
        get
        {
            float time = PlayerPrefs.GetFloat("StaminaTime", Define.STAMINA_RECHARGE_INTERVAL);
            return time;
        }
        set
        {
            float time = value;
            PlayerPrefs.SetFloat("StaminaTime", time);
            PlayerPrefs.Save();
        }
    }

    public DateTime LastGeneratedStaminaTime
    {
        get
        {
            string savedTimeStr = PlayerPrefs.GetString("LastGeneratedStaminaTime", string.Empty);
            if (!string.IsNullOrEmpty(savedTimeStr))
            {
                return DateTime.Parse(savedTimeStr);
            }
            else
            {
                return DateTime.Now;
            }
        }
        set
        {
            string timeStr = value.ToString();
            PlayerPrefs.SetString("LastGeneratedStaminaTime", timeStr);
            PlayerPrefs.Save();
        }
    }

    public TimeSpan TimeSinceLastStamina
    {
        get { return DateTime.Now - LastGeneratedStaminaTime; }
    }

    public DateTime LastLoginTime
    {
        get
        {
            string savedTimeStr = PlayerPrefs.GetString("LastLoginTime", string.Empty);
            if (!string.IsNullOrEmpty(savedTimeStr))
            {
                return DateTime.Parse(savedTimeStr);
            }
            else
            {
                return DateTime.Now;
            }
        }
        set
        {
            string timeStr = value.ToString();
            PlayerPrefs.SetString("LastLoginTime", timeStr);
            PlayerPrefs.Save();
        }
    }

    private DateTime _lastRewardTime;
    public DateTime LastRewardTime
    {
        get
        {
            if (_lastRewardTime == default(DateTime))
            {
                string savedTimeStr = PlayerPrefs.GetString("LastRewardTime", string.Empty);
                if (!string.IsNullOrEmpty(savedTimeStr))
                {
                    _lastRewardTime = DateTime.Parse(savedTimeStr);
                }
                else
                {
                    _lastRewardTime = DateTime.Now;
                }
            }
            return _lastRewardTime;
        }
        set
        {
            _lastRewardTime = value;
            string timeStr = value.ToString();
            PlayerPrefs.SetString("LastRewardTime", timeStr);
            PlayerPrefs.Save();
        }
    }

    public TimeSpan TimeSinceLastReward
    {
        get
        {
            TimeSpan timeSpan = DateTime.Now - LastRewardTime;
            if (timeSpan > TimeSpan.FromHours(24))
            {
                return TimeSpan.FromHours(24);
            }
            return timeSpan;
        }
    }

    public float _minute = 60;

    public void Init()
    {
        TimerStart();
    }

    public void CalcOfflineStamina()
    {
        TimeSpan span = DateTime.Now - LastGeneratedStaminaTime;

        // 스태미너 충전량 계산
        int generatedStamina = (int)(span.TotalMinutes / 5);

        RechargeStamina(generatedStamina);

        // 충전 완료된 시간 계산
        DateTime generatedTime = LastGeneratedStaminaTime.AddMinutes(generatedStamina * 5);
        TimeSpan remainingTime = generatedTime - DateTime.Now;

        StaminaTime = (float)remainingTime.TotalSeconds;
    }

    public void TimerStart()
    {
        StartCoroutine(CoStartTimer());
    }

    IEnumerator CoStartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            StaminaTime--;
            _minute--;
            TimeSpan timeSpan = TimeSpan.FromSeconds(StaminaTime);
            string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            if (StaminaTime <= 0)
            {
                RechargeStamina();
                StaminaTime = Define.STAMINA_RECHARGE_INTERVAL;
            }

            if (_minute <= 0)
            {
                CheckAttendance();
                _minute = 60;
            }
        }
    }

    private void RechargeStamina(int count = 1)
    {
        if (Managers.Game.Stamina < Define.MAX_STAMINA)
        {
            Managers.Game.Stamina += count;
            LastGeneratedStaminaTime = DateTime.Now;
        }
    }

    public void CheckAttendance()
    {
        if (IsSameDay(LastLoginTime, DateTime.Now) == false)
        {
            AttendanceDay++;
            LastLoginTime = DateTime.Now;

            // 모든 하루 n번 가능한 변수들 클리어
            Managers.Game.GachaCountAdsAdvanced = 1;
            Managers.Game.GachaCountAdsCommon = 1;
            Managers.Game.GoldCountAds = 1;
            Managers.Game.RebirthCountAds = 3;
            Managers.Game.DiaCountAds = 3;
            Managers.Game.StaminaCountAds = 1;
            Managers.Game.FastRewardCountAds = 1;
            Managers.Game.SkillRefreshCountAds = 3;
            Managers.Game.RemainsStaminaByDia = 3;
            Managers.Game.BronzeKeyCountAds = 3;
            Managers.Game.FastRewardCountStamina = 3;

            Managers.Game.DicMission.Clear();
            Managers.Game.DicMission = new Dictionary<MissionTarget, MissionInfo>()
            {
                {MissionTarget.StageEnter, new MissionInfo(){Progress = 0, IsRewarded = false}},
                {MissionTarget.StageClear, new MissionInfo(){Progress = 0, IsRewarded = false}},
                {MissionTarget.EquipmentLevelUp, new MissionInfo(){Progress = 0, IsRewarded = false}},
                {MissionTarget.OfflineRewardGet, new MissionInfo(){Progress = 0, IsRewarded = false}},
                {MissionTarget.EquipmentMerge, new MissionInfo(){Progress = 0, IsRewarded = false}},
                {MissionTarget.MonsterKill, new MissionInfo(){Progress = 0, IsRewarded = false}},
                {MissionTarget.EliteMonsterKill, new MissionInfo(){Progress = 0, IsRewarded = false}},
                {MissionTarget.GachaOpen, new MissionInfo(){Progress = 0, IsRewarded = false}},
                {MissionTarget.ADWatching, new MissionInfo(){Progress = 0, IsRewarded = false}},
            };
            Managers.Game.SaveGame();
        }
    }

    public void GiveOfflineReward(OfflineRewardData data)
    {
        string[] spriteName = new string[1];
        int[] count = new int[1];
        int gold = (int)CalculateGoldPerMinute(data.RewardGold);

        spriteName[0] = Define.GOLD_SPRITE_NAME;
        count[0] = gold;

        Managers.Game.Gold += gold;
        LastRewardTime = DateTime.Now;
        if (Managers.Game.DicMission.TryGetValue(Define.MissionTarget.OfflineRewardGet, out MissionInfo info))
        {
            info.Progress++;
        }
        Managers.Game.OfflineRewardCount++;

        // TODO : UI
        // UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        // rewardPopup.gameObject.SetActive(true);
        // rewardPopup.SetInfo(spriteName, count);
    }

    public void GiveFastOfflineReward(OfflineRewardData data)
    {
        string[] spriteName = new string[3];
        int[] count = new int[3];
        int gold = data.RewardGold * 5;

        spriteName[0] = Define.GOLD_SPRITE_NAME;
        count[0] = gold;

        spriteName[1] = Managers.Data.MaterialDic[Define.ID_RANDOM_SCROLL].SpriteName;
        count[1] = data.FastRewardScroll;

        spriteName[2] = Managers.Data.MaterialDic[Define.ID_SILVER_KEY].SpriteName;
        count[2] = data.FastRewardScroll;

        // TODO : UI
        // UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        // rewardPopup.gameObject.SetActive(true);

        if (Managers.Game.DicMission.TryGetValue(MissionTarget.FastOfflineRewardGet, out MissionInfo mission))
        {
            mission.Progress++;
        }
        Managers.Game.FastRewardCount++;

        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_RANDOM_SCROLL], data.FastRewardScroll);
        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_SILVER_KEY], data.FastRewardScroll);
        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], gold);

        // TODO : UI
        // rewardPopup.SetInfo(spriteName, count);
    }

    public float CalculateGoldPerMinute(float goldPerHour)
    {
        float goldPerMinute = goldPerHour / 60f * (int)TimeSinceLastReward.TotalMinutes;
        return goldPerMinute;
    }

    private bool IsSameDay(DateTime savedTime, DateTime currentTime)
    {
        if (savedTime.Day == currentTime.Day)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}