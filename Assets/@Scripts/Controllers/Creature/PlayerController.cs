using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Newtonsoft.Json.Schema;
using UnityEngine;

// Heirarchy에서 볼려고 만듬
[Serializable]
public class PlayerStat
{
    public int DataId;
    public float Hp;
    public float MaxHp;
    public float MaxHpBonusRate = 1;
    public float HealBonusRate = 1;
    public float HpRegen;
    public float Atk;
    public float AttackRate = 1;
    public float Def;
    public float DefRate = 1;
    public float CriRate;
    public float CriDamage = 1.5f;
    public float DamageReduction;
    public float MoveSpeedRate = 1;
    public float MoveSpeed;
}


public class PlayerController : CreatureController
{
    [SerializeField]
    public GameObject Indicator;
    [SerializeField]
    public GameObject IndicatorSprite;
    Vector2 _moveDir = Vector2.zero;
    public PlayerStat StatViewer = new PlayerStat();

    #region Action
    public Action OnPlayerDataUpdated;
    public Action OnPlayerLevelUp;
    public Action OnPlayerDead;
    public Action OnPlayerDamaged;
    public Action OnPlayerMove;
    #endregion

    #region For Save
    public override int DataId
    {
        get { return Managers.Game.ContinueInfo.PlayerDataId; }
        set { Managers.Game.ContinueInfo.PlayerDataId = StatViewer.DataId = value; }
    }
    public override float Hp
    {
        get { return Managers.Game.ContinueInfo.Hp; }
        set
        {
            if (value > MaxHp)
            {
                Managers.Game.ContinueInfo.Hp = StatViewer.Hp = MaxHp;
            }
            else
            {
                Managers.Game.ContinueInfo.Hp = StatViewer.Hp = value;
            }
        }
    }
    public override float MaxHp
    {
        get { return Managers.Game.ContinueInfo.MaxHp; }
        set { Managers.Game.ContinueInfo.MaxHp = StatViewer.MaxHp = value; }
    }
    public override float MaxHpBonusRate
    {
        get { return Managers.Game.ContinueInfo.MaxHpBonusRate; }
        set { Managers.Game.ContinueInfo.MaxHpBonusRate = StatViewer.MaxHpBonusRate = value; }
    }
    public override float HealBonusRate
    {
        get { return Managers.Game.ContinueInfo.HealBonusRate; }
        set { Managers.Game.ContinueInfo.HealBonusRate = StatViewer.HealBonusRate = value; }
    }
    public override float HpRegen
    {
        get { return Managers.Game.ContinueInfo.HpRegen; }
        set { Managers.Game.ContinueInfo.HpRegen = StatViewer.HpRegen = value; }
    }
    public override float Atk
    {
        get { return Managers.Game.ContinueInfo.Atk; }
        set { Managers.Game.ContinueInfo.Atk = StatViewer.Atk = value; }
    }
    public override float AttackRate
    {
        get { return Managers.Game.ContinueInfo.AttackRate; }
        set { Managers.Game.ContinueInfo.AttackRate = StatViewer.AttackRate = value; }
    }
    public override float Def
    {
        get { return Managers.Game.ContinueInfo.Def; }
        set { Managers.Game.ContinueInfo.Def = StatViewer.Def = value; }
    }
    public override float DefRate
    {
        get { return Managers.Game.ContinueInfo.DefRate; }
        set { Managers.Game.ContinueInfo.DefRate = StatViewer.DefRate = value; }
    }
    public override float CriRate
    {
        get { return Managers.Game.ContinueInfo.CriRate; }
        set { Managers.Game.ContinueInfo.CriRate = StatViewer.CriRate = value; }
    }
    public override float CriDamage
    {
        get { return Managers.Game.ContinueInfo.CriDamage; }
        set { Managers.Game.ContinueInfo.CriDamage = StatViewer.CriDamage = value; }
    }
    public override float DamageReduction
    {
        get { return Managers.Game.ContinueInfo.DamageReduction; }
        set { Managers.Game.ContinueInfo.DamageReduction = StatViewer.DamageReduction = value; }
    }
    public override float MoveSpeedRate
    {
        get { return Managers.Game.ContinueInfo.MoveSpeedRate; }
        set { Managers.Game.ContinueInfo.MoveSpeedRate = StatViewer.MoveSpeedRate = value; }
    }
    public override float MoveSpeed
    {
        get { return Managers.Game.ContinueInfo.MoveSpeed; }
        set { Managers.Game.ContinueInfo.MoveSpeed = StatViewer.MoveSpeed = value; }
    }
    public int Level
    {
        get { return Managers.Game.ContinueInfo.Level; }
        set { Managers.Game.ContinueInfo.Level = value; }
    }
    public float Exp
    {
        get { return Managers.Game.ContinueInfo.Exp; }
        set
        {
            Managers.Game.ContinueInfo.Exp = value;

            // Levelup Check
            int level = Level;
            while (true)
            {
                // If 만렙 break
                LevelData nextLevel;
                if (Managers.Data.LevelDataDic.TryGetValue(level + 1, out nextLevel) == false)
                    break;

                LevelData currentLevel;
                Managers.Data.LevelDataDic.TryGetValue(level, out currentLevel);
                if (Managers.Game.ContinueInfo.Exp < currentLevel.TotalExp)
                    break;
                level++;
            }

            if (level != Level)
            {
                Level = level;
                LevelData currentLevel;
                Managers.Data.LevelDataDic.TryGetValue(level, out currentLevel);
                TotalExp = currentLevel.TotalExp;
                // LevelUp(Level);
            }

            OnPlayerDataUpdated?.Invoke();
            // OnPlayerDataUpdated();
        }
    }
    public float TotalExp
    {
        get { return Managers.Game.ContinueInfo.TotalExp; }
        set { Managers.Game.ContinueInfo.TotalExp = value; }
    }
    public float ExpBonusRate
    {
        get { return Managers.Game.ContinueInfo.ExpBonusRate; }
        set { Managers.Game.ContinueInfo.ExpBonusRate = value; }
    }
    public float SoulBonusRate
    {
        get { return Managers.Game.ContinueInfo.SoulBonusRate; }
        set { Managers.Game.ContinueInfo.SoulBonusRate = value; }
    }
    public float CollectDistBonus
    {
        get { return Managers.Game.ContinueInfo.CollectDistBonus; }
        set { Managers.Game.ContinueInfo.CollectDistBonus = value; }
    }
    public int SkillRefreshCount
    {
        get { return Managers.Game.ContinueInfo.SkillRefreshCount; }
        set { Managers.Game.ContinueInfo.SkillRefreshCount = value; }
    }
    public int KillCount
    {
        get { return Managers.Game.ContinueInfo.KillCount; }
        set
        {
            Managers.Game.ContinueInfo.KillCount = value;
            // if (Managers.Game.DicMission.TryGetValue(MissionTarget.MonsterKill, out MissionInfo mission))
            // {
            //     mission.Progress = value;
            // }
            if (Managers.Game.ContinueInfo.KillCount % 500 == 0)
            {
                Skills.OnMonsterKillBonus();
            }
            OnPlayerDataUpdated?.Invoke();
        }
    }
    #endregion



    public override void OnDeathAnimationEnd()
    {
    }
}
