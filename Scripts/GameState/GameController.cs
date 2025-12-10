using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    public static event Action OnPause;
    public static event Action OnResume;
    [HideInInspector] public bool gameActive;

    [Header("References")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject gameOverScreen;
    private InputActions inputActions;

    [Header("Sound Effects")]
    [Tooltip("Plays on player win")]
    [SerializeField] private SoundEffectSO partyPop;
    [Tooltip("Plays on player win")]
    [SerializeField] private SoundEffectSO partyHorn;
    [SerializeField] private float partyHornDelay;
    private AudioSource audioSource;

    [Header("Level Specific Values")]
    [Tooltip("This level ID keeps track of whether or not this level has been reached/beaten by the player")]
    [SerializeField] private int levelID;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        inputActions = new InputActions();

        if (victoryScreen == null)
            Debug.LogError("victoryScreen in GameController null. Please assign a reference.");

        if (gameOverScreen == null)
            Debug.LogError("gameOverScreen in GameController null. Please assign a reference.");

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gameActive = true;
    }

    private void OnEnable()
    {
        inputActions.Enable();

        PlayerLosesIfTouchesCollider.OnPlayerLose += PlayerLost;
        VictoryZone.OnPlayerWin += PlayerWon;
    }

    private void OnDisable()
    {
        inputActions.Disable();

        PlayerLosesIfTouchesCollider.OnPlayerLose -= PlayerLost;
        VictoryZone.OnPlayerWin -= PlayerWon;
    }

    private void Update()
    {
        MonitorPauseInput();
    }

    private void MonitorPauseInput()
    {
        if (gameActive && inputActions.Player.Escape.WasPerformedThisFrame())
            Pause();
    }

    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? Time.timeScale = 1 : Time.timeScale = 0;

        if (Time.timeScale == 0)
            OnPause?.Invoke();
        else
            OnResume?.Invoke();
    }

    private void PlayerLost()
    {
        gameActive = false;

        gameOverScreen.SetActive(true);
    }

    private void PlayerWon()
    {
        gameActive = false;

        victoryScreen.SetActive(true);

        StartCoroutine(PlayWinSFX());

        if (PlayerPrefs.GetInt("LevelReached") < levelID)
            PlayerPrefs.SetInt("LevelReached", levelID);
    }

    private IEnumerator PlayWinSFX()
    {
        if (audioSource != null)
        {
            if (partyPop != null)
                partyPop.Play(audioSource);
            
            yield return new WaitForSeconds(partyHornDelay);

            if (partyHorn != null)
                partyHorn.Play(audioSource);
        }
    }
}
