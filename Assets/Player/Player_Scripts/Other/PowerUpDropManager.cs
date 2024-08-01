using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDropManager : MonoBehaviour
{
    public void DropRandomPowerUp(Vector3 position)
    {
        // Get a random power up from the list
        /*PowerUpData powerUpData = PowerUpManager.Instance.GetRandomPowerUp();

        // Create a new power up object
        GameObject powerUpObject = new GameObject(powerUpData.name);
        powerUpObject.transform.position = position;

        // Add a sprite renderer component
        SpriteRenderer spriteRenderer = powerUpObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = powerUpData.powerUpSprite;

        // Add a power up component
        PowerUp powerUp = powerUpObject.AddComponent<PowerUp>();
        powerUp.powerUpData = powerUpData;

        // Add a rigidbody component
        Rigidbody2D rigidbody = powerUpObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0.0f;

        // Add a collider component
        CircleCollider2D collider = powerUpObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;

        // Add a power up drop component
        PowerUpDrop powerUpDrop = powerUpObject.AddComponent<PowerUpDrop>();
        powerUpDrop.speed = Random.Range(1.0f, 3.0f);
        powerUpDrop.direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));*/
    }
}
