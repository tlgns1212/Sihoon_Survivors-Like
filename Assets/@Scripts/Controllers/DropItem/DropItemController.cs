using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemController : BaseController
{
    public float CollectDist { get; set; } = 4.0f;
    public Coroutine _coroutine;
    public Define.ObjectType ItemType;
    public override bool Init()
    {
        base.Init();
        return true;
    }

    virtual public void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    public void OnEnable()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Define.SOUL_SORT;
    }

    public virtual void GetItem()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Define.SOUL_SORT_GETITEM;
        Managers.Game.CurrentMap.Grid.Remove(this);
    }

    public virtual void CompleteGetItem() { }

    public IEnumerator CoCheckDistance()
    {
        while (this.IsValid() == true)
        {
            float dist = Vector3.Distance(gameObject.transform.position, Managers.Game.Player.PlayerCenterPos);

            transform.position = Vector3.MoveTowards(transform.position, Managers.Game.Player.PlayerCenterPos, Time.deltaTime * 15.0f);
            if (dist < 1f)
            {
                CompleteGetItem();
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

}

