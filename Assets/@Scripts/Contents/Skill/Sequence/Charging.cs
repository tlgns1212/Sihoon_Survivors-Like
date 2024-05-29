using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charging : SequenceSkill
{
    private void Awake()
    {
        SkillType = Define.SkillType.Charging;
        AnimationName = "Charge";
    }
    public override void DoSkill(Action callback = null)
    {
        CreatureController owner = GetComponent<CreatureController>();
        if (owner.CreatureState != Define.CreatureState.Skill)
            return;

        UpdateSkillData(DataId);

        _coroutine = null;
        _coroutine = StartCoroutine(CoSkill(callback));
    }

    Coroutine _coroutine;
    IEnumerator CoSkill(Action callback = null)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.GetChild(0).GetComponent<Animator>().Play(AnimationName);
        yield return new WaitForSeconds(SkillData.AttackInterval);
        callback?.Invoke();
    }
}
