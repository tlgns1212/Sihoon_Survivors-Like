using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementManager
{
    private List<Data.AchievementData> achievements
    {
        get { return Managers.Game.Achievements; }
        set { Managers.Game.Achievements = value; }
    }
    public event Action<Data.AchievementData> OnAchievementCompleted;   // 업적 완료 이벤트

    public void Init()
    {
        achievements = Managers.Data.AchievementDataDic.Values.ToList();
    }

    // 업적 완료 처리
    public void CompleteAchievement(int dataId)
    {
        AchievementData achievement = achievements.Find(a => a.AchievementId == dataId);
        if (achievement != null && achievement.IsCompleted == false)
        {
            achievement.IsCompleted = true;
            OnAchievementCompleted?.Invoke(achievement);
            // // Managers.UI.ShowToast($"업적달성 : {achievement.DescriptionTextId}");
            Managers.Game.SaveGame();
        }
    }

    // 보상 받기 완료 처리
    public void RewardedAchievement(int dataId)
    {
        AchievementData achievement = achievements.Find(a => a.AchievementId == dataId);
        if (achievement != null && achievement.IsRewarded == false)
        {
            achievement.IsRewarded = true;
            achievement.IsCompleted = true;
            Managers.Game.SaveGame();
        }
    }

    public void Attendance()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.Login).ToList();

        // 출석 업적 보상 확인
        foreach (AchievementData achievement in list)
        {
            CompleteAchievement(achievement.AchievementId);
        }
    }

    public List<AchievementData> GetProceedingAchievements()
    {
        List<AchievementData> resultList = new List<AchievementData>();

        // 1. achievements 리스트에서 MissionTarget이 같은 애들끼리 나눔
        foreach (Define.MissionTarget missionTarget in Enum.GetValues(typeof(Define.MissionTarget)))
        {
            List<AchievementData> list = achievements.Where(data => data.MissionTarget == missionTarget).ToList();
            // 2. list중에 현재 진행중인 애들을 Add
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsCompleted == false)
                {
                    resultList.Add(list[i]);
                    break;
                }
                else
                {
                    if (list[i].IsRewarded == false)
                    {
                        resultList.Add(list[i]);
                        break;
                    }

                    if (i == list.Count - 1)
                    {
                        resultList.Add(list[i]);
                    }
                }
            }
        }
        return resultList;
    }

    public int GetProgressValue(Define.MissionTarget missionTarget)
    {
        switch (missionTarget)
        {
            case Define.MissionTarget.DailyComplete:
            case Define.MissionTarget.WeeklyComplete:
            case Define.MissionTarget.MonthlyComplete:
                return 0;
            case Define.MissionTarget.StageEnter:
                return Managers.Game.DicMission[missionTarget].Progress;
            case Define.MissionTarget.StageClear:
                return Managers.Game.GetMaxStageClearIndex();
            case Define.MissionTarget.EquipmentLevelUp:
                return Managers.Game.DicMission[missionTarget].Progress;
            case Define.MissionTarget.CommonGachaOpen:
                return Managers.Game.CommonGachaOpenCount;
            case Define.MissionTarget.AdvancedGachaOpen:
                return Managers.Game.AdvancedGachaOpenCount;
            case Define.MissionTarget.OfflineRewardGet:
                return Managers.Game.OfflineRewardCount;
            case Define.MissionTarget.FastOfflineRewardGet:
                return Managers.Game.FastRewardCount;
            case Define.MissionTarget.ShopProductBuy:
                return 0;
            case Define.MissionTarget.Login:
                return Managers.Time.AttendanceDay;
            case Define.MissionTarget.EquipmentMerge:
                return Managers.Game.DicMission[missionTarget].Progress;
            case Define.MissionTarget.MonsterAttack:
                return 0;
            case Define.MissionTarget.MonsterKill:
                return Managers.Game.TotalMonsterKillCount;
            case Define.MissionTarget.EliteMonsterAttack:
                return 0;
            case Define.MissionTarget.EliteMonsterKill:
                return Managers.Game.TotalEliteKillCount;
            case Define.MissionTarget.BossKill:
                return Managers.Game.TotalBossKillCount;
            case Define.MissionTarget.DailyShopBuy:
                return 0;
            case Define.MissionTarget.GachaOpen:
                return Managers.Game.DicMission[missionTarget].Progress;
            case Define.MissionTarget.ADWatching:
                return Managers.Game.DicMission[missionTarget].Progress;
        }
        return 0;
    }

    public AchievementData GetNextAchievement(int dataId)
    {
        AchievementData achievement = achievements.Find(a => a.AchievementId == dataId + 1);
        if (achievement != null && achievement.IsRewarded == false)
        {
            return achievement;
        }
        return null;
    }

    public void StageClear()
    {
        int MaxStageClearIndex = Managers.Game.GetMaxStageClearIndex();

        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.StageClear).ToList();
        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == MaxStageClearIndex)
            {
                CompleteAchievement(achievement.AchievementId);
            }
        }
    }

    public void CommonOpen()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.CommonGachaOpen).ToList();
        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.CommonGachaOpenCount)
            {
                CompleteAchievement(achievement.AchievementId);
            }
        }
    }

    public void AdvancedOpen()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.AdvancedGachaOpen).ToList();
        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.AdvancedGachaOpenCount)
            {
                CompleteAchievement(achievement.AchievementId);
            }
        }
    }

    public void OfflineReward()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.OfflineRewardGet).ToList();
        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.OfflineRewardCount)
            {
                CompleteAchievement(achievement.AchievementId);
            }
        }
    }

    public void FastReward()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.FastOfflineRewardGet).ToList();
        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.FastRewardCount)
            {
                CompleteAchievement(achievement.AchievementId);
            }
        }
    }

    public void MonsterKill()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.MonsterKill).ToList();
        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.TotalMonsterKillCount)
            {
                CompleteAchievement(achievement.AchievementId);
            }
        }
    }

    public void EliteKill()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.EliteMonsterKill).ToList();
        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.TotalEliteKillCount)
            {
                CompleteAchievement(achievement.AchievementId);
            }
        }
    }

    public void BossKill()
    {
        List<AchievementData> list = achievements.Where(data => data.MissionTarget == Define.MissionTarget.BossKill).ToList();
        foreach (AchievementData achievement in list)
        {
            if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.TotalBossKillCount)
            {
                CompleteAchievement(achievement.AchievementId);
            }
        }
    }

}
