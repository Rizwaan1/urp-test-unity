using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NewZombieScript : MonoBehaviour
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

    // Add these public fields to set the money rewards
    public int moneyPerHit = 10;
    public int moneyOnDeath = 50;

    private MoneyManager moneyManager;
    private bool isDead = false; // Flag to track if the zombie is dead

    // Add fields for drop system
    public List<GameObject> dropItems; // The list of prefabs to drop
    [Range(0, 100)]
    public float dropChance = 30.0f; // Drop chance percentage

    // Public fields to set movement speed and attack range
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.0f;
    public float attackRange = 1.3f;

    // Arrays for random attack animations
    public string[] attackAnimations;

    private string currentAttackAnim;

    void Start()
    {
        // Choose random attack animation at the start
        currentAttackAnim = attackAnimations[Random.Range(0, attackAnimations.Length)];

        HealthBarUI = transform.Find("HealthBar_Canvas/Slider")?.GetComponent<Slider>();
        if (HealthBarUI == null)
        {
            Debug.LogError("HealthBarUI Slider not found. Ensure the hierarchy is correct.");
        }

        Pos = transform.Find("Pos");
        if (Pos == null)
        {
            Debug.LogError("Pos transform not found. Ensure the hierarchy is correct.");
        }

        ZombieNavMesh = GetComponent<NavMeshAgent>();
        if (ZombieNavMesh == null)
        {
            Debug.LogError("NavMeshAgent component not found.");
        }

        Anim = GetComponent<Animator>();
        if (Anim == null)
        {
            Debug.LogError("Animator component not found.");
        }

        Player = GameObject.Find(PlayerName)?.transform;
        if (Player == null)
        {
            Debug.LogError($"Player with name {PlayerName} not found.");
        }

        moneyManager = FindObjectOfType<MoneyManager>();
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager not found in the scene.");
        }

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
        if (isDead) // Skip update logic if the zombie is dead
            return;

        Anim.SetLayerWeight(Anim.GetLayerIndex("UpperBody"), 1);

        ReaytoFalse();
        HearingRange();

        if (DistanceToPlayer < 2 && ChackHit == true)
        {
            LookAtTarget();
        }

        if (HealthBarUI != null)
        {
            HealthBarUI.value = Health / 100;
        }

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
                ZombieNavMesh.speed = walkSpeed;
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
                ZombieNavMesh.speed = runSpeed;
                ZombieNavMesh.destination = Player.position;
                Anim.SetBool("Run", true);
            }
            if (IsMove == false)
            {
                Anim.SetBool("Run", false);
                ZombieNavMesh.speed = 0;
            }
        }

        if (Health <= 0.0f && !isDead)
        {
            Death();
        }

        if (NumberHealth == 0 && Health > 0.0f)
        {
            if (HealthBarUI != null)
            {
                HealthBarUI.gameObject.SetActive(true);
            }
        }

        if (NumberHealth == 1 && Health > 0.0f)
        {
            if (HealthBarUI != null)
            {
                HealthBarUI.gameObject.SetActive(false);
            }
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
        Debug.DrawRay(Pos.position, Pos.transform.TransformDirection(Vector3.forward) * attackRange, Color.green);

        if (Physics.Raycast(Pos.position, Pos.transform.TransformDirection(Vector3.forward), out hit, attackRange))
        {
            if (hit.transform.gameObject.name == PlayerName)
            {
                Anim.SetBool("Attack", true);
                Anim.Play(currentAttackAnim); // Play the chosen attack animation
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
        Debug.DrawRay(Pos.position, Pos.transform.TransformDirection(Vector3.forward) * attackRange, Color.red);

        if (Physics.Raycast(Pos.position, Pos.transform.TransformDirection(Vector3.forward), out hit, attackRange))
        {
            if (hit.transform.gameObject.name == PlayerName)
            {
                hit.transform.gameObject.SendMessage("ApplyDamagezombie", Damage); // Zorg ervoor dat de naam overeenkomt
            }
        }
    }

    void Death()
    {
        isDead = true; // Set the isDead flag to true
        if (HealthBarUI != null)
        {
            HealthBarUI.gameObject.SetActive(false);
        }
        Anim.enabled = false; // Disable the Animator
        ZombieNavMesh.enabled = false; // Disable the NavMeshAgent

        // Enable Ragdoll
        EnableRagdoll();

        // Add money on death
        if (moneyManager != null)
        {
            moneyManager.AddMoney(moneyOnDeath);
        }

        // Try to drop an item
        TryDropItem();

        if (NumberDestroy == 0)
        {
            StartCoroutine(TimeToDestroy());
        }
        else if (NumberDestroy == 1 && waveSystem != null) // Als de zombie niet wordt vernietigd, maar dood is, informeer het waveSystem direct
        {
            waveSystem.ZombieKilled(); // Informeert WaveSystem dat deze zombie is gedood
        }
    }

    IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(RandomDestroy);
        if (waveSystem != null)
        {
            waveSystem.ZombieKilled(); // Informeert WaveSystem dat deze zombie is gedood
        }
        Destroy(gameObject);
    }

    // Function to try dropping an item
    void TryDropItem()
    {
        if (dropItems.Count > 0)
        {
            float dropRoll = Random.Range(0f, 100f);
            if (dropRoll <= dropChance)
            {
                int randomIndex = Random.Range(0, dropItems.Count);
                Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity);
            }
        }
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

        // Add money on hit
        if (moneyManager != null && !isDead) // Ensure money is only added if the zombie is not dead
        {
            moneyManager.AddMoney(moneyPerHit);
        }
    }

    // Function to enable ragdoll
    void EnableRagdoll()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        Collider mainCollider = GetComponent<Collider>();
        if (mainCollider != null)
        {
            mainCollider.enabled = false;
        }

        Rigidbody mainRigidbody = GetComponent<Rigidbody>();
        if (mainRigidbody != null)
        {
            mainRigidbody.isKinematic = true;
        }
    }
}
