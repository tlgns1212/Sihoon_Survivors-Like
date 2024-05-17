using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;

public static class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius = 6, float maxRadius = 12)
    {
        float randomDist = UnityEngine.Random.Range(minRadius, maxRadius);

        Vector2 randomDir = new Vector2(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100)).normalized;
        //Debug.Log(randomDir);
        var point = origin + randomDir * randomDist;
        return point;
    }

    public static Vector2 GenerateMonsterSpawnPosition(Vector2 characterPosition, float minSpawnDistance = 20f, float maxSpawnDistance = 25f)
    {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        float xDist = Mathf.Cos(angle) * distance;
        float yDist = Mathf.Sin(angle) * distance;

        // 원모양으로 생성
        Vector2 spawnPosition = characterPosition + new Vector2(xDist, yDist);

        // 맵 경계를 벗어나는 경우 타원 모양으로 생성
        float size = Managers.Game.CurrentMap.MapSize.x * 0.5f;
        if (Mathf.Abs(spawnPosition.x) > size || Mathf.Abs(spawnPosition.y) > size)
        {
            float ellipseFactorX = Mathf.Lerp(1f, 0.5f, Mathf.Abs(characterPosition.x) / size);
            float ellipseFactorY = Mathf.Lerp(1f, 0.5f, Mathf.Abs(characterPosition.y) / size);

            xDist *= ellipseFactorX;
            yDist *= ellipseFactorY;

            spawnPosition = Vector2.zero + new Vector2(xDist, yDist);

            // 생성 위치를 맵 사이즈 범위 내로 조정
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -size, size);
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, -size, size);
        }

        return spawnPosition;
    }

    public static Color HexToColor(string color)
    {
        Color parsedColor;
        ColorUtility.TryParseHtmlString("#" + color, out parsedColor);

        return parsedColor;
    }

    // Enum값 중 랜덤값 변환
    public static T GetRandomEnumValue<T>() where T : struct, Enum
    {
        Type type = typeof(T);
        if (!_enumDict.ContainsKey(type))
        {
            _enumDict[type] = Enum.GetValues(type);
        }

        Array values = _enumDict[type];

        int index = Random.Range(0, values.Length);
        return (T)values.GetValue(index);
    }


    //string값 으로 Enum값 찾기
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static SkillType GetSkillTypeFromInt(int value)
    {
        foreach (SkillType skillType in Enum.GetValues(typeof(SkillType)))
        {
            int minValue = (int)skillType;
            int maxValue = minValue + 5; // 10501 ~ 10506 사이 값이면 10501리턴

            if (value >= minValue && value <= maxValue)
            {
                return skillType;
            }
        }
        Debug.LogError($"Failed to Add Skill : {value}");
        return SkillType.None;
    }

}
