using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using System.Linq;
using UnityEditor.AddressableAssets;
using Unity.Plastic.Newtonsoft.Json;
// using Data;
using System.ComponentModel;
using static Define;
using UnityEngine.Analytics;

public class DataTransformer : EditorWindow
{
    // #if UNITY_EDITOR
    //     [MenuItem("Tools/DeleteGameData ")]
    //     public static void DeleteGameData()
    //     {
    //         PlayerPrefs.DeleteAll();
    //         string path = Application.persistentDataPath + "/SaveData.json";
    //         if (File.Exists(path))
    //             File.Delete(path);
    //     }

    //     [MenuItem("Tools/ParseExcel %#K")]
    //     public static void ParseExcel()
    //     {
    //         ParseSkillData("Skill");
    //         ParseStageData("Stage");
    //         ParseCreatureData("Creature");
    //         ParseLevelData("Level");
    //         ParseEquipmentLevelData("EquipmentLevel");
    //         ParseEquipmentData("Equipment");
    //         ParseMaterialData("Material");
    //         ParseSupportSkillData("SupportSkill");
    //         ParseDropItemData("DropItem");
    //         ParseGachaDataData("GachaTable"); // Dictionary키로 넣을 데이터가 없음 #Neo
    //         ParseStagePackageData("StagePackage");
    //         ParseMissionData("Mission");
    //         ParseAchievementData("Achievement");
    //         ParseCheckOutData("CheckOut");
    //         ParseOfflineRewardData("OfflineReward");
    //         ParseBattlePassData("BattlePass");
    //         ParseDailyShopData("DailyShop");
    //         ParseAccountPassDataData("AccountPass");
    //         //ParseBossData("Boss");
    //         //ParseChapterResourceData("ChapterResource");
    //         Debug.Log("Complete DataTransformer");
    //     }

