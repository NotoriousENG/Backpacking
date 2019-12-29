using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Text healthText;
    public Image healthRedMask;
    public Image healthRedImage;
    public int maxHealth;
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        healthText.text = health + " HP";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool hitBy(int damage)
    {
        bool willDie = false;
        if (damage >= health)
        {
            willDie = true;
        }
        StartCoroutine(decrementHealthBy(damage));
        return willDie;
    }
    IEnumerator decrementHealthBy(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            Debug.Log("Health: " + health);
            if (i == damage - 1)
            {
            }
            if (health > 0)
            {
                health -= 1;
                healthText.text = health + " HP";
                float healthFraction = (float)health / maxHealth;
                healthRedMask.rectTransform.localPosition = new Vector3(60, 45 - 40 * (1 - healthFraction), 0);
                healthRedImage.rectTransform.localPosition = new Vector3(0, 80 * (1 - healthFraction), 0);
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                yield break;
            }
        }
    }
}
