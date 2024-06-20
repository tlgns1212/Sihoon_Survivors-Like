using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBoxController : DropItemController
{
    public override bool Init()
    {
        base.Init();
        CollectDist = Define.BOX_COLLECT_DISTANCE;
        ItemType = Define.ObjectType.DropBox;
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

    public void SetInfo()
    {

    }

    public override void CompleteGetItem()
    {
        UI_LearnSkillPopup popup = Managers.UI.ShowPopupUI<UI_LearnSkillPopup>();
        popup.SetInfo();

        Managers.Object.Despawn(this);
    }
}
