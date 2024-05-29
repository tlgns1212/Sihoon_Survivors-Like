using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Move : SequenceSkill
{
    private Rigidbody2D _rb;
    private Coroutine _coroutine;
    private CreatureController _owner;

    private void Awake()
    {
        _owner = GetComponent<CreatureController>();
    }
    
    public override void DoSkill(Action callback = null)
    {
        UpdateSkillData(DataId);
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(CoMove(callback));
    }

    IEnumerator CoMove(Action callback = null)
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.GetChild(0).GetComponent<Animator>().Play(AnimationName);
        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;
            if (elapsed > 3.0f)
                break;

            Vector3 dir = (Managers.Game.Player.CenterPosition - _owner.CenterPosition).normalized;
            Vector2 targetPosition = Managers.Game.Player.CenterPosition +
                                     dir * Random.Range(SkillData.MinCoverage, SkillData.MaxCoverage);

            if (Vector3.Distance(_rb.position, targetPosition) <= 0.1f)
                continue;

            Vector2 dirVec = targetPosition - _rb.position;
            Vector2 nextVec = dirVec.normalized * SkillData.ProjSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + nextVec);

            yield return null;
        }
        callback?.Invoke();
    }
}
