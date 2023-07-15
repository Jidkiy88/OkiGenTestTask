using DG.Tweening;
using Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TasksManager tasksManager;
        [SerializeField] private CharacterBehavior character;
        [SerializeField] private ConveyorBehavior conveyor;
        [SerializeField] private UIController uiController;
        [SerializeField] private ItemsPool pool;
        [SerializeField] private Health health;
        [SerializeField] private int tasksCount;
        [SerializeField] private Transform closePosition;

        private Camera _camera;

        private void Awake()
        {
            uiController.OnGameOver += CompleteLevel;
        }

        private void Start()
        {
            _camera = Camera.main;
            GenerateTasks();
        }

        private void GenerateTasks()
        {
            for (int i = 0; i < tasksCount; i++)
            {
                tasksManager.GenerateTask();
            }
        }

        private void CompleteLevel(bool state)
        {
            pool.EndSpawn();
            conveyor.EndMoveItems();
            FinalConveyorAnimation();
            FinalCameraAnimation(state);
        }

        private void FinalConveyorAnimation()
        {
            float dur = 1f;
            conveyor.transform.DOMoveY(conveyor.transform.position.y - 3f, dur);
        }

        private void FinalCameraAnimation(bool state)
        {
            float dur = 1f;
            var seq = DOTween.Sequence();
            seq.Append(_camera.transform.DOMove(closePosition.position, dur))
                .Join(_camera.transform.DORotate(closePosition.rotation.eulerAngles, dur))
                .AppendCallback(() => 
            {
                if (state)
                {
                    character.Victory();
                }
                else
                {
                    character.Defeat();
                }

                uiController.EndLevel(state);
            });
        }

        private void OnDestroy()
        {
            uiController.OnGameOver -= CompleteLevel;
        }
    }
}
