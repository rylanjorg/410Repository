using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private Image _healthbarBackgroundSprite;
    [SerializeField] private float _reduceSpeed = 2;
    private float _target = 1;
    private Camera _mainCamera;

    /*public int maxHealth = 100;
    public GameObject healthBarUI;
    public Slider healthSlider;
    public Text healthText;

    private int currentHealth;*/

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
        _healthbarSprite.fillAmount = Mathf.Lerp(_healthbarSprite.fillAmount, _target, Time.deltaTime * _reduceSpeed);
    }


    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        //_healthbarSprite.fillAmount = currentHealth / maxHealth;
        _target = currentHealth / maxHealth;
    }
  
    /*public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = (float)currentHealth / maxHealth;
        healthText.text = currentHealth.ToString();

        if (!healthBarUI.activeSelf)
        {
            healthBarUI.SetActive(true);
        }

        if (currentHealth <= 0)
        {
            // Enemy is dead, disable health bar
            DisableHealthBar();
        }
    }*/

    public void EnableHealthBar()
    {
        _healthbarBackgroundSprite.gameObject.SetActive(true);
    }

    public void DisableHealthBar()
    {
        _healthbarBackgroundSprite.gameObject.SetActive(false);
    }
}
