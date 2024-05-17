using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill_01 : RepeatSkill
{
    private CreatureController _owner;
    Rigidbody2D _target;
    Rigidbody2D _rigidBody;

    private void Awake()
    {
        SkillType = Define.SkillType.MonsterSkill_01;
    }

    private void OnEnable()
    {
        _owner = GetComponent<CreatureController>();
        _target = Managers.Game.Player.GetComponent<Rigidbody2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        StopAllCoroutines();
        if (IsLearnedSkill)
            StartCoroutine(SetProjectile());
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        StartCoroutine(SetProjectile());
    }

    public override void OnChangedSkillData()
    {
    }

    IEnumerator SetProjectile()
    {
        while (true)
        {
            Vector3 dirVec = Managers.Game.Player.CenterPosition - _owner.CenterPosition;

            if (dirVec.magnitude > SkillData.ProjRange)
            {
                _owner.CreatureState = Define.CreatureState.Moving;
            }
            else
            {
                _owner.CreatureState = Define.CreatureState.Skill;
                Vector3 startPos = transform.position;
                GenerateProjectile(_owner, SkillData.PrefabLabel, startPos, dirVec.normalized, Vector3.zero, this);
                Managers.Sound.Play(Define.Sound.Effect, "MonsterProjectile_Start");
                yield return new WaitForSeconds(SkillData.CoolTime);
            }
            yield return null;
        }
    }

    protected override void DoSkillJob()
    {
    }
}
