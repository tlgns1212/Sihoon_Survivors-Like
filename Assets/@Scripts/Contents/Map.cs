using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer SpriteBackground;
    [SerializeField]
    public SpriteRenderer SpritePattern;
    [SerializeField]
    public BoxCollider2D[] BorderCollider;
    [SerializeField]
    public GridController Grid;
    [NonSerialized]
    public Color BackgroundColor;
    [NonSerialized]
    public Color PatternColor;
    public Vector2 MapSize
    {
        get { return SpriteBackground.size; }
        set { SpriteBackground.size = value; }
    }

    public void Init()
    {
        // Managers.Game.CurrentMap = this;
    }

    public void ChangeMapSize(float targetRate, float time = 120)
    {
        Vector3 currentSize = Vector3.one * 20f;
        if (Managers.Game.CurrentWaveIndex > 7)
            return;
    }

}
