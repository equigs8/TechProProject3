using UnityEngine;

public class Traps : MonoBehaviour
{
    public bool ready = true;
    public float damage = 1f;
    public float triggerInterval = 2f;
    private float coolDown = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0f)
        {
            coolDown -= Time.deltaTime;
            if (coolDown <= 0f)
            {
                ready = true;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy") && ready)
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            ready = false;
            coolDown = triggerInterval;
        }
    }
}
