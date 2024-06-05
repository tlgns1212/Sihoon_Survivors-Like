using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Data
{
    #region LevelData
    [Serializable]
    public class LevelData
    {
        public int Level;
        public int TotalExp;
        public int RequiredExp;
    }
    [Serializable]
    public class LevelDataLoader : ILoader<int, LevelData>
    {
        public List<LevelData> levels = new List<LevelData>();

        public Dictionary<int, LevelData> MakeDict()
        {
            Dictionary<int, LevelData> dict = new Dictionary<int, LevelData>();
            foreach (LevelData level in levels)
                dict.Add(level.Level, level);

            return dict;
        }
    }
    #endregion

    #region CreatureData
    [Serializable]
    public class CreatureData
    {
        public int DataId;
        public string DescriptionTextId;
        public string PrefabLabel;
        public float MaxHp;
        public float MaxHpBonus;
        public float Atk;
        public float AtkBonus;
        public float Def;
        public float MoveSpeed;
        public float TotalExp;
        public float HpRate;
        public float AtkRate;
        public float DefRate;
        public float MoveSpeedRate;
        public string IconLabel;
        public List<int> SkillTypeList;
    }
    [Serializable]
    public class CreatureDataLoader : ILoader<int, CreatureData>
    {
        public List<CreatureData> creatures = new List<CreatureData>();

        public Dictionary<int, CreatureData> MakeDict()
        {
            Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();
            foreach (CreatureData creature in creatures)
                dict.Add(creature.DataId, creature);

            return dict;
        }
    }
    #endregion

    #region SkillData
    [Serializable]
    public class SkillData
    {
        public int DataId;
        public string Name;
        public string Description;
        public string PrefabLabel;
        public string IconLabel;
        public string SoundLabel;
        public string Category;
        public float CoolTime;
        public float DamageMultiplier;
        public float ProjectileSpacing;
        public float Duration;
        public float RecognitionRange;
        public int NumProjectiles;
        public string CastingSound;
        public float AngleBetweenProj;
        public float AttackInterval;
        public int NumBounce;
        public float BounceSpeed;
        public float BounceDist;
        public int NumPenetrations;
        public int CastingEffect;
        public string HitSoundLabel;
        public float ProbCastingEffect;
        public int HitEffect;
        public float ProbHitEffect;
        public float ProjRange;
        public float MinCoverage;
        public float MaxCoverage;
        public float RotateSpeed;
        public float ProjSpeed;
        public float ScaleMultiplier;
    }
    [Serializable]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        public List<SkillData> skills = new List<SkillData>();

        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
            foreach (SkillData skill in skills)
                dict.Add(skill.DataId, skill);

            return dict;
        }
    }
    #endregion

    #region SupportSkillData
    [Serializable]
    public class SupportSkillData
    {
        public int DataId;
        public int AcquiredLevel;
        public Define.SupportSkillName SupportSkillName;
        public Define.SupportSkillGrade SupportSkillGrade;
        public Define.SupportSkillType SupportSkillType;
        public string Name;
        public string Description;
        public string IconLabel;
        public float HpRegen;
        public float HealRate;  // 회복량 (최대HP%)
        public float HealBonusRate; // 회복량 증가
        public float MagneticRange; // 아이템 습득 범위
        public int SoulAmount;  // 영혼 획득
        public float HpRate;
        public float AtkRate;
        public float DefRate;
        public float MoveSpeedRate;
        public float CriRate;
        public float CriDmg;
        public float DamageReduction;
        public float ExpBonusRate;
        public float SoulBonusRate;
        public float ProjectileSpacing; // 발사체 사이 간격
        public float Duration;  // 스킬 지속시간
        public int NumProjectiles;  // 회당 공격횟수
        public float AttackInterval;    // 공격 간격
        public int NumBounce;   // 바운스 횟수
        public int NumPenetrations;  // 관통 횟수
        public float ProjRange; // 투사체 사거리
        public float RotateSpeed;   // 회전 속도
        public float ScaleMultiplier;
        public float Price;
        public bool IsLocked = false;
        public bool IsPurchased = false;

        public bool CheckRecommendationCondition()
        {
            if (IsLocked == true || Managers.Game.SoulShopList.Contains(this) == true)
            {
                return false;
            }

            if (SupportSkillType == Define.SupportSkillType.Special)
            {
                // 내가 가지고 있는 장비스킬이 아니면 false
                if (Managers.Game.EquippedEquipments.TryGetValue(Define.EquipmentType.Weapon, out Equipment myWeapon))
                {
                    int skillId = myWeapon.EquipmentData.BasicSkill;
                    SkillType type = Util.GetSkillTypeFromInt(skillId);

                    switch (SupportSkillName)
                    {
                        case Define.SupportSkillName.ArrowShot:
                        case Define.SupportSkillName.SavageSmash:
                        case Define.SupportSkillName.PhotonStrike:
                        case Define.SupportSkillName.Shuriken:
                        case Define.SupportSkillName.EgoSword:
                            if (SupportSkillName.ToString() != type.ToString())
                                return false;
                            break;
                    }
                }
            }
            #region 서포트 스킬 중복 방지 모드 보류
            // //if (Managers.Game.Player.Skills.SupportSkills.TryGetValue(SupportSkillName, out var existingSkill))
            // //{
            // //    if (existingSkill == null)
            // //        return true;

            // //    if (DataId <= existingSkill.DataId)
            // //    {
            // //        return false;
            // //    }
            // //}
            #endregion
            return true;
        }
    }
    [Serializable]
    public class SupportSkillDataLoader : ILoader<int, SupportSkillData>
    {
        public List<SupportSkillData> supportSkills = new List<SupportSkillData>();

        public Dictionary<int, SupportSkillData> MakeDict()
        {
            Dictionary<int, SupportSkillData> dict = new Dictionary<int, SupportSkillData>();
            foreach (SupportSkillData supportSkill in supportSkills)
                dict.Add(supportSkill.DataId, supportSkill);

            return dict;
        }
    }
    #endregion

    #region StageData
    [Serializable]
    public class StageData
    {
        public int StageIndex = 1;
        public string StageName;
        public int StageLevel = 1;
        public string MapName;
        public int StageSkill;

        public int FirstWaveCountValue;
        public int FirstWaveClearRewardItemId;
        public int FirstWaveClearRewardItemValue;

        public int SecondWaveCountValue;
        public int SecondWaveClearRewardItemId;
        public int SecondWaveClearRewardItemValue;

        public int ThirdWaveCountValue;
        public int ThirdWaveClearRewardItemId;
        public int ThirdWaveClearRewardItemValue;

        public int ClearRewardGold;
        public int ClearRewardExp;
        public string StageImage;
        public List<int> AppearingMonsters;
        public List<WaveData> WaveArray;
    }
    [Serializable]
    public class StageDataLoader : ILoader<int, StageData>
    {
        public List<StageData> stages = new List<StageData>();

        public Dictionary<int, StageData> MakeDict()
        {
            Dictionary<int, StageData> dict = new Dictionary<int, StageData>();
            foreach (StageData stage in stages)
                dict.Add(stage.StageIndex, stage);

            return dict;
        }
    }
    #endregion

    #region WaveData
    [Serializable]
    public class WaveData
    {
        public int StageIndex = 1;
        public int WaveIndex = 1;
        public float SpawnInterval = 0.5f;
        public int OnceSpawnCount;
        public List<int> MonsterId;
        public List<int> EliteId;
        public List<int> BossId;
        public float RemainsTime;
        public Define.WaveType WaveType;
        public float FirstMonsterSpawnRate;
        public float HpIncreaseRate;
        public float NonDropRate;
        public float SmallGemDropRate;
        public float GreenGemDropRate;
        public float BlueGemDropRate;
        public float YellowGemDropRate;
        public List<int> EliteDropItemId;
    }
    [Serializable]
    public class WaveDataLoader : ILoader<int, WaveData>
    {
        public List<WaveData> waves = new List<WaveData>();

        public Dictionary<int, WaveData> MakeDict()
        {
            Dictionary<int, WaveData> dict = new Dictionary<int, WaveData>();
            foreach (WaveData wave in waves)
                dict.Add(wave.WaveIndex, wave);
            return dict;
        }
    }
    #endregion

    #region EquipmentData
    [Serializable]
    public class EquipmentData
    {
        public string DataId;
        public Define.GachaRarity GachaRarity;
        public Define.EquipmentType EquipmentType;
        public Define.EquipmentGrade EquipmentGrade;
        public string NameTextId;
        public string DescriptionTextId;
        public string SpriteName;
        public string HpRegen;
        public int MaxHpBonus;
        public int MaxHpBonusPerUpgrade;
        public int AtkDmgBonus;
        public int AtkDmgBonusPerUpgrade;
        public int MaxLevel;
        public int UncommonGradeSkill;
        public int RareGradeSkill;
        public int EpicGradeSkill;
        public int LegendaryGradeSkill;
        public int BasicSkill;
        public Define.MergeEquipmentType MergeEquipmentType1;
        public string MergeEquipment1;
        public Define.MergeEquipmentType MergeEquipmentType2;
        public string MergeEquipment2;
        public string MergedItemCode;
        public int LevelupMaterialId;
        public string DowngradeEquipmentCode;
        public string DowngradeMaterialCode;
        public int DowngradeMaterialCount;
    }

    [Serializable]
    public class EquipmentDataLoader : ILoader<string, EquipmentData>
    {
        public List<EquipmentData> equipments = new List<EquipmentData>();

        public Dictionary<string, EquipmentData> MakeDict()
        {
            Dictionary<string, EquipmentData> dict = new Dictionary<string, EquipmentData>();
            foreach (EquipmentData equipment in equipments)
                dict.Add(equipment.DataId, equipment);

            return dict;
        }
    }
    #endregion

    #region MaterialData
    [Serializable]
    public class MaterialData
    {
        public int DataId;
        public Define.MaterialType MaterialType;
        public Define.MaterialGrade MaterialGrade;
        public string NameTextId;
        public string DescriptionTextId;
        public string SpriteName;
    }
    [Serializable]
    public class MaterialDataLoader : ILoader<int, MaterialData>
    {
        public List<MaterialData> materials = new List<MaterialData>();

        public Dictionary<int, MaterialData> MakeDict()
        {
            Dictionary<int, MaterialData> dict = new Dictionary<int, MaterialData>();
            foreach (MaterialData material in materials)
                dict.Add(material.DataId, material);

            return dict;
        }
    }
    #endregion

    #region EquipmentLevelData
    [Serializable]
    public class EquipmentLevelData
    {
        public int Level;
        public int UpgradeCost;
        public int UpgradeRequiredItems;
    }
    [Serializable]
    public class EquipmentLevelDataLoader : ILoader<int, EquipmentLevelData>
    {
        public List<EquipmentLevelData> levels = new List<EquipmentLevelData>();

        public Dictionary<int, EquipmentLevelData> MakeDict()
        {
            Dictionary<int, EquipmentLevelData> dict = new Dictionary<int, EquipmentLevelData>();
            foreach (EquipmentLevelData level in levels)
                dict.Add(level.Level, level);

            return dict;
        }
    }
    #endregion

    #region DropItemData
    [Serializable]
    public class DropItemData
    {
        public int DataId;
        public Define.DropItemType DropItemType;
        public string NameTextId;
        public string DescriptionTextId;
        public string SpriteName;
    }
    [Serializable]
    public class DropItemDataLoader : ILoader<int, DropItemData>
    {
        public List<DropItemData> dropItems = new List<DropItemData>();

        public Dictionary<int, DropItemData> MakeDict()
        {
            Dictionary<int, DropItemData> dict = new Dictionary<int, DropItemData>();
            foreach (DropItemData dropItem in dropItems)
                dict.Add(dropItem.DataId, dropItem);

            return dict;
        }
    }
    #endregion

    #region GachaTableData
    [Serializable]
    public class GachaTableData
    {
        public Define.GachaType Type;
        public List<GachaRateData> GachaRateTable = new List<GachaRateData>();
    }
    [Serializable]
    public class GachaTableDataLoader : ILoader<Define.GachaType, GachaTableData>
    {
        public List<GachaTableData> gachaTables = new List<GachaTableData>();

        public Dictionary<Define.GachaType, GachaTableData> MakeDict()
        {
            Dictionary<Define.GachaType, GachaTableData> dict = new Dictionary<Define.GachaType, GachaTableData>();
            foreach (GachaTableData gacha in gachaTables)
                dict.Add(gacha.Type, gacha);

            return dict;
        }
    }
    #endregion

    #region GachaRateData
    public class GachaRateData
    {
        public string EquipmentId;
        public float GachaRate;
        public Define.EquipmentGrade EquipGrade;
    }
    #endregion

    #region MissionData
    public class MissionData
    {
        public int MissionId;
        public Define.MissionType MissionType;
        public string DescriptionTextId;
        public Define.MissionTarget MissionTarget;
        public int MissionTargetValue;
        public int ClearRewardItemId;
        public int RewardValue;
    }
    [Serializable]
    public class MissionDataLoader : ILoader<int, MissionData>
    {
        public List<MissionData> missions = new List<MissionData>();

        public Dictionary<int, MissionData> MakeDict()
        {
            Dictionary<int, MissionData> dict = new Dictionary<int, MissionData>();
            foreach (MissionData mission in missions)
                dict.Add(mission.MissionId, mission);
            return dict;
        }
    }
    #endregion

    #region AchievementData
    [Serializable]
    public class AchievementData
    {
        public int AchievementId;
        public string DescriptionTextId;
        public Define.MissionTarget MissionTarget;
        public int MissionTargetValue;
        public int ClearRewardItemId;
        public int RewardValue;
        public bool IsCompleted;
        public bool IsRewarded;
        public int ProgressValue;
    }
    [Serializable]
    public class AchievementDataLoader : ILoader<int, AchievementData>
    {
        public List<AchievementData> achievements = new List<AchievementData>();

        public Dictionary<int, AchievementData> MakeDict()
        {
            Dictionary<int, AchievementData> dict = new Dictionary<int, AchievementData>();
            foreach (AchievementData achievement in achievements)
                dict.Add(achievement.AchievementId, achievement);

            return dict;
        }
    }
    #endregion
    
    #region CheckOutData
    [Serializable]
    public class CheckOutData
    {
        public int Day;
        public int RewardItemId;
        public int MissionTarRewardItemValueGetValue;
    }
    [Serializable]
    public class CheckOutDataLoader : ILoader<int, CheckOutData>
    {
        public List<CheckOutData> checkouts = new List<CheckOutData>();

        public Dictionary<int, CheckOutData> MakeDict()
        {
            Dictionary<int, CheckOutData> dict = new Dictionary<int, CheckOutData>();
            foreach (CheckOutData checkout in checkouts)
                dict.Add(checkout.Day, checkout);
            return dict;
        }
    }
    #endregion

    #region OfflineRewardData
    [Serializable]
    public class OfflineRewardData
    {
        public int StageIndex;
        public int RewardGold;
        public int RewardExp;
        public int FastRewardScroll;
        public int FastRewardBox;
    }
    [Serializable]
    public class OfflineRewardDataLoader : ILoader<int, OfflineRewardData>
    {
        public List<OfflineRewardData> offlines = new List<OfflineRewardData>();

        public Dictionary<int, OfflineRewardData> MakeDict()
        {
            Dictionary<int, OfflineRewardData> dict = new Dictionary<int, OfflineRewardData>();
            foreach (OfflineRewardData offline in offlines)
                dict.Add(offline.StageIndex, offline);
            return dict;
        }
    }
    #endregion
}