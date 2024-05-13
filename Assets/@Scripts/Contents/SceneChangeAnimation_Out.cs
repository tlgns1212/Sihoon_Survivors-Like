using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In
public class SceneChangeAnimation_Out : UI_Popup
{
    Animator _anim;
    Action _action;
    Define.Scene _prevScene;

    private void Awake()
    {
        _anim= GetComponent<Animator>();   
    }

    public void SetInfo(Define.Scene prevScene, Action callback)
    {
        transform.localScale = Vector3.one;
        _action = callback;
        _prevScene = prevScene;
        //_anim.Play("LobbyToGame");
    }

    public void OnAnimationComplete()
    {
        //if(_nextScene == Define.Scene.GameScene)
        _action.Invoke();
    }
}
