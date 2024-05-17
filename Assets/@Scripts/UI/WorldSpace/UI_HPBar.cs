using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        Bind<GameObject>(typeof(GameObjects));
        return true;
    }

    private void Update()
    {
        Transform parent = transform.parent;
        //transform.position = Camera.main.WorldToScreenPoint(parent.position - Vector3.up * 1.2f);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = Managers.Game.Player.Hp / (float)Managers.Game.Player.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
