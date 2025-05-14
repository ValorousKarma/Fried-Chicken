using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance;
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Awake()
    {
        if (HealthBar.Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        slider.maxValue = Player.Instance.GetComponent<PlayerMovement>().maxHitpoint;
        slider.value = Player.Instance.GetComponent<PlayerMovement>().hitpoint;

        fill.color=gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
