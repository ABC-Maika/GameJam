using UnityEngine;

public class cage : MonoBehaviour
{
    public int cageNumber;   // Set in Inspector
    private bool cageOpened;

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerStats stats = other.gameObject.GetComponent<PlayerMovement>().stats;

        if (stats != null)
        {
            // Safety check to prevent index errors
            if (cageNumber < stats.keys.Length && stats.keys[cageNumber] == true)
            {
                cageOpened = true;
                Debug.Log("Cage Opened!");

                // Optional: disable cage
                gameObject.SetActive(false);
            }
        }
    }
}