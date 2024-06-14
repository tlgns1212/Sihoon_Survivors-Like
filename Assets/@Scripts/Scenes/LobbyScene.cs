using System;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.LobbyScene;

        //TitleUI
        Managers.UI.ShowSceneUI<UI_LobbyScene>();
        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        Managers.Sound.Play(Define.Sound.Bgm, "Bgm_Lobby");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("LobbyScene F1");
            Managers.Game.Gold += 10000;
            Managers.Game.Dia += 1000;
            Managers.Game.Stamina += 5;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("LobbyScene F2");
            Debug.Log($"ID_BRONZE_KEY = {Managers.Game.ItemDictionary[Define.ID_BRONZE_KEY]}");
            Debug.Log($"ID_GOLD_KEY = {Managers.Game.ItemDictionary[Define.ID_GOLD_KEY]}");
            Debug.Log($"ID_SILVER_KEY = {Managers.Game.ItemDictionary[Define.ID_SILVER_KEY]}");
            Debug.Log($"ID_BELT_SCROLL = {Managers.Game.ItemDictionary[Define.ID_BELT_SCROLL]}");
            Debug.Log($"ID_RING_SCROLL = {Managers.Game.ItemDictionary[Define.ID_RING_SCROLL]}");
            Debug.Log($"ID_ARMOR_SCROLL = {Managers.Game.ItemDictionary[Define.ID_ARMOR_SCROLL]}");
            Debug.Log($"ID_BOOTS_SCROLL = {Managers.Game.ItemDictionary[Define.ID_BOOTS_SCROLL]}");
            Debug.Log($"ID_GLOVES_SCROLL = {Managers.Game.ItemDictionary[Define.ID_GLOVES_SCROLL]}");
            Debug.Log($"ID_WEAPON_SCROLL = {Managers.Game.ItemDictionary[Define.ID_WEAPON_SCROLL]}");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("LobbyScene F3");
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD_KEY], 10);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_BRONZE_KEY], 10);
            Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_SILVER_KEY], 10);
            Managers.Game.AddMaterialItem(Define.ID_WEAPON_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_GLOVES_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_RING_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_BELT_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_ARMOR_SCROLL, 10);
            Managers.Game.AddMaterialItem(Define.ID_BOOTS_SCROLL, 10);
        }
    }

    public override void Clear()
    {

    }

}
