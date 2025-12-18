using SkyBeneathDemo.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SkyBeneathDemo
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] PlayerController m_playerController;
        [SerializeField] InputManager m_playerInput;
        [SerializeField] GameObject m_startGameUI;
        [SerializeField] Button m_startGameButton;
        [SerializeField] TMP_Text m_startTitleTxt;
        [SerializeField] GameObject m_restartGameUI;
        [SerializeField] Button m_restartButton;
        [SerializeField] TMP_Text m_restartTitleTxt;
        [SerializeField] TMP_Text m_collectedCountTxt;
        [SerializeField] TMP_Text m_timerTxt;
        [SerializeField] int m_timeToCollectInMinutes = 2;
        [SerializeField] int m_numOfCollectables;

        private int m_numOfCubesCollected = 0;

        private void OnEnable()
        {
            m_playerController.onFallenIntoVoid += OnPlayerFallenIntoVoid;
            m_playerController.onCubeCollected += OnCubeCollected;
        }

        private void Start()
        {
            m_playerInput.enabled = false;
            m_numOfCollectables = GameObject.FindGameObjectsWithTag("Collectable").Length;
            SetCollectedTxt();
            m_startTitleTxt.text = $"Collect {m_numOfCollectables} Cubes in {m_timeToCollectInMinutes} minutes time";
            m_restartGameUI.SetActive(false);
            m_startGameUI.SetActive(true);
            m_startGameButton.onClick.AddListener(() =>
            {
                m_startGameUI.SetActive(false);
                m_playerInput.enabled = true;
                StartTimer();
            });
            m_restartButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        }

        private void OnDisable()
        {
            m_playerController.onFallenIntoVoid -= OnPlayerFallenIntoVoid;
            m_playerController.onCubeCollected -= OnCubeCollected;
        }

        private async void StartTimer()
        {
            float tiemInSeconds = m_timeToCollectInMinutes * 60f;
            SetTimeTxt(tiemInSeconds);
            while (tiemInSeconds > 0)
            {
                await Awaitable.WaitForSecondsAsync(1f);
                tiemInSeconds--;
                SetTimeTxt(tiemInSeconds);
            }
            OnTimeOut();
        }

        private void SetTimeTxt(float timeInSeconds)
        {
            float minutes = Mathf.FloorToInt(timeInSeconds / 60);
            float seconds = Mathf.FloorToInt(timeInSeconds % 60);

            // Update the text. "{0:00}" ensures leading zeros (e.g., 05:09)
            m_timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private async void OnPlayerFallenIntoVoid()
        {
            m_restartTitleTxt.text = "Game Over";
            m_playerController.enabled = false;
            await Awaitable.WaitForSecondsAsync(1f);
            m_playerController.gameObject.SetActive(false);
            m_restartGameUI.SetActive(true);
        }

        private void OnCubeCollected()
        {
            m_numOfCubesCollected++;
            Debug.Log("Cube Collected");
            SetCollectedTxt();
            if (m_numOfCubesCollected == m_numOfCollectables)
            {
                OnGameWin();
            }
        }

        private void OnGameWin()
        {
            m_playerInput.enabled = false;
            m_restartTitleTxt.text = "Game Won";
            m_restartGameUI.SetActive(true);
        }

        private void OnTimeOut()
        {
            m_playerInput.enabled = false;
            m_restartTitleTxt.text = "Times Up!";
            m_restartGameUI.SetActive(true);
        }

        private void SetCollectedTxt()
        {
            m_collectedCountTxt.text = $"Collected: {m_numOfCubesCollected}/{m_numOfCollectables}";
        }
    }
}
