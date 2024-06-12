using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CoolTimeBar : UI_Base
{
    enum GameObjects
    {
        CoolTimeBar
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindObject(typeof(GameObjects));
        return true;
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = Camera.main.WorldToScreenPoint(parent.position - Vector3.up * 2f);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = Managers.Game.Player.Hp / (float)Managers.Game.Player.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.CoolTimeBar).GetComponent<Slider>().value = ratio;
    }
}
