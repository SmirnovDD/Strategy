using UnityEngine;

public class SortEnemiesByDistance : MonoBehaviour
{
    private static Transform closestTargetTransform;
    public static Transform SortEnemies(bool isPlayersUnit, Vector3 position)
    {
        if (isPlayersUnit)
        {
            if (AllUnitsList.allEnemies.Count > 0)
            {
                closestTargetTransform = AllUnitsList.allEnemies[0];

                for (int i = 1; i < AllUnitsList.allEnemies.Count; i++)
                {
                    if (Vector3.Distance(position, closestTargetTransform.position) > Vector3.Distance(position, AllUnitsList.allEnemies[i].position))
                        closestTargetTransform = AllUnitsList.allEnemies[i];
                }
                return closestTargetTransform;
            }
            else
            {
                return null;
            }
        }
        else
        {
            if (AllUnitsList.allAllies.Count > 0)
            {
                closestTargetTransform = AllUnitsList.allAllies[0];

                for (int i = 1; i < AllUnitsList.allAllies.Count; i++)
                {
                    if (Vector3.Distance(position, closestTargetTransform.position) > Vector3.Distance(position, AllUnitsList.allAllies[i].position))
                        closestTargetTransform = AllUnitsList.allAllies[i];
                }
                return closestTargetTransform;
            }
            else
            {
                return null;
            }
        }
    }
}
