using System.Collections;
using UnityEngine;

public class SortDistancesForAllUnits : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        for(int i = 0; i < AllUnitsList.allAllies.Count; i++)
        {
            Transform targetTr = SortEnemiesByDistance.SortEnemies(AllUnitsList.allAlliesControllers[i].isPlayerUnit, AllUnitsList.allAllies[i].position);
            if(AllUnitsList.allAlliesControllers[i].targetTr != targetTr)
            {
                AllUnitsList.allAlliesControllers[i].targetTr = targetTr;
                AllUnitsList.allAlliesControllers[i].UpdateTarget();
            }

            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < AllUnitsList.allEnemies.Count; i++)
        {
            Transform targetTr = SortEnemiesByDistance.SortEnemies(AllUnitsList.allEnemiesControllers[i].isPlayerUnit, AllUnitsList.allEnemies[i].position);
            if (AllUnitsList.allEnemiesControllers[i].targetTr != targetTr)
            {
                AllUnitsList.allEnemiesControllers[i].targetTr = targetTr;
                AllUnitsList.allEnemiesControllers[i].UpdateTarget();
            }

            yield return new WaitForFixedUpdate();
        }

        if(!GameController.BattleEnded) //после конца боя, если оба отряда были уничтожены будет переполнение памяти, т.к. оба списка врагов будут пустыми, если не сделать проверку и не менять переменную
            StartCoroutine(Start());
    }
}
