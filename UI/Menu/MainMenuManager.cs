using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using static InputActions;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject initialText;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsSelection;
    [SerializeField] GameObject graphicSettings;
    [SerializeField] GameObject audioSettings;

    public MainMenuActions MainMenuActions { get; private set; }

    private InputAction pressAction;
    private InputAction anyKeyAction;
    private List<GameObject> currentMenuButtons = new List<GameObject>();
    private bool primeraPagina = false;

    private void Awake()
    {
        MainMenuActions = new InputActions().MainMenu;

        pressAction = MainMenuActions.Press;
        pressAction.performed += OnPressPerformed;

        anyKeyAction = MainMenuActions.AnyKey;
        anyKeyAction.performed += OnAnyKeyPress;
    }

    private void OnEnable()
    {
        pressAction.Enable();
        anyKeyAction.Enable();
    }

    private void OnDisable()
    {
        pressAction.Disable();
        anyKeyAction.Disable();
    }

    private void Start()
    {
        initialText.SetActive(true);
        mainMenu.SetActive(false);
        settingsSelection.SetActive(false);
        graphicSettings.SetActive(false);
        audioSettings.SetActive(false);
    }

    private void SetupMenuButtons(GameObject menu)
    {
        currentMenuButtons.Clear();

        foreach (Transform child in menu.transform)
        {
            GameObject button = child.gameObject;
            currentMenuButtons.Add(button);
        }

        if (currentMenuButtons.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(currentMenuButtons[0]);
            var animator = currentMenuButtons[0].GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("selected", true);
            }
        }
    }

    private void ExecuteButtonAction(int buttonIndex)
    {
        /*var animator = currentMenuButtons[buttonIndex].GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("pressed", true);
        }*/
        if (buttonIndex < 0 || buttonIndex >= currentMenuButtons.Count) return;

        if (mainMenu.activeSelf)
        {
            if (buttonIndex == 0)
            {
                SceneManager.LoadScene("Overworld");
            }
            else if (buttonIndex == 1)
            {
                OpenSettingsSelectionMenu();
            }
            else if (buttonIndex == 2)
            {
                Application.Quit();
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
        }
        else if (settingsSelection.activeSelf)
        {
            if (buttonIndex == 0)
            {
                OpenAudioSettingsMenu();
            }
            else if (buttonIndex == 1)
            {
                OpenGraphicSettingsMenu();
            }
            else if (buttonIndex == 2)
            {
                OpenMainMenu();
            }
        }
        else if (audioSettings.activeSelf)
        {
            if (buttonIndex == 0)
            {
                Debug.Log("NO HAGO NADA");
            }
            else if (buttonIndex == 1)
            {
                Debug.Log("NO HAGO NADA");
            }
            else if (buttonIndex == 2)
            {
                OpenSettingsSelectionMenu();
            }
        }
        else if (graphicSettings.activeSelf)
        {
            if (buttonIndex == 0)
            {
                Debug.Log("NO HAGO NADA");
            }
            else if (buttonIndex == 1)
            {
                Debug.Log("NO HAGO NADA");
            }
            else if (buttonIndex == 2)
            {
                OpenSettingsSelectionMenu();
            }
        }
    }

    private void OnAnyKeyPress(InputAction.CallbackContext context)
    {
        if(!primeraPagina)
        {
            initialText.SetActive(false);
            OpenMainMenu();
            primeraPagina = true;

            anyKeyAction.Disable();
        }
    }

    private void OnPressPerformed(InputAction.CallbackContext context)
    {
        if (primeraPagina)
        {
            int selectedButtonIndex = currentMenuButtons.IndexOf(EventSystem.current.currentSelectedGameObject);
            if(!(context.action.activeControl.device is Mouse))
            {
                ExecuteButtonAction(selectedButtonIndex);
            }
            else
            {
                MenuButton buttonScript = currentMenuButtons[selectedButtonIndex].GetComponent<MenuButton>();

                if(buttonScript.mouseInside)
                {
                    ExecuteButtonAction(selectedButtonIndex);
                }
            }
        }
    }

    public void OpenMenu(MenuScreenType menuScreenType)
    {
        initialText.SetActive(false);
        mainMenu.SetActive(false);
        settingsSelection.SetActive(false);
        graphicSettings.SetActive(false);
        audioSettings.SetActive(false);

        currentMenuButtons.Clear();

        GameObject activeMenu = null;

        switch (menuScreenType)
        {
            case MenuScreenType.InitialText:
                activeMenu = initialText;
                break;
            case MenuScreenType.MainMenu:
                activeMenu = mainMenu;
                break;
            case MenuScreenType.SettingsSelection:
                activeMenu = settingsSelection;
                break;
            case MenuScreenType.GraphicSettings:
                activeMenu = graphicSettings;
                break;
            case MenuScreenType.AudioSettings:
                activeMenu = audioSettings;
                break;
        }

        if (activeMenu != null)
        {
            activeMenu.SetActive(true);

            foreach (Transform child in activeMenu.transform)
            {
                currentMenuButtons.Add(child.gameObject);
            }
        }
    }
    
    public void OpenInitialTextMenu()
    {
        OpenMenu(MenuScreenType.InitialText);
        SetupMenuButtons(initialText);
    }

    public void OpenMainMenu()
    {
        OpenMenu(MenuScreenType.MainMenu);
        SetupMenuButtons(mainMenu);
    }

    public void OpenSettingsSelectionMenu()
    {
        OpenMenu(MenuScreenType.SettingsSelection);
        SetupMenuButtons(settingsSelection);
    }

    public void OpenGraphicSettingsMenu()
    {
        OpenMenu(MenuScreenType.GraphicSettings);
        SetupMenuButtons(graphicSettings);
    }

    public void OpenAudioSettingsMenu()
    {
        OpenMenu(MenuScreenType.AudioSettings);
        SetupMenuButtons(audioSettings);
    }
}
