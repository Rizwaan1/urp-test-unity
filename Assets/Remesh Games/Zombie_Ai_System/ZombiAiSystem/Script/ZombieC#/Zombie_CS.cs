using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombie_CS : MonoBehaviour
{
    public WaveSystem waveSystem; // Referentie naar het WaveSystem script

    NavMeshAgent ZombieNavMesh;
    Transform Player;
    Animator Anim;
    [HideInInspector]
    public bool IsMove;
    bool Ready;
    public float Loudness = 1.0f;
    float ReadyTime;

    public enum Movment { Walk, Run };
    public Movment MovementState;
    [HideInInspector]
    public int NumberMovment;

    public enum HealthBar { Hidden, Unhidden };
    public HealthBar HealthBarState;
    [HideInInspector]
    public int NumberHealth;

    public enum DestroyED { Enable, Disable };
    public DestroyED DestroyState;
    [HideInInspector]
    public int NumberDestroy;

    public enum AttackActivation { Enable, Disable };
    public AttackActivation AttackActivationState;
    [HideInInspector]
    public int NumberAttackActivation;

    float LookAtSpeed = 3.0f;
    public string PlayerName;
    public float Damage;
    public float Health = 100.0f;
    float RandomDestroy;
    Slider HealthBarUI;
    float DistanceToPlayer;
    [HideInInspector]
    public bool Spawn;
    Transform Pos;
    [HideInInspector]
    public bool ChackHit;
    [HideInInspector]
    public bool CanISee;

    void Start()
    {
        HealthBarUI = transform.Find("HealthBar_Canvas/Slider").GetComponent<Slider>();
        Pos = transform.Find("Pos").GetComponent<Transform>();
        ZombieNavMesh = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();

        Player = GameObject.Find(PlayerName).transform;
        RandomDestroy = Random.Range(5.0f, 8.0f);
        ChackHit = false;
        if (Spawn == false)
        {
            switch (AttackActivationState)
            {
                case AttackActivation.Enable:
                    {
                        NumberAttackActivation = 0;
                        break;
                    }
                case AttackActivation.Disable:
                    {
                        NumberAttackActivation = 1;
                        break;
                    }
            }

            switch (MovementState)
            {
                case Movment.Walk:
                    {
                        NumberMovment = 0;
                        break;
                    }
                case Movment.Run:
                    {
                        NumberMovment = 1;
                        break;
                    }
            }

            switch (HealthBarState)
            {
                case HealthBar.Unhidden:
                    {
                        NumberHealth = 0;
                        break;
                    }
                case HealthBar.Hidden:
                    {
                        NumberHealth = 1;
                        break;
                    }
            }

            switch (DestroyState)
            {
                case DestroyED.Enable:
                    {
                        NumberDestroy = 0;
                        break;
                    }
                case DestroyED.Disable:
                    {
                        NumberDestroy = 1;
                        break;
                    }
            }
        }

        if (NumberAttackActivation == 0)
        {
            ChackHit = true;
        }

        if (NumberAttackActivation == 1)
        {
            ChackHit = false;
        }
    }

    void Update()
    {
        Anim.SetLayerWeight(Anim.GetLayerIndex("UpperBody"), 1);

        ReaytoFalse();
        HearingRange();

        if (DistanceToPlayer < 2 && ChackHit == true)
        {
            LookAtTarget();
        }

        HealthBarUI.value = Health / 100;

        if (Health > 0.0f)
        {
            CheckAttack();
            SeePlayer();
        }

        Vector3 targetDir = Player.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle < 45.0f && DistanceToPlayer < 10.0f && CanISee == true)
        {
            ChackHit = true;
        }

        DistanceToPlayer = Vector3.Distance(Player.position, transform.position);

        if (NumberMovment == 0)
        {
            if (IsMove == true)
            {
                ZombieNavMesh.speed = 1.5f;
                ZombieNavMesh.destination = Player.position;
                Anim.SetBool("Walk", true);
            }
            if (IsMove == false)
            {
                Anim.SetBool("Walk", false);
                ZombieNavMesh.speed = 0;
            }
        }

        if (NumberMovment == 1)
        {
            if (IsMove == true)
            {
                ZombieNavMesh.speed = 3f;
                ZombieNavMesh.destination = Player.position;
                Anim.SetBool("Run", true);
            }
            if (IsMove == false)
            {
                Anim.SetBool("Run", false);
                ZombieNavMesh.speed = 0;
            }
        }

        if (Health <= 0.0f)
        {
            Death();
        }

        if (NumberHealth == 0 && Health > 0.0f)
        {
            HealthBarUI.gameObject.SetActive(true);
        }

        if (NumberHealth == 1 && Health > 0.0f)
        {
            HealthBarUI.gameObject.SetActive(false);
        }

        MakeNoise ZombieNoise = Player.GetComponent<MakeNoise>();
        if (ZombieNoise != null)
        {
            if (Ready == true && ZombieNoise.Noise == true)
            {
                ChackHit = true;
            }
        }
    }

    void LookAtTarget()
    {
        if (Health > 0.0f)
        {
            var rotation = Quaternion.LookRotation(Player.position - transform.position);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * LookAtSpeed);
        }
    }

    void CheckAttack()
    {
        RaycastHit hit;
        float range = 1.3f;
        Debug.DrawRay(Pos.position, Pos.transform.TransformDirection(Vector3.forward) * range, Color.green);

        if (Physics.Raycast(Pos.position, Pos.transform.TransformDirection(Vector3.forward), out hit, range))
        {
            if (hit.transform.gameObject.name == PlayerName)
            {
                Anim.SetBool("Attack", true);
                IsMove = false;
                LookAtSpeed = 2.0f;
            }
        }
        else
        {
            Anim.SetBool("Attack", false);
            if (ChackHit == true)
            {
                IsMove = true;
                LookAtSpeed = 3.0f;
            }
        }
    }

    public void EventAttack()
    {
        RaycastHit hit;
        float range = 1.3f;
        Debug.DrawRay(Pos.position, Pos.transform.TransformDirection(Vector3.forward) * range, Color.red);

        if (Physics.Raycast(Pos.position, Pos.transform.TransformDirection(Vector3.forward), out hit, range))
        {
            if (hit.transform.gameObject.name == PlayerName)
            {
                hit.transform.gameObject.SendMessage("ApplyDamagezombie", Damage); // Zorg ervoor dat de naam overeenkomt
            }
        }
    }

    void Death()
    {
        Anim.SetBool("Attack", false);
        HealthBarUI.gameObject.SetActive(false);
        Anim.SetBool("Death", true);
        int RandomDeath = Random.Range(1, 5);
        Anim.SetInteger("Death_Int", RandomDeath);
        ZombieNavMesh.speed = 0.0f;
        this.GetComponent<Collider>().enabled = false;
        if (NumberDestroy == 0)
        {
            StartCoroutine(TimeToDestroy());
        }
        waveSystem.ZombieKilled(); // Informeert WaveSystem dat deze zombie is gedood
    }


    IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(RandomDestroy);
        Destroy(gameObject);
    }

    public void MakeNoise(float Loudness)
    {
        if (DistanceToPlayer < Loudness)
        {
            IsMove = true;
        }
    }

    void HearingRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Loudness);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.name == PlayerName)
            {
                Ready = true;
            }
        }
    }

    void ReaytoFalse()
    {
        if (ReadyTime > 0.0f)
        {
            ReadyTime -= Time.deltaTime;
        }
        if (ReadyTime <= 0.0f)
        {
            Ready = false;
            ReadyTime = 1.0f;
        }
    }

    void SeePlayer()
    {
        RaycastHit hit;
        float range = 1000f;
        Vector3 fromPosition = Pos.transform.position;
        Vector3 toPosition = new Vector3(Player.transform.position.x, Player.transform.position.y + 1, Player.transform.position.z);
        Vector3 direction = toPosition - fromPosition;

        Debug.DrawRay(Pos.position, direction, Color.cyan);

        if (Physics.Raycast(Pos.position, direction, out hit, range))
        {
            if (hit.transform.gameObject.name == PlayerName)
            {
                CanISee = true;
            }
            if (hit.transform.gameObject.name != PlayerName)
            {
                CanISee = false;
            }
        }
    }

    public void EnemyDamage(float Damage)
    {
        Health -= Damage;
        IsMove = true;
    }
}
