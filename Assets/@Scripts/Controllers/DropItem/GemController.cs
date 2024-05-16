using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GemInfo
{
    public enum GemType
    {
        Small,
        Green,
        Blue,
        Yellow,
    }

    public GemType Type;
    public string SpriteName;
    public Vector3 GemScale;
    public int ExpAmount;

    public GemInfo(GemType type, Vector3 gemScale)
    {
        Type = type;
        SpriteName = $"{type}Gem.sprite";
        GemScale = gemScale;
        switch (type)
        {
            case GemType.Small:
                ExpAmount = Define.SMALL_EXP_AMOUNT;
                break;
            case GemType.Green:
                ExpAmount = Define.GREEN_EXP_AMOUNT;
                break;
            case GemType.Blue:
                ExpAmount = Define.BLUE_EXP_AMOUNT;
                break;
            case GemType.Yellow:
                ExpAmount = Define.YELLOW_EXP_AMOUNT;
                break;
        }
    }
}

public class GemController : DropItemController
{
    GemInfo _gemInfo;

    public override bool Init()
    {
        base.Init();
        ItemType = Define.ObjectType.Gem;
        return true;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    public void SetInfo(GemInfo gemInfo)
    {
        _gemInfo = gemInfo;
        Sprite sprite = Managers.Resource.Load<Sprite>($"{_gemInfo.SpriteName}");
        GetComponent<SpriteRenderer>().sprite = sprite;
        transform.localScale = _gemInfo.GemScale;
    }

    public override void GetItem()
    {
        base.GetItem();
        if (_coroutine == null && this.IsValid())
        {
            Sequence seq = DOTween.Sequence();
            Vector3 dir = (transform.position - Managers.Game.Player.PlayerCenterPos).normalized;
            Vector3 target = gameObject.transform.position + dir * 1.5f;
            seq.Append(transform.DOMove(target, 0.3f).SetEase(Ease.Linear)).OnComplete(() =>
            {
                _coroutine = StartCoroutine(CoMoveToPlayer());
            });
        }
    }

    public IEnumerator CoMoveToPlayer()
    {
        while (this.IsValid() == true)
        {
            float dist = Vector3.Distance(gameObject.transform.position, Managers.Game.Player.PlayerCenterPos);

            transform.position = Vector3.MoveTowards(transform.position, Managers.Game.Player.PlayerCenterPos, Time.deltaTime * 30.0f);

            if (dist < 0.4f)
            {
                string soundName = Random.value > 0.5f ? "ExpGet_01" : "ExpGet_02";
                Managers.Sound.Play(Define.Sound.Effect, soundName);
                Managers.Game.Player.Exp += _gemInfo.ExpAmount * Managers.Game.Player.ExpBonusRate;
                // Managers.Object.Despawn(this);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
