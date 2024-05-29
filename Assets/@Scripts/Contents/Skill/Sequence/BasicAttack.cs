using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : SequenceSkill
{
    private void Awake()
    {
        SkillType = Define.SkillType.BasicAttack;
        AnimationName = "Attack";
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
        // 스킬 범위 가이드라인
        GameObject obj = Managers.Resource.Instantiate("SkillRange", pooling: true);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        SkillRange sr = obj.GetComponent<SkillRange>();
        float radius = SkillData.ProjRange;
        float wait = sr.SetCircle(radius * 2);
        transform.GetChild(0).GetComponent<Animator>().Play(AnimationName);
        yield return new WaitForSeconds(wait);

        Managers.Resource.Destroy(obj);

        // 플레이어랑 나랑 거리가 radius 이하면 데미지 주기
        // 1. 타겟 콜라이더 반지름 targetRadius
        float targetRadius = Managers.Game.Player.ColliderRadius;
        // 2. 스킬 범위 반지름 radius

        // 두 포지션의 거리가 반지름의 합보다 작으면
        if (Vector3.Distance(transform.position, Managers.Game.Player.CenterPosition) < radius + targetRadius)
        {
            Managers.Game.Player.OnDamaged(Owner, this, 0);
        }

        // Hit Effect
        GameObject hitEffectObj = Managers.Resource.Instantiate("BossSmashHitEffect", pooling: true);
        hitEffectObj.transform.SetParent(transform);
        hitEffectObj.transform.localPosition = Vector3.zero;
        hitEffectObj.transform.localScale = Vector3.one * radius * 0.3f;
        yield return new WaitForSeconds(0.7f);
        Managers.Resource.Destroy(hitEffectObj);

        yield return new WaitForSeconds(SkillData.AttackInterval);

        callback?.Invoke();
    }
}
