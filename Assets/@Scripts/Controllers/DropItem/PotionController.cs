using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : DropItemController
{
    Data.DropItemData _dropItemData;

    public override bool Init()
    {
        base.Init();
        ItemType = Define.ObjectType.Potion;
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
        CollectDist = Define.POTION_COLLECT_DISTANCE;
        GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>(_dropItemData.SpriteName);
    }

    public override void CompleteGetItem()
    {
        // float healAmount = 30;

        // if(Define.DicPotionAmount.TryGetValue(_dropItemData.DataId, out healAmount) == true)
        // {
        //     Managers.Game.Player.Healing(healAmount);
        // }
        // Managers.Object.Despawn(this);
    }
}
