using Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.LevelData> LevelDataDic { get; private set; } = new Dictionary<int, Data.LevelData>();
    public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();
    public Dictionary<int, Data.CreatureData> CreatureDic { get; private set; } = new Dictionary<int, Data.CreatureData>();
    public Dictionary<string, Data.EquipmentData> EquipDataDic { get; private set; } = new Dictionary<string, Data.EquipmentData>();
    public Dictionary<int, Data.EquipmentLevelData> EquipLevelDataDic { get; private set; } = new Dictionary<int, Data.EquipmentLevelData>();
    public Dictionary<int, AchievementData> AchievementDataDic { get; private set; } = new Dictionary<int, AchievementData>();



    public void Init()
    {
        LevelDataDic = LoadJson<Data.LevelDataLoader, int, Data.LevelData>("LevelData").MakeDict();
        SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData").MakeDict();
        CreatureDic = LoadJson<Data.CreatureDataLoader, int, Data.CreatureData>("CreatureData").MakeDict();
        EquipDataDic = LoadJson<Data.EquipmentDataLoader, string, Data.EquipmentData>("EquipmentData").MakeDict();
        EquipLevelDataDic = LoadJson<Data.EquipmentLevelDataLoader, int, Data.EquipmentLevelData>("EquipmentLevelData").MakeDict();
        AchievementDataDic = LoadJson<Data.AchievementDataLoader, int, Data.AchievementData>("AchievementData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }


}
