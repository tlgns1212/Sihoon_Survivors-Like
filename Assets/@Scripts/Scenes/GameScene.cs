using Data;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class GameScene : BaseScene
{

    GameManager _game;


    #region Action
    public Action<int> OnWaveStart;
    public Action<int> OnSecondChange;
    public Action OnWaveEnd;
    #endregion
    // UI_GameScene _ui;
    // BossController _boss;


    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {

    }

    public override void Clear()
    {

    }

}
