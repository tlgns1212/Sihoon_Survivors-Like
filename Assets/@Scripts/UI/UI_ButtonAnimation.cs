using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class UI_ButtonAnimation : UI_Base
{

    void Start()
    {
        gameObject.BindEvent(ButtonPointerDownAnimation, type: Define.UIEvent.PointerDown);
        gameObject.BindEvent(ButtonPointerUpAnimation, type: Define.UIEvent.PointerUp);
    }

    public void ButtonPointerDownAnimation()
    {
        transform.DOScale(0.85f, 0.1f).SetEase(Ease.InOutBack).SetUpdate(true); 
    }

    public void ButtonPointerUpAnimation()
    {
        transform.DOScale(1f, 0.1f).SetEase(Ease.InOutSine).SetUpdate(true); 
    }
}
