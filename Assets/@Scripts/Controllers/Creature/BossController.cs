using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonsterController
{
    public float _chargingTime = 1f;
    public float _rushingTime = 2.0f;
    public float _attackingTime = 1f;
    private Queue<SkillBase> _skillQueue;

    public Vector2 DashPoint { get; set; }

    public override bool Init()
    {
        base.Init();
        ObjectType = Define.ObjectType.Boss;
        // // SpriteRenderer = GetComponent<SpriteRenderer>();
        CreatureState = Define.CreatureState.Skill;
        transform.localScale = new Vector3(2f, 2f, 2f);

        return true;
    }

    public void Start()
    {
        Init();
        CreatureState = Define.CreatureState.Skill;
        Skills.StartNextSequenceSkill();
        InvokeMonsterData();
    }

    public override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                Anim.Play("Idle");
                break;
            case Define.CreatureState.Moving:
                Anim.Play("Move");
                break;
            case Define.CreatureState.Skill:
                break;
            case Define.CreatureState.Dead:
                Skills.StopSkills();
                break;
        }
    }

    public override void InitCreatureStat(bool isFullHp = true)
    {
        MaxHp = (CreatureData.MaxHp + (CreatureData.MaxHpBonus * Managers.Game.CurrentStageData.StageLevel)) * CreatureData.HpRate;
        Atk = (CreatureData.Atk + (CreatureData.AtkBonus * Managers.Game.CurrentStageData.StageLevel)) * CreatureData.AtkRate;
        Hp = MaxHp;
        MoveSpeed = CreatureData.MoveSpeed * CreatureData.MoveSpeedRate;
    }


    // // 쓸모 없어서 주석처리해놨음 필요할 때 쓸 것
    // // public override void OnCollisionEnter2D(Collision2D collision)
    // // {
    // //     base.OnCollisionEnter2D(collision);

    // //     PlayerController target = collision.gameObject.GetComponent<PlayerController>();

    // //     if (target.IsValid() == false)
    // //         return;
    // //     if (this.IsValid() == false)
    // //         return;
    // // }

    // // public override void OnCollisionExit2D(Collision2D collision)
    // // {
    // //     base.OnCollisionExit2D(collision);

    // //     PlayerController target = collision.gameObject.GetComponent<PlayerController>();

    // //     if (target.IsValid() == false)
    // //         return;
    // //     if (this.IsValid() == false)
    // //         return;
    // // }
}
