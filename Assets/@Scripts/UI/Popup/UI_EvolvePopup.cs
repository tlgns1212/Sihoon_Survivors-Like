using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EvolvePopup : UI_Popup
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Refresh();
        return true;
    }

    public void SetInfo()
    {

        Refresh();
    }

    void Refresh()
    {


    }
}
