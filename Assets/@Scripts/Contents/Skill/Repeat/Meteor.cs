using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : RepeatSkill
{
    private void Awake()
    {
        SkillType = Define.SkillType.Meteor;
    }

    public override void OnChangedSkillData()
    {

    }

    IEnumerator GenerateMeteor()
    {
        List<MonsterController> targets = Managers.Object.GetMonsterWithinCamera(SkillData.NumProjectiles);
        if (targets == null)
            yield break;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].IsValid() == true)
            {
                Vector2 startPos = GetMeteorPosition(targets[i].CenterPosition);
                GenerateProjectile(Managers.Game.Player, "MeteorProjectile", startPos, Vector3.zero, targets[i].CenterPosition, this);
                yield return new WaitForSeconds(SkillData.AttackInterval);
            }
        }
    }

    public Vector2 GetMeteorPosition(Vector3 target)
    {
        float angleInRadians = 60f * Mathf.Deg2Rad;
        float spawnMargin = 1f;
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;

        float spawnX = target.x + (halfWidth + spawnMargin) * Mathf.Cos(angleInRadians);
        float spawnY = target.y + (halfHeight + spawnMargin) * Mathf.Sin(angleInRadians);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        return spawnPosition;
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(GenerateMeteor());
    }
}
