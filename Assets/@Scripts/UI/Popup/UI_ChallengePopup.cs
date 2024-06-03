using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChallengePopup : UI_Popup
{
    #region Enum

    enum Texts
    {
        UnlockInfoText,
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindText(typeof(Texts));

        Refresh();
        return true;
    }

    public void SetInfo()
    {
        
    }

    void Refresh()
    {
        
    }
}
