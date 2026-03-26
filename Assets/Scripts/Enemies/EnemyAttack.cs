using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Target Reference")]
    public Target targetScript; // Drag the target object here or find via code

    [Header("Attack Parameters")]
    public float attackStrength = 10f;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;
    private EnemyMovement movement;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        
        // Automatically find the target script if not assigned
        if (targetScript == null)
        {
            targetScript = FindFirstObjectByType<Target>();
        }
    }

    void Update()
    {
        // Only attack if the movement script says we've reached the end
        if (movement != null && movement.ReachedTarget())
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        if (targetScript != null)
        {
            targetScript.TakeDamage(attackStrength);
            lastAttackTime = Time.time;
            
            // Optional: Play an animation or sound here
            Debug.Log(gameObject.name + " attacked the target!");
        }
    }
}