using UnityEngine;

public class PlayerStats
{
    public float speed;
    public float normalSpeed => speed;

	public float power;
    public float health;
    public float stamina;
    public float sprint;
    public float jumpSpeed;

    public PlayerStats()
    {
        speed = 5f;
        power = 10f;
        health = 100f;
        stamina = 100f;
        sprint = 7f;
        jumpSpeed = 8f;
	}

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Die();
        }
    }
}
