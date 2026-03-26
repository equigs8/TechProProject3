using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Health Settings")]
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Target HP: " + health);

        if (health <= 0)
        {
            // Handle Game Over logic here
            Debug.Log("Game Over!");
        }
    }
}