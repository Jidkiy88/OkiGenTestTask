using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TasksManager tasksManager;
        [SerializeField] private Health health;
        [SerializeField] private GameObject gameResultView;
        [SerializeField] private Transform taskHidePoint;
        [SerializeField] private TextMeshProUGUI resultLabel;
        [SerializeField] private Button restartButton;

        public Action<bool> OnGameOver;

        private void Awake()
        {
            tasksManager.OnTasksCompleted += WinGame;
            tasksManager.OnMistake += OnMistake;
            tasksManager.OnTaskComplete += TaskAnimation;
            health.OnLose += LoseGame;
            restartButton.onClick.AddListener(RestartScene);
        }

        private void Start()
        {
            gameResultView.SetActive(false);
        }

        private void OnMistake()
        {
            health.GetHit();
        }

        private void WinGame()
        {
            OnGameOver?.Invoke(true);
        }

        private void LoseGame()
        {
            OnGameOver?.Invoke(false);
        }

        private void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void EndLevel(bool state)
        {
            tasksManager.gameObject.SetActive(false);
            health.gameObject.SetActive(false);
            gameResultView.SetActive(true);
            string result = state ? "You win!" : "You lose!";
            resultLabel.text = result;
            ButtonAnimation();
        }

        private void ResultsAnimation()
        {

        }

        private void ButtonAnimation()
        {
            float dur = 1f;
            var seq = DOTween.Sequence();
            seq.Append(restartButton.transform.DOScale(1.1f, dur))
                .Append(restartButton.transform.DOScale(1f, dur))
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void TaskAnimation(Task task)
        {
            float dur = 1f;
            Transform tt = task.transform;
            tt.parent = transform;
            tt.DOMoveX(taskHidePoint.position.x, dur);
            var seq = DOTween.Sequence();
            seq.AppendInterval(dur / 2f)
                .Append(tt.DOMoveY(taskHidePoint.position.y, dur))
                .AppendCallback(() => 
                {
                    task.gameObject.SetActive(false);
                });
        }

        private void OnDestroy()
        {
            tasksManager.OnTasksCompleted -= WinGame;
            tasksManager.OnMistake -= OnMistake;
            health.OnLose -= LoseGame;
        }
    }
}