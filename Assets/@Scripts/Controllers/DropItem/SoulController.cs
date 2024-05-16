using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SoulController : DropItemController
{
    public int _soulCount = 5;
    public override bool Init()
    {
        base.Init();
        // 소울 획득량
        _soulCount = Define.STAGE_SOULCOUNT;
        return true;
    }

    public override void GetItem()
    {
        base.GetItem();
        // if (_coroutine == null && this.IsValid())
        // {
        //     Sequence seq = DOTween.Sequence();
        //     Vector3 dir = (transform.position - Managers.Game.SoulDestination).normalized;
        //     Vector3 target = transform.position + dir * 0.5f;
        //     seq.Append(transform.DOMove(target, 0.4f).SetEase(Ease.Linear)).OnComplete(() =>
        //     {
        //         _coroutine = StartCoroutine(CoMoveToPlayer());
        //     });
        // }
    }

    public IEnumerator CoMoveToPlayer()
    {
        float speed = 17f;
        float acceleration = 8.5f;

        // while (this.IsValid())
        // {
        //     float dist = Vector3.Distance(gameObject.transform.position, Managers.Game.SoulDestination);

        //     // 현재 시간에 따른 속도 계산
        //     speed += acceleration * Time.deltaTime;

        //     // 목적지 방향으로 일정한 속도로 이동
        //     Vector3 direction = (Managers.Game.SoulDestination - transform.position).normalized;
        //     transform.position += direction * speed * Time.deltaTime;

        //     if (dist < 0.4f)
        //     {
        //         string soundName = "SoulGet_01";
        //         Managers.Sound.Play(Define.Sound.Effect, soundName);
        //         Managers.Game.Player.SoulCount += _soulCount * Managers.Game.Player.SoulBonusRate;
        //         Managers.Object.Despawn(this);
        //         yield break;
        //     }

        yield return new WaitForFixedUpdate();
        // }
    }
}
