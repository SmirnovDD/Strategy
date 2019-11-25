using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class ControllerAI : MonoBehaviour
{

    public enum UnitType
    {
        melee,
        archer
    }
    public UnitType unitType;

    public bool isPlayerUnit;
    private NavMeshAgent agent;
    private Transform thisTr;

    //MOVEMENT
    [HideInInspector]
    public Transform targetTr; //позиция врага, к которому движется agent
    [HideInInspector]
    private bool targetSwitched; //вызывается, когда какой то враг становится ближе чем предыдущий, на первом кадре agent.remaining distance не обновляется, так что поставил true, чтобы вызвать метод move
    private float dist; //нужна, потому что agent.remainingDistance долго обрабатывается и равна 0, поэтому применяется Vector3.distance первые кадры

    //ANIMATION
    public Animator anim;

    //ATTACK
    public float damage;
    private bool canChangeTarget = true; //вызывается, когда персонаж заканчивает анимацию атаки (и в начале), нужна, чтобы не было суеты, когда несколько врагов находятся радом с персонажем, и происходит постоянное переключение между ними
    private UnitHealth attackedUnitHealth;
    private bool lerpRotationCourutieneRunning;
    private bool isAttacking; //юнит начал анимацию атаки, но еще не завершил ее
    private ArcherShoot archerShootS;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        thisTr = transform;

        if(unitType == UnitType.archer)
        {
            archerShootS = GetComponent<ArcherShoot>();
        }
    }
    private void OnEnable()
    {
        GameController.OnBattleStarted += CheckIfThereAreNoEnemies;

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
        GameController.OnBattleStarted -= CheckIfThereAreNoEnemies;

        if (isPlayerUnit)
        {
            AllUnitsList.allAllies.Remove(transform);
            AllUnitsList.allAlliesControllers.Remove(this);
            if (AllUnitsList.allAllies.Count == 0 && GameController.battleStarted)
                GameController.BattleEnded = true;
        }
        else
        {
            AllUnitsList.allEnemies.Remove(transform);
            AllUnitsList.allEnemiesControllers.Remove(this);
            if (AllUnitsList.allEnemies.Count == 0 && GameController.battleStarted)
                GameController.BattleEnded = true;
        }
    }

    private void CheckIfThereAreNoEnemies()
    {
        if (isPlayerUnit)
        {
            if (AllUnitsList.allEnemies.Count == 0)
                GameController.BattleEnded = true;
        }
        else
        {
            if (AllUnitsList.allAllies.Count == 0)
                GameController.BattleEnded = true;
        }
    }

    void FixedUpdate()
    {
        if (!GameController.battleStarted || GameController.BattleEnded)
        {
            return;
        }
        if (!targetTr)
        {
            targetTr = SortEnemiesByDistance.SortEnemies(isPlayerUnit, thisTr.position);
            targetSwitched = true;
        }

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
            if (targetTr && !isAttacking)
            {
                canChangeTarget = false;

                anim.SetBool("isMoving", false);                
                anim.SetBool("attack", true);

                if (!lerpRotationCourutieneRunning)
                    StartCoroutine(LerpRotationToFaceTarget());

                attackedUnitHealth = targetTr.gameObject.GetComponent<UnitHealth>(); //запоминаем, кого атаковали во время начала анимации

                isAttacking = true;
            }
        }
    }

    public IEnumerator LerpRotationToFaceTarget()
    {
        lerpRotationCourutieneRunning = true;
        float lerpPercent = 0;
        while(targetTr)
        {
            lerpPercent += 0.1f * Time.deltaTime;
            thisTr.rotation = Quaternion.Lerp(thisTr.rotation, Quaternion.LookRotation(targetTr.position - thisTr.position, Vector3.up), lerpPercent);
            yield return new WaitForFixedUpdate();
        }
    }

    public void Attack() //вызывается из события в анимации
    {
        if (unitType == UnitType.archer)
        {
            archerShootS.Shoot();
        }
        else
        {
            if (attackedUnitHealth)
                DealDamageToEnemy.DealDamage(attackedUnitHealth, damage);
        }

        canChangeTarget = true;

        StopCoroutine(LerpRotationToFaceTarget());

        isAttacking = false;
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
