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
        get; set;
        // get { return Managers.Game.Achievements; }
        // set { Managers.Game.Achievements = value; }
    }
    public event Action<Data.AchievementData> OnAchievementCompleted;   // 업적 완료 이벤트

    public void Init()
    {
        achievements = Managers.Data.AchievementDataDic.Values.ToList();
    }

    // 업적 완료 처리
    public void CompleteAchievement(int dataId)
    {
        AchievementData achievement = achievements.Find(a => a.AchievmentId == dataId);
        if (achievement != null && achievement.IsCompleted == false)
        {
            achievement.IsCompleted = true;
            OnAchievementCompleted?.Invoke(achievement);
            // Managers.UI.ShowToast($"업적달성 : {achievement.DescriptionTextId}");
            Managers.Game.SaveGame();
        }
    }

    // 보상 받기 완료 처리
    public void RewardedAchievement(int dataId)
    {
        AchievementData achievement = achievements.Find(a => a.AchievmentId == dataId);
        if (achievement != null && achievement.IsRewarded == false)
        {
            achievement.IsRewarded = true;
            achievement.IsCompleted = true;
            Managers.Game.SaveGame();
        }
    }

    // TODO 여기서부터 이어서 하면 됨


}
