using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.Timeline;

public abstract class CreatureController : BaseController
{
    [SerializeField]
    protected SpriteRenderer CreatureSprite;
    protected string SpriteName;
    public Material DefaultMat;
    public Material HitEffectMat;
    [SerializeField]
    protected bool isPlayDamagedAnim = false;

    public Rigidbody2D _rigidBody { get; set; }
    public Animator Anim { get; set; }
    public CreatureData CreatureData;

    public virtual int DataId { get; set; }
    public virtual float Hp { get; set; }
    public virtual float MaxHp { get; set; }
    public virtual float MaxHpBonusRate { get; set; } = 1;
    public virtual float HealBonusRate { get; set; } = 1;
    public virtual float HpRegen { get; set; }
    public virtual float Atk { get; set; }
    public virtual float AttackRate { get; set; } = 1;
    public virtual float Def { get; set; }
    public virtual float DefRate { get; set; }
    public virtual float CriRate { get; set; }
    public virtual float CriDamage { get; set; } = 1.5f;
    public virtual float DamageReduction { get; set; }
    public virtual float MoveSpeedRate { get; set; } = 1;
    public virtual float MoveSpeed { get; set; }
    public virtual SkillBook Skills { get; set; }

    private Collider2D _offset;
    public Vector3 CenterPosition
    {
        get
        {
            return _offset.bounds.center;
        }
    }
    public float ColliderRadius { get; set; }

    Define.CreatureState _creatureState = Define.CreatureState.Moving;
    public virtual Define.CreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            _creatureState = value;
            UpdateAnimation();
        }
    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        base.Init();

        Skills = gameObject.GetOrAddComponent<SkillBook>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _offset = GetComponent<Collider2D>();
        ColliderRadius = GetComponent<CircleCollider2D>().radius;

        CreatureSprite = GetComponent<SpriteRenderer>();
        if (CreatureSprite == null)
        {
            CreatureSprite = Util.FindChild<SpriteRenderer>(gameObject);
        }
        Anim = GetComponent<Animator>();
        if (Anim == null)
        {
            Anim = Util.FindChild<Animator>(gameObject);
        }
        return true;
    }

    public virtual void UpdateAnimation() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateDead() { }

    public virtual void OnDamaged(BaseController attacker, SkillBase skill = null, float damage = 0)
    {
        bool isCritical = false;
        PlayerController player = attacker as PlayerController;
        if (player != null)
        {
            // 크리티컬 적용
            if (UnityEngine.Random.value <= player.CriRate)
            {
                damage = damage * player.CriDamage;
                isCritical = true;
            }
        }

        if (skill)
        {
            skill.TotalDamage += damage;
        }
        Hp -= damage;
        Managers.Object.ShowDamageFont(CenterPosition, damage, 0, transform, isCritical);

        if (gameObject.IsValid() || this.IsValid())
        {
            StartCoroutine(PlayDamageAnimation());
        }
    }

    public virtual void OnDead()
    {
        _rigidBody.simulated = false;
        transform.localScale = new Vector3(1, 1, 1);
        CreatureState = Define.CreatureState.Dead;
    }

    public abstract void OnDeathAnimationEnd();

    public virtual void InitCreatureStat(bool isFullHp = true)
    {
        float waveRate = Managers.Game.CurrentWaveData.HpIncreaseRate;
        //보스, 플레이어 빼고 엘리트, 몬스터만
        MaxHp = (CreatureData.MaxHp + (CreatureData.MaxHpBonus * Managers.Game.CurrentStageData.StageLevel)) * (CreatureData.HpRate + waveRate);
        Atk = (CreatureData.Atk + (CreatureData.AtkBonus * Managers.Game.CurrentStageData.StageLevel)) * CreatureData.AtkRate;
        Hp = MaxHp;
        MoveSpeed = CreatureData.MoveSpeed * CreatureData.MoveSpeedRate;
    }

    public virtual void UpdatePlayerStat() { }
    public virtual void Healing(float amount, bool isEffect = true) { }

    public void SetInfo(int creatureId)
    {
        DataId = creatureId;
        Dictionary<int, Data.CreatureData> dict = Managers.Data.CreatureDic;
        CreatureData = dict[creatureId];
        InitCreatureStat();
        Sprite sprite = Managers.Resource.Load<Sprite>(CreatureData.IconLabel);
        CreatureSprite.sprite = sprite;

        Init();
    }

    public void LoadSkill()
    {
        foreach (KeyValuePair<Define.SkillType, int> pair in Managers.Game.ContinueInfo.SavedBattleSkillDic.ToList())
        {
            Skills.LoadSkill(pair.Key, pair.Value);
        }
        foreach (Data.SupportSkillData supportSkill in Managers.Game.ContinueInfo.SavedSupportSkillList.ToList())
        {
            Skills.AddSupportSkill(supportSkill, true);
        }
    }

    public virtual void InitSkill()
    {
        foreach (int skillId in CreatureData.SkillTypeList)
        {
            Define.SkillType type = Util.GetSkillTypeFromInt(skillId);
            if (type != Define.SkillType.None)
            {
                Skills.AddSkill(type, skillId);
            }
        }
    }

    public bool IsMonster()
    {
        switch (ObjectType)
        {
            case Define.ObjectType.Boss:
            case Define.ObjectType.EliteMonster:
            case Define.ObjectType.Monster:
                return true;
            case Define.ObjectType.Player:
            case Define.ObjectType.Projectile:
                return false;
            default:
                return false;
        }
    }

    IEnumerator PlayDamageAnimation()
    {
        if (isPlayDamagedAnim == false)
        {
            isPlayDamagedAnim = true;
            DefaultMat = Managers.Resource.Load<Material>("CreatureDefaultMat");
            HitEffectMat = Managers.Resource.Load<Material>("PaintWhite");
            // Damaged Animation
            CreatureSprite.material = HitEffectMat;
            yield return new WaitForSeconds(0.1f);
            CreatureSprite.material = DefaultMat;

            if (Hp <= 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                switch (ObjectType)
                {
                    case Define.ObjectType.Player:
                        // 부활 카운트 확인
                        SupportSkillData resurrection = Skills.SupportSkills
                            .FirstOrDefault(skill => skill.SupportSkillName == Define.SupportSkillName.Resurrection);

                        if (resurrection == null)
                        {
                            OnDead();
                        }
                        else
                        {
                            Resurrection(resurrection.HealRate, resurrection.MoveSpeedRate, resurrection.AtkRate);

                            Skills.SupportSkills.Remove(resurrection);
                            Skills.OnSkillBookChanged();
                        }
                        break;
                    default:
                        OnDead();
                        break;
                }
            }
            isPlayDamagedAnim = false;
        }
    }

    public void Resurrection(float healRate, float moveSpeed = 0, float atkRate = 0)
    {
        Healing(healRate, false);
        Managers.Resource.Instantiate("Revival", transform);
        MoveSpeedRate += moveSpeed;
        AttackRate += atkRate;
        UpdatePlayerStat();
        Managers.Object.KillAllMonsters();
    }

}
