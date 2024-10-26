using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DeathScreen : MonoBehaviour
{
    public float timeToAllowRespawn = 0.5f;
    [SerializeField, HideInInspector] Button button;
    private bool canSpawn = false;

    public void Awake()
    {
        gameObject.SetActive(false);
        GameManager.Instance.uiManager.DeathScreen = this;
    }


    public void OnPlayerDeath()
    {
        gameObject.SetActive(true);
        GameManager.Instance.uiManager.hudPanel.gameObject.SetActive(false);

        StartCoroutine(DelayBeforeAllowingRespawn());
    }

    public void Update()
    {
        if (!canSpawn) return;

        bool gamepadButtonPressed = false;
        if (Gamepad.current != null)
        {
            gamepadButtonPressed =
                Gamepad.current.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic);
        }
        
        if (gamepadButtonPressed || Input.anyKey)
        {
            RespawnPlayer();
        }
    }

    IEnumerator DelayBeforeAllowingRespawn()
    {
        yield return new WaitForSecondsRealtime(timeToAllowRespawn);
        canSpawn = true;
    }

    public void OnValidate()
    {
        button = GetComponent<Button>();
    }

    public void RespawnPlayer()
    {
        gameObject.SetActive(false);
        GameManager.Instance.uiManager.hudPanel.gameObject.SetActive(true);
        
        GameManager.Instance.spawnController.RespawnPlayer();
    }
}