    //     static void ParseSkillData(string filename)
    //     {
    //         SkillDataLoader loader = new SkillDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');
    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             SkillData skillData = new SkillData();
    //             skillData.DataId = ConvertValue<int>(row[i++]);
    //             skillData.Name = ConvertValue<string>(row[i++]);
    //             skillData.Description = ConvertValue<string>(row[i++]);
    //             skillData.PrefabLabel = ConvertValue<string>(row[i++]);
    //             skillData.IconLabel = ConvertValue<string>(row[i++]);
    //             skillData.SoundLabel = ConvertValue<string>(row[i++]);
    //             skillData.Category = ConvertValue<string>(row[i++]);
    //             skillData.CoolTime = ConvertValue<float>(row[i++]);
    //             skillData.DamageMultiplier = ConvertValue<float>(row[i++]);
    //             skillData.ProjectileSpacing = ConvertValue<float>(row[i++]);
    //             skillData.Duration = ConvertValue<float>(row[i++]);
    //             skillData.RecognitionRange = ConvertValue<float>(row[i++]);
    //             skillData.NumProjectiles = ConvertValue<int>(row[i++]);
    //             skillData.CastingSound = ConvertValue<string>(row[i++]);
    //             skillData.AngleBetweenProj = ConvertValue<float>(row[i++]);
    //             skillData.AttackInterval = ConvertValue<float>(row[i++]);
    //             skillData.NumBounce = ConvertValue<int>(row[i++]);
    //             skillData.BounceSpeed = ConvertValue<float>(row[i++]);
    //             skillData.BounceDist = ConvertValue<float>(row[i++]);
    //             skillData.NumPenerations = ConvertValue<int>(row[i++]);
    //             skillData.CastingEffect = ConvertValue<int>(row[i++]);
    //             skillData.HitSoundLabel = ConvertValue<string>(row[i++]);
    //             skillData.ProbCastingEffect = ConvertValue<float>(row[i++]);
    //             skillData.HitEffect = ConvertValue<int>(row[i++]);
    //             skillData.ProbHitEffect = ConvertValue<float>(row[i++]);
    //             skillData.ProjRange = ConvertValue<float>(row[i++]);
    //             skillData.MinCoverage = ConvertValue<float>(row[i++]);
    //             skillData.MaxCoverage = ConvertValue<float>(row[i++]);
    //             skillData.RoatateSpeed = ConvertValue<float>(row[i++]);
    //             skillData.ProjSpeed = ConvertValue<float>(row[i++]);
    //             skillData.ScaleMultiplier = ConvertValue<float>(row[i++]);
    //             loader.skills.Add(skillData);
    //         }
    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseSupportSkillData(string filename)
    //     {
    //         SupportSkillDataLoader loader = new SupportSkillDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');
    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             SupportSkillData skillData = new SupportSkillData();
    //             skillData.DataId = ConvertValue<int>(row[i++]);
    //             skillData.SupportSkillType = ConvertValue<SupportSkillType>(row[i++]);
    //             skillData.SupportSkillName = ConvertValue<SupportSkillName>(row[i++]);
    //             skillData.SupportSkillGrade = ConvertValue<SupportSkillGrade>(row[i++]);
    //             skillData.Name = ConvertValue<string>(row[i++]);
    //             skillData.Description = ConvertValue<string>(row[i++]);
    //             skillData.IconLabel = ConvertValue<string>(row[i++]);
    //             skillData.HpRegen = ConvertValue<float>(row[i++]);
    //             skillData.HealRate = ConvertValue<float>(row[i++]);
    //             skillData.HealBonusRate = ConvertValue<float>(row[i++]);
    //             skillData.MagneticRange = ConvertValue<float>(row[i++]);
    //             skillData.SoulAmount = ConvertValue<int>(row[i++]);
    //             skillData.HpRate = ConvertValue<float>(row[i++]);
    //             skillData.AtkRate = ConvertValue<float>(row[i++]);
    //             skillData.DefRate = ConvertValue<float>(row[i++]);
    //             skillData.MoveSpeedRate = ConvertValue<float>(row[i++]);
    //             skillData.CriRate = ConvertValue<float>(row[i++]);
    //             skillData.CriDmg = ConvertValue<float>(row[i++]);
    //             skillData.DamageReduction = ConvertValue<float>(row[i++]);
    //             skillData.ExpBonusRate = ConvertValue<float>(row[i++]);
    //             skillData.SoulBonusRate = ConvertValue<float>(row[i++]);
    //             skillData.ProjectileSpacing = ConvertValue<float>(row[i++]);
    //             skillData.Duration = ConvertValue<float>(row[i++]);
    //             skillData.NumProjectiles = ConvertValue<int>(row[i++]);
    //             skillData.AttackInterval = ConvertValue<float>(row[i++]);
    //             skillData.NumBounce = ConvertValue<int>(row[i++]);
    //             skillData.NumPenerations = ConvertValue<int>(row[i++]);
    //             skillData.ProjRange = ConvertValue<float>(row[i++]);
    //             skillData.RoatateSpeed = ConvertValue<float>(row[i++]);
    //             skillData.ScaleMultiplier = ConvertValue<float>(row[i++]);
    //             skillData.Price = ConvertValue<float>(row[i++]);
    //             loader.supportSkills.Add(skillData);
    //         }
    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }
    //     static void ParseStageData(string filename)
    //     {
    //         Dictionary<int, List<WaveData>> waveTable = ParseWaveData("Wave");
    //         StageDataLoader loader = new StageDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');
    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;

    //             StageData stageData = new StageData();
    //             stageData.StageIndex = ConvertValue<int>(row[i++]);
    //             stageData.StageName = ConvertValue<string>(row[i++]);
    //             stageData.StageLevel = ConvertValue<int>(row[i++]);
    //             stageData.MapName = ConvertValue<string>(row[i++]);
    //             stageData.StageSkill = ConvertValue<int>(row[i++]);
    //             stageData.FirstWaveCountValue = ConvertValue<int>(row[i++]);
    //             stageData.FirstWaveClearRewardItemId = ConvertValue<int>(row[i++]);
    //             stageData.FirstWaveClearRewardItemValue = ConvertValue<int>(row[i++]);

    //             stageData.SecondWaveCountValue = ConvertValue<int>(row[i++]);
    //             stageData.SecondWaveClearRewardItemId = ConvertValue<int>(row[i++]);
    //             stageData.SecondWaveClearRewardItemValue = ConvertValue<int>(row[i++]);

    //             stageData.ThirdWaveCountValue = ConvertValue<int>(row[i++]);
    //             stageData.ThirdWaveClearRewardItemId = ConvertValue<int>(row[i++]);
    //             stageData.ThirdWaveClearRewardItemValue = ConvertValue<int>(row[i++]);

    //             stageData.ClearReward_Gold = ConvertValue<int>(row[i++]);
    //             stageData.ClearReward_Exp = ConvertValue<int>(row[i++]);
    //             stageData.StageImage = ConvertValue<string>(row[i++]);
    //             stageData.AppearingMonsters = ConvertList<int>(row[i++]);
    //             waveTable.TryGetValue(stageData.StageIndex, out stageData.WaveArray);

    //             loader.stages.Add(stageData);
    //         }
    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static Dictionary<int, List<WaveData>> ParseWaveData(string filename)
    //     {
    //         Dictionary<int, List<WaveData>> waveTable = new Dictionary<int, List<WaveData>>();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');
    //             if (row.Length == 0)
    //                 continue;

    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             //int respawnID = ConvertValue<int>(row[i++]);
    //             WaveData waveData = new WaveData();
    //             waveData.StageIndex = ConvertValue<int>(row[i++]);
    //             waveData.WaveIndex = ConvertValue<int>(row[i++]);
    //             waveData.SpawnInterval = ConvertValue<float>(row[i++]);
    //             waveData.OnceSpawnCount = ConvertValue<int>(row[i++]);
    //             waveData.MonsterId = ConvertList<int>(row[i++]);
    //             waveData.EleteId = ConvertList<int>(row[i++]);
    //             waveData.BossId = ConvertList<int>(row[i++]);
    //             waveData.RemainsTime = ConvertValue<float>(row[i++]);
    //             waveData.WaveType = ConvertValue<Define.WaveType>(row[i++]);
    //             waveData.FirstMonsterSpawnRate = ConvertValue<float>(row[i++]);
    //             waveData.HpIncreaseRate = ConvertValue<float>(row[i++]);
    //             waveData.nonDropRate = ConvertValue<float>(row[i++]);
    //             waveData.SmallGemDropRate = ConvertValue<float>(row[i++]);
    //             waveData.GreenGemDropRate = ConvertValue<float>(row[i++]);
    //             waveData.BlueGemDropRate = ConvertValue<float>(row[i++]);
    //             waveData.YellowGemDropRate = ConvertValue<float>(row[i++]);
    //             waveData.EliteDropItemId = ConvertList<int>(row[i++]);

    //             if (waveTable.ContainsKey(waveData.StageIndex) == false)
    //                 waveTable.Add(waveData.StageIndex, new List<WaveData>());

    //             waveTable[waveData.StageIndex].Add(waveData);
    //         }
    //         #endregion

    //         return waveTable;
    //     }
    //     static void ParseCreatureData(string filename)
    //     {
    //         CreatureDataLoader loader = new CreatureDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             CreatureData cd = new CreatureData();
    //             cd.DataId = ConvertValue<int>(row[i++]);
    //             cd.DescriptionTextID = ConvertValue<string>(row[i++]);
    //             cd.PrefabLabel = ConvertValue<string>(row[i++]);
    //             cd.MaxHp = ConvertValue<float>(row[i++]);
    //             cd.MaxHpBonus = ConvertValue<float>(row[i++]);
    //             cd.Atk = ConvertValue<float>(row[i++]);
    //             cd.AtkBonus = ConvertValue<float>(row[i++]);
    //             cd.Def = ConvertValue<float>(row[i++]);
    //             cd.MoveSpeed = ConvertValue<float>(row[i++]);
    //             cd.TotalExp = ConvertValue<float>(row[i++]);
    //             cd.HpRate = ConvertValue<float>(row[i++]);
    //             cd.AtkRate = ConvertValue<float>(row[i++]);
    //             cd.DefRate = ConvertValue<float>(row[i++]);
    //             cd.MoveSpeedRate = ConvertValue<float>(row[i++]);
    //             cd.IconLabel = ConvertValue<string>(row[i++]);
    //             cd.SkillTypeList = ConvertList<int>(row[i++]);
    //             loader.creatures.Add(cd);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }
    //     static void ParseLevelData(string filename)
    //     {
    //         LevelDataLoader loader = new LevelDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             LevelData data = new LevelData();
    //             data.Level = ConvertValue<int>(row[i++]);
    //             data.TotalExp = ConvertValue<int>(row[i++]);
    //             loader.levels.Add(data);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }
    //     static void ParseEquipmentLevelData(string filename)
    //     {
    //         EquipmentLevelDataLoader loader = new EquipmentLevelDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             EquipmentLevelData data = new EquipmentLevelData();
    //             data.Level = ConvertValue<int>(row[i++]);
    //             data.UpgradeCost = ConvertValue<int>(row[i++]);
    //             data.UpgradeRequiredItems = ConvertValue<int>(row[i++]);

    //             loader.levels.Add(data);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }
    //     static void ParseEquipmentData(string filename)
    //     {
    //         EquipmentDataLoader loader = new EquipmentDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             EquipmentData ed = new EquipmentData();
    //             ed.DataId = ConvertValue<string>(row[i++]);
    //             ed.GachaRarity = ConvertValue<GachaRarity>(row[i++]);
    //             ed.EquipmentType = ConvertValue<EquipmentType>(row[i++]);
    //             ed.EquipmentGrade = ConvertValue<EquipmentGrade>(row[i++]);
    //             ed.NameTextID = ConvertValue<string>(row[i++]);
    //             ed.DescriptionTextID = ConvertValue<string>(row[i++]);
    //             ed.SpriteName = ConvertValue<string>(row[i++]);
    //             ed.MaxHpBonus = ConvertValue<int>(row[i++]);
    //             ed.MaxHpBonusPerUpgrade = ConvertValue<int>(row[i++]);
    //             ed.AtkDmgBonus = ConvertValue<int>(row[i++]);
    //             ed.AtkDmgBonusPerUpgrade = ConvertValue<int>(row[i++]);
    //             ed.MaxLevel = ConvertValue<int>(row[i++]);
    //             ed.UncommonGradeSkill = ConvertValue<int>(row[i++]);
    //             ed.RareGradeSkill = ConvertValue<int>(row[i++]);
    //             ed.EpicGradeSkill = ConvertValue<int>(row[i++]);
    //             ed.LegendaryGradeSkill = ConvertValue<int>(row[i++]);
    //             ed.BasicSkill = ConvertValue<int>(row[i++]);
    //             ed.MergeEquipmentType1 = ConvertValue<MergeEquipmentType>(row[i++]);
    //             ed.MergeEquipment1 = row[i++];
    //             ed.MergeEquipmentType2 = ConvertValue<MergeEquipmentType>(row[i++]);
    //             ed.MergeEquipment2 = row[i++];
    //             ed.MergedItemCode = row[i++];
    //             ed.LevelupMaterialID = ConvertValue<int>(row[i++]);
    //             ed.DowngradeEquipmentCode = ConvertValue<string>(row[i++]);
    //             ed.DowngradeMaterialCode = ConvertValue<string>(row[i++]);
    //             ed.DowngradeMaterialCount = ConvertValue<int>(row[i++]);
    //             loader.Equipments.Add(ed);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseMaterialData(string filename)
    //     {
    //         MaterialDataLoader loader = new MaterialDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');
    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;

    //             MaterialData material = new MaterialData();
    //             material.DataId = ConvertValue<int>(row[i++]);
    //             material.MaterialType = ConvertValue<Define.MaterialType>(row[i++]);
    //             material.MaterialGrade = ConvertValue<Define.MaterialGrade>(row[i++]);
    //             material.NameTextID = ConvertValue<string>(row[i++]);
    //             material.DescriptionTextID = ConvertValue<string>(row[i++]);
    //             material.SpriteName = ConvertValue<string>(row[i++]);

    //             loader.Materials.Add(material);
    //         }
    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseDropItemData(string filename)
    //     {
    //         DropItemDataLoader loader = new DropItemDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');
    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;

    //             DropItemData dropItem = new DropItemData();
    //             dropItem.DataId = ConvertValue<int>(row[i++]);
    //             dropItem.DropItemType = ConvertValue<Define.DropItemType>(row[i++]);
    //             dropItem.NameTextID = ConvertValue<string>(row[i++]);
    //             dropItem.DescriptionTextID = ConvertValue<string>(row[i++]);
    //             dropItem.SpriteName = ConvertValue<string>(row[i++]);

    //             loader.DropItems.Add(dropItem);
    //         }
    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseGachaDataData(string filename)
    //     {
    //         Dictionary<GachaType, List<GachaRateData>> gachaTable = ParseGachaRateData("GachaTable");
    //         GachaDataLoader loader = new GachaDataLoader();

    //         #region ExcelData
    //         /*        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //                 for (int y = 1; y < lines.Length; y++)
    //                 {
    //                     string[] row = lines[y].Replace("\r", "").Split(',');
    //                     if (row.Length == 0)
    //                         continue;
    //                     if (string.IsNullOrEmpty(row[0]))
    //                         continue;

    //                     int i = 0;

    //                     GachaData gacha = new GachaData();
    //                     gacha.DropItemType = ConvertValue<Define.DropItemType>(row[i++]);

    //                     loader.Gachas.Add(gacha);
    //                 }*/
    //         for (int i = 0; i < gachaTable.Count+1; i++)
    //         {
    //             GachaTableData gachaData = new GachaTableData()
    //             {
    //                 Type = (GachaType)i,
    //             };
    //             if (gachaTable.TryGetValue(gachaData.Type, out List<GachaRateData> gachaRate))
    //                 gachaData.GachaRateTable.AddRange(gachaRate);

    //             loader.GachaTable.Add(gachaData);
    //         }
    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static Dictionary<GachaType, List<GachaRateData>> ParseGachaRateData(string filename)
    //     {
    //         Dictionary<GachaType, List<GachaRateData>> gachaTable = new Dictionary<GachaType, List<GachaRateData>>();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for(int y=1; y<lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');
    //             if (row.Length == 0)
    //                 continue;

    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             GachaType dropType = (GachaType)Enum.Parse(typeof(GachaType), row[i++]);
    //             GachaRateData rateData = new GachaRateData()
    //             {
    //                 EquipmentID = row[i++],
    //                 GachaRate = float.Parse(row[i++]),
    //                 EquipGrade = ConvertValue<EquipmentGrade>(row[i++]),
    //             };

    //             if (gachaTable.ContainsKey(dropType) == false)
    //                 gachaTable.Add(dropType, new List<GachaRateData>());

    //             gachaTable[dropType].Add(rateData);
    //         }
    //         #endregion

    //         return gachaTable;
    //     }

    //     static void ParseStagePackageData(string filename)
    //     {
    //         StagePackageDataLoader loader = new StagePackageDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             StagePackageData stp = new StagePackageData();
    //             stp.StageIndex = ConvertValue<int>(row[i++]);
    //             stp.DiaValue = ConvertValue<int>(row[i++]);
    //             stp.GoldValue = ConvertValue<int>(row[i++]);
    //             stp.RandomScrollValue = ConvertValue<int>(row[i++]);
    //             stp.GoldKeyValue = ConvertValue<int>(row[i++]);
    //             stp.ProductCostValue = ConvertValue<int>(row[i++]);

    //             loader.stagePackages.Add(stp);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseMissionData(string filename)
    //     {
    //         MissionDataLoader loader = new MissionDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             MissionData stp = new MissionData();
    //             stp.MissionId = ConvertValue<int>(row[i++]);
    //             stp.MissionType = ConvertValue<Define.MissionType>(row[i++]);
    //             stp.DescriptionTextID = ConvertValue<string>(row[i++]);
    //             stp.MissionTarget = ConvertValue<Define.MissionTarget>(row[i++]);
    //             stp.MissionTargetValue = ConvertValue<int>(row[i++]);
    //             stp.ClearRewardItmeId = ConvertValue<int>(row[i++]);
    //             stp.RewardValue = ConvertValue<int>(row[i++]);

    //             loader.missions.Add(stp);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseAchievementData(string filename)
    //     {
    //         AchievementDataLoader loader = new AchievementDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             AchievementData ach = new AchievementData();
    //             ach.AchievementID = ConvertValue<int>(row[i++]);
    //             ach.DescriptionTextID = ConvertValue<string>(row[i++]);
    //             ach.MissionTarget = ConvertValue<Define.MissionTarget>(row[i++]);
    //             ach.MissionTargetValue = ConvertValue<int>(row[i++]);
    //             ach.ClearRewardItmeId = ConvertValue<int>(row[i++]);
    //             ach.RewardValue = ConvertValue<int>(row[i++]);

    //             loader.Achievements.Add(ach);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseCheckOutData(string filename)
    //     {
    //         CheckOutDataLoader loader = new CheckOutDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             CheckOutData chk = new CheckOutData();
    //             chk.Day = ConvertValue<int>(row[i++]);
    //             chk.RewardItemId = ConvertValue<int>(row[i++]);
    //             chk.MissionTarRewardItemValuegetValue = ConvertValue<int>(row[i++]);

    //             loader.checkouts.Add(chk);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseOfflineRewardData(string filename)
    //     {
    //         OfflineRewardDataLoader loader = new OfflineRewardDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             OfflineRewardData ofr = new OfflineRewardData();
    //             ofr.StageIndex = ConvertValue<int>(row[i++]);
    //             ofr.Reward_Gold = ConvertValue<int>(row[i++]);
    //             ofr.Reward_Exp = ConvertValue<int>(row[i++]);
    //             ofr.FastReward_Scroll = ConvertValue<int>(row[i++]);
    //             ofr.FastReward_Box = ConvertValue<int>(row[i++]);


    //             loader.offlines.Add(ofr);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseBattlePassData(string filename)
    //     {
    //         BattlePassDataLoader loader = new BattlePassDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             BattlePassData bts = new BattlePassData();
    //             bts.PassLevel = ConvertValue<int>(row[i++]);
    //             bts.FreeRewardItemId = ConvertValue<int>(row[i++]);
    //             bts.FreeRewardItemValue = ConvertValue<int>(row[i++]);
    //             bts.RareRewardItemId = ConvertValue<int>(row[i++]);
    //             bts.RareRewardItemValue = ConvertValue<int>(row[i++]);
    //             bts.EpicRewardItemId = ConvertValue<int>(row[i++]);
    //             bts.EpicRewardItemValue = ConvertValue<int>(row[i++]);


    //             loader.battles.Add(bts);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseDailyShopData(string filename)
    //     {
    //         DailyShopDataLoader loader = new DailyShopDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             DailyShopData dai = new DailyShopData();
    //             dai.Index = ConvertValue<int>(row[i++]);
    //             dai.BuyItemId = ConvertValue<int>(row[i++]);
    //             dai.CostItemId = ConvertValue<int>(row[i++]);
    //             dai.CostValue = ConvertValue<int>(row[i++]);
    //             dai.DiscountValue = ConvertValue<float>(row[i++]);

    //             loader.dailys.Add(dai);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     static void ParseAccountPassDataData(string filename)
    //     {
    //         AccountPassDataLoader loader = new AccountPassDataLoader();

    //         #region ExcelData
    //         string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");

    //         for (int y = 1; y < lines.Length; y++)
    //         {
    //             string[] row = lines[y].Replace("\r", "").Split(',');

    //             if (row.Length == 0)
    //                 continue;
    //             if (string.IsNullOrEmpty(row[0]))
    //                 continue;

    //             int i = 0;
    //             AccountPassData aps = new AccountPassData();
    //             aps.AccountLevel = ConvertValue<int>(row[i++]);
    //             aps.FreeRewardItemId = ConvertValue<int>(row[i++]);
    //             aps.FreeRewardItemValue = ConvertValue<int>(row[i++]);
    //             aps.RareRewardItemId = ConvertValue<int>(row[i++]);
    //             aps.RareRewardItemValue = ConvertValue<int>(row[i++]);
    //             aps.EpicRewardItemId = ConvertValue<int>(row[i++]);
    //             aps.EpicRewardItemValue = ConvertValue<int>(row[i++]);


    //             loader.accounts.Add(aps);
    //         }

    //         #endregion

    //         string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    //         File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    //         AssetDatabase.Refresh();
    //     }

    //     public static T ConvertValue<T>(string value)
    //     {
    //         if (string.IsNullOrEmpty(value))
    //             return default(T);

    //         TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
    //         return (T)converter.ConvertFromString(value);
    //     }

    //     public static List<T> ConvertList<T>(string value)
    //     {
    //         if (string.IsNullOrEmpty(value))
    //             return new List<T>();

    //         return value.Split('&').Select(x => ConvertValue<T>(x)).ToList();
    //     }
    // #endif

}