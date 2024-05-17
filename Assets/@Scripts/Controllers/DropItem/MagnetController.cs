using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : DropItemController
{
    Data.DropItemData _dropItemData;

    public override bool Init()
    {
        base.Init();
        ItemType = Define.ObjectType.Magnet;
        return true;
    }

    public override void GetItem()
    {
        base.GetItem();
        if (_coroutine == null && this.IsValid())
        {
            _coroutine = StartCoroutine(CoCheckDistance());
        }
    }

    public void SetInfo(Data.DropItemData data)
    {
        _dropItemData = data;
        CollectDist = Define.BOX_COLLECT_DISTANCE;
        GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>(_dropItemData.SpriteName);
    }

    public override void CompleteGetItem()
    {
        Managers.Object.CollectAllItems();
        Managers.Object.Despawn(this);
    }
}
