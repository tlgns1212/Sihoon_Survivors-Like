using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class Define
{










    public static float POTION_COLLECT_DISTANCE = 2.6F;
    public static float BOX_COLLECT_DISTANCE = 2.6f;
    public static int MAX_SKILL_LEVEL = 6;
    public static int MAX_SKILL_COUNT = 6;

    #region 넉백 데이터
    public static float KNOCKBACK_TIME = 0.1f; // 밀려나는 시간
    public static float KNOCKBACK_SPEED = 10f; // 속도
    public static float KNOCKBACK_COOLTIME = 0.5f;
    #endregion

    #region 스테이지 관련 데이터
    public static readonly int STAGE_SOULCOUNT = 10;
    public static readonly float STAGE_SOULDROP_RATE = 0.05f;
    #endregion

    #region 가챠 확률
    public static readonly float[] SUPPORTSKILL_GRADE_PROB = new float[]
    {
        0.4f,   // Common 확률
        0.4f,   // Uncommon 확률
        0.1f,   // Rare 확률
        0.07f,  // Epic 확률
        0.03f,  // Legend 확률
    };

    #endregion

    #region 보석 경험치 획득량
    public const int SMALL_EXP_AMOUNT = 1;
    public const int GREEN_EXP_AMOUNT = 2;
    public const int BLUE_EXP_AMOUNT = 5;
    public const int YELLOW_EXP_AMOUNT = 10;
    #endregion

    #region sortingOrder
    public static readonly int UI_GAMESCENE_SORT_CLOSED = 321;
    public static readonly int SOUL_SORT = 105;

    // 소울이 이동중일 때 오더 변경
    public static readonly int UI_GAMESCENE_SORT_OPEN = 323;
    public static readonly int SOUL_SORT_GETITEM = 322;
    #endregion

    #region Enum

    public enum DropItemType
    {
        Potion,
        Magnet,
        DropBox,
        Bomb,
    }

    public enum Scene
    {
        Unknown,
        TitleScene,
        LobbyScene,
        GameScene,
    }

    public enum Sound
    {
        Bgm,
        SubBgm,
        Effect,
        Max,
    }

    public enum UIEvent
    {
        Click,
        Preseed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }

    public enum CreatureState
    {
        Idle,
        Skill,
        Moving,
        OnDamaged,
        Dead
    }

    public enum ObjectType
    {
        Player,
        Monster,
        EliteMonster,
        Boss,
        Projectile,
        Gem,
        Soul,
        Potion,
        DropBox,
        Magnet,
        Bomb,
    }

    public enum GachaRarity
    {
        Normal,
        Special,
    }

    public enum EquipmentType
    {
        Weapon,
        Gloves,
        Ring,
        Belt,
        Armor,
        Boots,
    }

    public enum EquipmentGrade
    {
        None,
        Common,
        UnCommon,
        Rare,
        Epic,
        Epic1,
        Epic2,
        Legendary,
        Legendary1,
        Legendary2,
        Legendary3,
        Myth,
        Myth1,
        Myth2,
        Myth3,
    }

    public enum EquipmentSortType
    {
        Level,
        Grade,
    }

    public enum MergeEquipmentType
    {
        None,
        ItemCode,
        Grade,
    }

    public enum SkillType
    {
        None = 0,
        EnergyBolt = 10001, // 10001 ~ 10005
        IcycleArrow = 10011,
        PoisonField = 10021,
        EletronicField = 10031,
        Meteor = 10041,
        FrozenHeart = 10051,
        WindCutter = 10061,
        EgoSword = 10071,
        ChainLightning = 10081,
        Shuriken = 10091,
        ArrowShot = 10101,
        SavageSmash = 10111,
        PhotonStrike = 10121,
        StormBlade = 10131,
        MonsterSkill_01 = 20091,
        BasicAttack = 100101,
        Move = 100201,
        Charging = 100301,
        Dash = 100401,
        SpinShot = 100501,
        CircleShot = 100601,
        ComboShot = 100701,
    }
    public enum SupportSkillName
    {
        Critical,
        MaxHpBonus,
        ExpBonus,
        SoulBonus,
        DamageReduction,
        AtkBonusRate,
        MoveBonusRate,
        Healing,
        HealBonusRate,
        HpRegen,
        CriticalDamage,
        MagneticRange,
        Resurrection,
        LevelUpMoveSpeed,
        LevelUpReduction,
        LevelUpAtk,
        LevelUpCri,
        levelUpCriDamage,
        MonsterKillAtk,
        MonsterKillMaxHp,
        MonsterKillReduction,
        EliteKillExp,
        EliteKillSoul,
        EnergyBolt,
        IcicleArrow,
        PoisonField,
        Meteor,
        FrozenHeart,
        WindCutter,
        EgoSword,
        ChainLightning,
        Shuriken,
        ArrowShot,
        SavageSmash,
        PhotonStrike,
        StormBlade,
    }

    public enum SupportSkillGrade
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legend,
    }

    public enum SupportSkillType
    {
        General,
        Passive,
        Levelup,
        MonsterKill,
        EliteKill,
        Special,
    }

    public enum MissionType
    {
        Complete,
        Daily,
        Weekly,
        Monthly,
    }

    public enum MissionTarget
    {
        DailyComplete,
        WeeklyComplete,
        MonthlyComplete,
        StageEnter,
        StargeClear,
        EquipmentLevelUp,
        CommonGachaOpen,
        AdvancedGachaOpen,
        OfflineRewardGet,
        FastOfflineRewardGet,
        ShopProductBuy,
        Login,
        EquipmentMerge,
        MonsterAttack,
        MonsterKill,
        EliteMonsterAttack,
        EliteMonsterKill,
        BossKill,
        DailyShopBuy,
        GachaOpen,
        ADWatching,
    }

    #endregion
}
public static class EquipmentUIColors
{
    #region 장비 이름 색상
    public static readonly Color CommonNameColor = HexToColor("A2A2A2");
    public static readonly Color UncommonNameColor = HexToColor("57FF0B");
    public static readonly Color RareNameColor = HexToColor("2471E0");
    public static readonly Color EpicNameColor = HexToColor("9F37F2");
    public static readonly Color LegendaryNameColor = HexToColor("F67B09");
    public static readonly Color MythNameColor = HexToColor("F1331A");
    #endregion
    #region 테두리 색상
    public static readonly Color Common = HexToColor("AC9B83");
    public static readonly Color Uncommon = HexToColor("73EC4E");
    public static readonly Color Rare = HexToColor("0F84FF");
    public static readonly Color Epic = HexToColor("B740EA");
    public static readonly Color Legendary = HexToColor("F19B02");
    public static readonly Color Myth = HexToColor("FC2302");
    #endregion
    #region 배경색상
    public static readonly Color EpicBg = HexToColor("D094FF");
    public static readonly Color LegendaryBg = HexToColor("F8BE56");
    public static readonly Color MythBg = HexToColor("FF7F6E");
    #endregion
}
