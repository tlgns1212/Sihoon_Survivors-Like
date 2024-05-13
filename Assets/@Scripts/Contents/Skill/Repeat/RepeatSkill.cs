using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
    public override bool Init()
    {
        base.Init();
        return true;
    }

    #region CoSkill
    Coroutine _coSkill;

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        if (_coSkill != null)
            StopCoroutine(_coSkill);

        gameObject.SetActive(true);
        _coSkill = StartCoroutine(CoStartSkill());
    }

    protected abstract void DoSkillJob();

    protected virtual IEnumerator CoStartSkill()
    {
        WaitForSeconds wait = new WaitForSeconds(SkillData.CoolTime); ;

        yield return wait;
        while (true)
        {
            if (SkillData.CoolTime != 0)
                Managers.Sound.Play(Define.Sound.Effect, SkillData.CastingSound);
            DoSkillJob();
            yield return wait;
        }
    }
    #endregion
}
