using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class Define
{

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

    public enum SkillType
    {
        None = 0,
        EnergyBolt = 10001, // 10001 ~ 10005
        IcycleArrow = 10011,
        PoisonField = 10021,
        EletronicField = 10031,
        Meteor = 10041,
        FrozenHeard = 10051,
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
