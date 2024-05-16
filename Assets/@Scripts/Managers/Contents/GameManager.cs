using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Define;
using Random = UnityEngine.Random;


// 계정에 대한 모든 정보
[Serializable]
public class GameData
{
    public int UserLevel = 1;
    public string UserName = "Sihoon Kim";


    public List<Character> Characters = new List<Character>();
    public ContinueData ContinueInfo = new ContinueData();
}

[Serializable]
public class ContinueData
{
    public bool isContinue { get { return SavedBattleSkillDic.Count > 0; } }
    public int PlayerDataId;
    public float Hp;
    public float MaxHp;
    public float MaxHpBonusRate = 1;
    public float HealBonusRate = 1;
    public float HpRegen;
    public float Atk;
    public float AttackRate = 1;
    public float Def;
    public float DefRate;
    public float MoveSpeed;
    public float MoveSpeedRate = 1;
    public float TotalExp;
    public int Level = 1;
    public float Exp;
    public float CriRate;
    public float CriDamage = 1.5f;
    public float DamageReduction;
    public float ExpBonusRate = 1;
    public float SoulBonusRate = 1;
    public float CollectDistBonus = 1;
    public int KillCount;
    public int SkillRefreshCount = 3;
    public float SoulCount;

    public List<SupportSkillData> SoulShopList = new List<SupportSkillData>();
    public List<SupportSkillData> SavedSupportSkillList = new List<SupportSkillData>();
    public Dictionary<Define.SkillType, int> SavedBattleSkillDic = new Dictionary<Define.SkillType, int>();

    public int WaveIndex;

    public void Clear()
    {
        PlayerDataId = 0;
        Hp = 0f;
        MaxHp = 0f;
        MaxHpBonusRate = 1f;
        HealBonusRate = 1;
        HpRegen = 0f;
        Atk = 0f;
        AttackRate = 1f;
        Def = 0f;
        DefRate = 0f;
        MoveSpeed = 0f;
        MoveSpeedRate = 1f;
        TotalExp = 0f;
        Level = 1;
        Exp = 0f;
        CriRate = 0f;
        CriDamage = 1.5f;
        DamageReduction = 0f;
        ExpBonusRate = 1f;
        SoulBonusRate = 1f;
        CollectDistBonus = 1f;

        KillCount = 0;
        SoulCount = 0f;
        SkillRefreshCount = 3;

        SoulShopList.Clear();
        SavedSupportSkillList.Clear();
        SavedBattleSkillDic.Clear();
    }
}

public class GameManager
{
    #region GameData
    public GameData _gameData = new GameData();
    public List<SupportSkillData> SoulShopList
    {
        get { return _gameData.ContinueInfo.SoulShopList; }
        set
        {
            _gameData.ContinueInfo.SoulShopList = value;
            // SaveGame();
        }
    }
    public ContinueData ContinueInfo
    {
        get { return _gameData.ContinueInfo; }
        set { _gameData.ContinueInfo = value; }
    }
    public int CurrentWaveIndex
    {
        get { return _gameData.ContinueInfo.WaveIndex; }
        set { _gameData.ContinueInfo.WaveIndex = value; }
    }


    #endregion

    #region Character
    public Character CurrentCharacter
    {
        get { return _gameData.Characters.Find(c => c.isCurrentCharacter == true); }
    }
    #endregion

    #region Player
    public PlayerController Player { get; set; }
    Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(_moveDir);
        }
    }
    #endregion

    #region Action
    public event Action<Vector2> OnMoveDirChanged;
    #endregion

    #region InGame

    public (int hp, int atk) GetCurrentCharacterStat()
    {
        int hpBonus = 0;
        int atkBonus = 0;
        // var (equipHpBonus, equipAtkBonus) = GetEquipmentBonus();

        // Character ch = CurrentCharacter;

        // hpBonus = (equipHpBonus);
        // atkBonus = (equipAtkBonus);

        return (hpBonus, atkBonus);
    }

    #endregion


    #region Save&Load
    string _path;

    public void SaveGame()
    {
        // if(Player != null)
        // {
        //     _gameData.ContinueInfo.SavedBattleSkillDic = Player.Skills?.SavedBattleSkill;
        //     _gameData.ContinueInfo.SavedSupportSkillList = Player.Skills?.SupportSkills;
        // }
        // string jsonStr = JsonConvert.SerializeObject(_gameData);
        // File.WriteAllText(_path, jsonStr);
    }

    public void ClearContinueData()
    {
        Managers.Game.SoulShopList.Clear();
        ContinueInfo.Clear();
        CurrentWaveIndex = 0;
        SaveGame();
    }

    #endregion
}

