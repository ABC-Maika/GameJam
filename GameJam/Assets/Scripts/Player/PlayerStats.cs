using UnityEngine;

public class PlayerStats
{
    public float speed;
    public float power;
    public float maxHealth = 100;
    public float health = 100;
    public float stamina;
    public float sprint;

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Die();
        }
    }
}
