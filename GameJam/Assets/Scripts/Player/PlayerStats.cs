using UnityEngine;

public class PlayerStats
{
    public float speed;
<<<<<<< Updated upstream
    public float power;
    public float maxHealth = 100;
    public float health = 100;
    public float stamina;
    public float sprint;
=======
    public float normalSpeed => speed;

	public float power;
    public float maxHealth;
    public float health;
    public float stamina;
    public float sprint;
    public float jumpSpeed;

    public PlayerStats()
    {
        speed = 5f;
        power = 10f;
        maxHealth = 100f;
        health = maxHealth;
        stamina = 100f;
        sprint = 7f;
        jumpSpeed = 8f;
	}
>>>>>>> Stashed changes

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Die();
        }
    }
}
