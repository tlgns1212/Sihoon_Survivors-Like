using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboShot : SequenceSkill
{
    private CreatureController _owner;
    private float _spawnTimer;

    private void Awake()
    {
        SkillType = Define.SkillType.ComboShot;
    }

    public override void DoSkill(Action callback = null)
    {
        CreatureController owner = GetComponent<CreatureController>();
        if (owner.CreatureState != Define.CreatureState.Skill)
            return;

        UpdateSkillData(DataId);

        Vector3 dir = Managers.Game.Player.CenterPosition - _owner.CenterPosition;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        _coroutine = null;
        _coroutine = StartCoroutine(CoSkill(callback));
    }

    private Coroutine _coroutine;

    IEnumerator CoSkill(Action callback = null)
    {
        float angleIncrement = 360f / SkillData.NumProjectiles;
        transform.GetChild(0).GetComponent<Animator>().Play(AnimationName);

        for (int i = 0; i < SkillData.NumProjectiles; i++)
        {
            float angle = i * angleIncrement;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;

            Vector3 startPos = _owner.CenterPosition + dir;
            GenerateProjectile(_owner, SkillData.PrefabLabel, startPos, dir.normalized, Vector3.zero, this);
        }

        yield return new WaitForSeconds(SkillData.AttackInterval);
        
        callback?.Invoke();
    }
}
