using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class CreatureController : BaseController
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
    public virtual float AtkRate { get; set; } = 1;
    public virtual float Def { get; set; }
    public virtual float DefRate { get; set; }
    public virtual float CriRate { get; set; }
    public virtual float CriDamage { get; set; } = 1.5f;
    public virtual float DamageReduction { get; set; }
    public virtual float MoveSpeedRate { get; set; } = 1;
    public virtual float MoveSpeed { get; set; }
    // public virtual SkillBook Skills{ get; set; }

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
        return true;
    }

    public virtual void UpdateAnimation() { }
}
