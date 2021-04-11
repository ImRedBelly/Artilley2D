using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public UnityEvent healing;
    public Text healthText;
    float health = 3;
    bool isRise = true;

    private void Start()
    {
        instance = this;
        healthText.text = ": " + health;
    }

    public void ApplyDamage()
    {
        health--;
        healthText.text = ": " + health;
        FindObjectOfType<UIController>().DoMove();

        if (health <= 0)
        {
            if (!isRise)
            {
                Destroy(PlayerCreator.instance.gameObject);
                SceneManager.LoadScene(0);
            }
            healing.Invoke();
            isRise = false;
            return;
        }

    }

    public void Healing(float pointHealth)
    {
        health += pointHealth;
        isRise = false;
        healthText.text = ": " + health;
    }
}
