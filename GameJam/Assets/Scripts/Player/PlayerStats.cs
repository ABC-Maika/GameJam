using System;
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
    public int keysAmount;
    public bool[] keys;

    public PlayerStats()
    {
        speed = 7f;
        power = 10f;
        health = 100f;
        stamina = 100f;
        sprint = 10f;
        jumpSpeed = 8f;
        keysAmount = 5;
        keys = new bool[keysAmount];
        for(int i = 0; i < keysAmount; i++)
        {
            keys[i] = false;
        }
        keys[1] = true;
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
