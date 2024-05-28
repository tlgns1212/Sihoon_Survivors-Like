using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : RepeatSkill
{
    private void Awake()
    {
        SkillType = Define.SkillType.ChainLightning;
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(CoChainLightning());
    }

    IEnumerator CoChainLightning()
    {
        string prefabName = SkillData.PrefabLabel;

        if (Managers.Game.Player != null)
        {
            for (int i = 0; i < SkillData.NumProjectiles; i++)
            {
                Vector3 startPos = Managers.Game.Player.PlayerCenterPos;
                int minDist = (int)SkillData.BounceDist - 1;
                int maxDist = (int)SkillData.BounceDist + 1;
                List<MonsterController> targets = GetChainMonsters(SkillData.NumBounce, minDist, maxDist, index: i);
                if (targets == null)
                    continue;
                for (int j = 0; j < targets.Count; j++)
                {
                    if (j > 0)
                    {
                        startPos = targets[j - 1].CenterPosition;
                    }
                    Vector3 dir = (targets[j].CenterPosition - startPos).normalized;
                    GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir, targets[j].CenterPosition, this);
                }
                yield return null;
            }
        }
    }

    public List<MonsterController> GetChainMonsters(int numTargets, float minDistance, float maxDistance, float angleRange = 180, int index = 0)
    {
        List<MonsterController> chainMonsters = new List<MonsterController>();

        List<MonsterController> nearestMonsters = Managers.Object.GetNearestMonster(SkillData.NumProjectiles, (int)SkillData.ProjRange);
        if (nearestMonsters != null)
        {
            int idx = Mathf.Min(index, nearestMonsters.Count - 1);
            chainMonsters.Add(nearestMonsters[idx]);

            for (int i = 1; i < numTargets; i++)
            {
                MonsterController chainMonster = GetChainMonster(chainMonsters[i - 1].transform.position, minDistance, maxDistance, angleRange, chainMonsters);
                if (chainMonster != null)
                {
                    chainMonsters.Add(chainMonster);
                }
                else
                {
                    break;
                }
            }
        }
        return chainMonsters;
    }

    public MonsterController GetChainMonster(Vector3 origin, float minDistance, float maxDistance, float angleRange, List<MonsterController> ignoreMonsters)
    {
        LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
        Collider2D[] targets = Physics2D.OverlapCircleAll(origin, maxDistance, targetLayer);

        MonsterController closestMonster = null;
        foreach (Collider2D target in targets)
        {
            if (ignoreMonsters.Contains(target.GetComponent<MonsterController>()))
            {
                continue;
            }

            Vector3 targetPosition = target.transform.position;
            float distance = Vector3.Distance(origin, targetPosition);
            if (distance >= minDistance && distance <= maxDistance)
            {
                Vector3 direction = (targetPosition - origin).normalized;
                float angle = Vector3.Angle(direction, Vector3.up);
                closestMonster = target.GetComponent<MonsterController>();
            }
        }
        return closestMonster;
    }
}
