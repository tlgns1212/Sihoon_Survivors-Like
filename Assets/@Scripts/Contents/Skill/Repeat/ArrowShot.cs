using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : RepeatSkill
{
    private void Awake()
    {
        SkillType = Define.SkillType.ArrowShot;
    }



    IEnumerator SetArrowShot()
    {
        string prefabName = SkillData.PrefabLabel;

        if (Managers.Game.Player != null)
        {
            List<MonsterController> targets = Managers.Object.GetNearestMonster(SkillData.NumProjectiles);
            if (targets == null)
                yield break;

            for (int i = 0; i < targets.Count; i++)
            {
                Vector3 dir = Managers.Game.Player.PlayerDirection;
                Vector3 startPos = Managers.Game.Player.CenterPosition;
                GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir, Vector3.zero, this);
            }
        }
    }

    public override void OnChangedSkillData()
    {
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SetArrowShot());
    }
}
