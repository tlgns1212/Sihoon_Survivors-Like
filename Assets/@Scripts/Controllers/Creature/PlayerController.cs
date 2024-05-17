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
                LevelUp(Level);
            }

            OnPlayerDataUpdated?.Invoke();
            // // OnPlayerDataUpdated();
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
            if (Managers.Game.DicMission.TryGetValue(Define.MissionTarget.MonsterKill, out MissionInfo mission))
            {
                mission.Progress = value;
            }
            if (Managers.Game.ContinueInfo.KillCount % 500 == 0)
            {
                Skills.OnMonsterKillBonus();
            }
            OnPlayerDataUpdated?.Invoke();
        }
    }
    public float SoulCount
    {
        get { return Managers.Game.ContinueInfo.SoulCount; }
        set
        {
            Managers.Game.ContinueInfo.SoulCount = Mathf.Round(value);

            OnPlayerDataUpdated?.Invoke();
        }
    }
    public float ExpRatio
    {
        get
        {
            LevelData currentLevelData;
            if (Managers.Data.LevelDataDic.TryGetValue(Level, out currentLevelData))
            {
                int currentLevelExp = currentLevelData.TotalExp;
                int nextLevelExp = currentLevelExp;
                int previousLevelExp = 0;

                LevelData prevLevelData;
                if (Managers.Data.LevelDataDic.TryGetValue(Level - 1, out prevLevelData))
                {
                    previousLevelExp = prevLevelData.TotalExp;
                }

                // 만렙이 아닌 경우
                LevelData nextLevelData;
                if (Managers.Data.LevelDataDic.TryGetValue(Level + 1, out nextLevelData))
                {
                    nextLevelExp = nextLevelData.TotalExp;
                }

                return (float)(Exp - previousLevelExp) / (currentLevelExp - previousLevelExp);
            }

            return 0f;
        }
    }
    public float _ItemCollectDist { get; } = 4.0f;

    #endregion

    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }
    public Vector3 PlayerCenterPos { get { return Indicator.transform.position; } }
    public Vector3 PlayerDirection { get { return (IndicatorSprite.transform.position - PlayerCenterPos).normalized; } }

    private void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        }
    }

    private void Start()
    {
        StartCoroutine(CoSelfRecovery());
        if (Managers.Game.ContinueInfo.isContinue == true)
        {
            LoadSkill();
        }
        else
        {
            InitSkill();
        }
    }

    private void Update()
    {
        UpdatePlayerDirection();
        MovePlayer();
        CollectEnv();
    }

    public override bool Init()
    {
        base.Init();

        ObjectType = Define.ObjectType.Player;

        // event
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;

        // camera
        FindObjectOfType<CameraController>()._playerTransform = gameObject.transform;
        transform.localScale = Vector3.one;
        return true;
    }

    public override void InitSkill()
    {
        base.InitSkill();

        Equipment item;
        Managers.Game.EquippedEquipments.TryGetValue(Define.EquipmentType.Weapon, out item);
        if (item != null)
        {
            // 베이스 스킬
            Define.SkillType type = Util.GetSkillTypeFromInt(item.EquipmentData.BasicSkill);
            if (type != Define.SkillType.None)
            {
                Skills.AddSkill(type, item.EquipmentData.BasicSkill);
                Skills.LevelUpSkill(type);
            }

            Data.SupportSkillData uncommonSkill;
            SupportSkillData rareSkill;
            SupportSkillData epicSkill;
            SupportSkillData legendSkill;
            // 등급별 서포트 스킬
            foreach (Equipment equip in Managers.Game.EquippedEquipments.Values)
            {
                switch (equip.EquipmentData.EquipmentGrade)
                {
                    case Define.EquipmentGrade.UnCommon:
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.UncommonGradeSkill, out uncommonSkill))
                        {
                            Skills.AddSupportSkill(uncommonSkill);
                        }
                        break;
                    case Define.EquipmentGrade.Rare:
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.UncommonGradeSkill, out uncommonSkill))
                        {
                            Skills.AddSupportSkill(uncommonSkill);
                        }
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.RareGradeSkill, out rareSkill))
                        {
                            Skills.AddSupportSkill(rareSkill);
                        }
                        break;
                    case Define.EquipmentGrade.Epic:
                    case Define.EquipmentGrade.Epic1:
                    case Define.EquipmentGrade.Epic2:
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.UncommonGradeSkill, out uncommonSkill))
                        {
                            Skills.AddSupportSkill(uncommonSkill);
                        }
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.RareGradeSkill, out rareSkill))
                        {
                            Skills.AddSupportSkill(rareSkill);
                        }
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.EpicGradeSkill, out epicSkill))
                        {
                            Skills.AddSupportSkill(epicSkill);
                        }
                        break;
                    case Define.EquipmentGrade.Legendary:
                    case Define.EquipmentGrade.Legendary1:
                    case Define.EquipmentGrade.Legendary2:
                    case Define.EquipmentGrade.Legendary3:
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.UncommonGradeSkill, out uncommonSkill))
                        {
                            Skills.AddSupportSkill(uncommonSkill);
                        }
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.RareGradeSkill, out rareSkill))
                        {
                            Skills.AddSupportSkill(rareSkill);
                        }
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.EpicGradeSkill, out epicSkill))
                        {
                            Skills.AddSupportSkill(epicSkill);
                        }
                        if (Managers.Data.SupportSkillDic.TryGetValue(equip.EquipmentData.LegendaryGradeSkill, out legendSkill))
                        {
                            Skills.AddSupportSkill(legendSkill);
                        }
                        break;
                }
            }
        }
    }

    public override void InitCreatureStat(bool isFullHp = true)
    {
        // 현재 캐릭터의 Stat 가져오기
        MaxHp = Managers.Game.CurrentCharacter.MaxHp;
        Atk = Managers.Game.CurrentCharacter.Atk;
        MoveSpeed = CreatureData.MoveSpeed * CreatureData.MoveSpeedRate;

        // 장비 합산 데이터 다 가져오기
        var (equip_hp, equip_attack) = Managers.Game.GetCurrentCharacterStat();
        MaxHp += equip_hp;
        Atk += equip_attack;

        MaxHp *= MaxHpBonusRate;
        Atk *= AttackRate;
        Def *= DefRate;
        MoveSpeed *= MoveSpeedRate;

        if (isFullHp == true)
        {
            Hp = MaxHp;
        }
    }

    public override void UpdatePlayerStat()
    {
        InitCreatureStat(false);

        MaxHp *= MaxHpBonusRate;
        Hp *= MaxHpBonusRate;
        Atk *= AttackRate;
        Def *= DefRate;
        MoveSpeed *= MoveSpeedRate;
    }

    public override void OnDead()
    {
        OnPlayerDead?.Invoke();
    }

    void MovePlayer()
    {
        if (CreatureState == Define.CreatureState.OnDamaged)
            return;

        _rigidBody.velocity = Vector2.zero;

        Vector3 dir = _moveDir * MoveSpeed * Time.deltaTime;
        transform.position += dir;

        if (dir != Vector3.zero)
        {
            Indicator.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * 180 / Mathf.PI);
            OnPlayerMove?.Invoke();
        }
        else
        {
            _rigidBody.velocity = Vector2.zero;
        }
    }

    void UpdatePlayerDirection()
    {
        if (_moveDir.x < 0)
        {
            CreatureSprite.flipX = false;
        }
        else
        {
            CreatureSprite.flipX = true;
        }
    }

    void LevelUp(int level = 0)
    {
        if (Level > 1)
        {
            OnPlayerLevelUp?.Invoke();
        }

        Skills.OnPlayerLevelUpBonus();
    }

    void CollectEnv()
    {
        List<DropItemController> items = Managers.Game.CurrentMap.Grid.GatherObjects(transform.position, _ItemCollectDist + 0.5f);

        foreach (DropItemController item in items)
        {
            Vector3 dir = item.transform.position - transform.position;
            switch (item.ItemType)
            {
                case Define.ObjectType.DropBox:
                case Define.ObjectType.Potion:
                case Define.ObjectType.Magnet:
                case Define.ObjectType.Bomb:
                    if (dir.sqrMagnitude <= item.CollectDist * item.CollectDist)
                    {
                        item.GetItem();
                    }
                    break;
                default:
                    float cd = item.CollectDist * CollectDistBonus;
                    if (dir.sqrMagnitude <= cd * cd)
                    {
                        item.GetItem();
                    }
                    break;
            }
        }
    }

    public override void Healing(float amount, bool isEffect = true)
    {
        if (amount == 0) return;
        float res = ((MaxHp * amount) * HealBonusRate);
        if (res == 0) return;

        Hp = Hp + res;
        Managers.Object.ShowDamageFont(CenterPosition, 0, res, transform);
        if (isEffect)
        {
            Managers.Resource.Instantiate("HealEffect", transform);
        }
    }

    public void OnSafetyZoneExit(BaseController attacker)
    {
        float damage = MaxHp * 0.1f;
        OnDamaged(attacker, null, damage);
        CreatureSprite.color = new Color(1, 1, 1, 0.5f);
        OnPlayerDamaged?.Invoke();
    }

    public void OnSafetyZoneEnter(BaseController attacker)
    {
        CreatureSprite.color = new Color(1, 1, 1, 1f);
    }

    public override void OnDamaged(BaseController attacker, SkillBase skill = null, float damage = 0)
    {
        float totalDamage = 0;
        CreatureController creatureController = attacker as CreatureController;
        if (creatureController != null)
        {
            // 몬스터와 닿았을 때
            if (skill == null)
            {
                totalDamage = creatureController.Atk;
            }
            else    // 몬스터 스킬 맞았을때
            {
                totalDamage = creatureController.Atk + (creatureController.Atk * skill.SkillData.DamageMultiplier);
            }
        }
        else
        {
            totalDamage = damage;
        }

        totalDamage *= 1 - DamageReduction;
        Managers.Game.CameraController.Shake();
        base.OnDamaged(attacker, skill, totalDamage);
    }

    public void OnKnockBack(Vector3 dir)
    {
        // // CreatureState = Define.CreatureState.KnockBack;
        // // Rigidbody.velocity = Vector2.zero();
        // // RigitBody.AddForce(dir * 3, ForceMode2D.Impulse);
    }

    void HandleOnMoveDirChanged(Vector2 dir)
    {
        _moveDir = dir;
    }

    public override void OnDeathAnimationEnd()
    {
    }

    IEnumerator CoSelfRecovery()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Healing(HpRegen, false);
        }
    }
}
