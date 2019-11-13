using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ControllerAI : MonoBehaviour
{
    public bool isPlayerUnit;
    private NavMeshAgent agent;
    private Transform thisTr;

    //MOVEMENT
    public Transform targetTr; //позиция врага, к которому движется agent
    [HideInInspector]
    private bool targetSwitched; //вызывается, когда какой то враг становится ближе чем предыдущий, на первом кадре agent.remaining distance не обновляется, так что поставил true, чтобы вызвать метод move
    private float dist; //нужна, потому что agent.remainingDistance долго обрабатывается и равна 0, поэтому применяется Vector3.distance первые кадры

    //ANIMATION
    public Animator anim;

    //ATTACK
    public float damage;
    private bool canChangeTarget = true; //вызывается, когда персонаж заканчивает анимацию атаки (и в начале), нужна, чтобы не было суеты, когда несколько врагов находятся радом с персонажем, и происходит постоянное переключение между ними

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        thisTr = transform;
    }
    private void OnEnable()
    {
        if (isPlayerUnit)
        {
            AllUnitsList.allAllies.Add(transform);
            AllUnitsList.allAlliesControllers.Add(this);
        }
        else
        {
            AllUnitsList.allEnemies.Add(transform);
            AllUnitsList.allEnemiesControllers.Add(this);
        }

    }
    private void OnDisable()
    {
        if (isPlayerUnit)
        {
            AllUnitsList.allAllies.Remove(transform);
            AllUnitsList.allAlliesControllers.Remove(this);
            if (AllUnitsList.allAllies.Count == 0)
                GameController.BattleEnded = true;
        }
        else
        {
            AllUnitsList.allEnemies.Remove(transform);
            AllUnitsList.allEnemiesControllers.Remove(this);
            if (AllUnitsList.allEnemies.Count == 0)
                GameController.BattleEnded = true;
        }
    }

    void FixedUpdate()
    {
        if (!GameController.battleStarted)
            return;

        if (!targetTr)
        {
            targetTr = SortEnemiesByDistance.SortEnemies(isPlayerUnit, thisTr.position);
            targetSwitched = true;
        }
        if (gameObject.name == "test")
            Debug.DrawLine(thisTr.position, targetTr.position, Color.red);

        if (agent.remainingDistance == 0)
        {
            dist = Vector3.Distance(thisTr.position, targetTr.position);
        }
        else
        {
            dist = agent.remainingDistance;
        }

        if ((dist >= agent.stoppingDistance || targetSwitched) && canChangeTarget)
        {
            MoveToTarget();
            targetSwitched = false;
        }
        else
        {
            if (targetTr)
            {
                canChangeTarget = false;
                thisTr.LookAt(targetTr);
                anim.SetBool("isMoving", false);
                anim.SetBool("attack", true);
            }
        }
    }

    public void Attack() //вызывается из события в анимации
    {
        if (isPlayerUnit && AllUnitsList.allEnemies.Count > 0)
            DealDamageToEnemy.DealDamage(AllUnitsList.allEnemies[0].gameObject.GetComponent<UnitHealth>(), damage);
        else if(!isPlayerUnit && AllUnitsList.allAllies.Count > 0)
            DealDamageToEnemy.DealDamage(AllUnitsList.allAllies[0].gameObject.GetComponent<UnitHealth>(), damage);

        canChangeTarget = true;
    }

    public void UpdateTarget()
    {
        targetSwitched = true;
    }
    private void MoveToTarget()
    {
        anim.SetBool("isMoving", true);
        anim.SetBool("attack", false);

        if (targetTr != null)
        {
            agent.SetDestination(targetTr.position);
        }
    }
}
