using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.UI
{
    public class TasksManager : MonoBehaviour
    {
        [SerializeField] private List<Task> tasks;
        [SerializeField] private Basket basket;
        [SerializeField] private int maxItemsCount;

        private List<Task> _activeTasks = new List<Task>();

        public Action<Task> OnTaskComplete;
        public Action OnTasksCompleted;
        public Action OnMistake;

        private void Awake()
        {
            basket.OnItemCollect += OnItemCollect;
        }

        private void OnItemCollect(ItemType type)
        {
            Task task = _activeTasks.Find(t => t.itemType == type && !t.IsCompleted());

            if (task == null)
            {
                OnMistake?.Invoke();
                return;
            }

            basket.CollectEffect();
            task.ProgressTask();
        }

        public void GenerateTask()
        {
            Task task = tasks.Find(t => !t.gameObject.activeSelf);

            if (task == null)
            {
                return;
            }

            int count = Random.Range(0, maxItemsCount) + 1;
            task.gameObject.SetActive(true);
            task.SetTask(GetNewType(), count);
            _activeTasks.Add(task);
            task.OnTaskComplete += CompleteTask;
        }

        private ItemType GetNewType()
        {
            List<ItemType> allTypes = new List<ItemType>();
            for (int i = 0; i < Enum.GetNames(typeof(ItemType)).Length; i++)
            {
                ItemType type = (ItemType) i;
                if (!_activeTasks.Contains(_activeTasks.Find(t => t.itemType == type)))
                {
                    allTypes.Add(type);
                }
            }

            ItemType newType = allTypes[Random.Range(0, allTypes.Count)];
            return newType;
        }

        private void CompleteTask(Task task)
        {
            OnTaskComplete?.Invoke(task);
            task.OnTaskComplete -= CompleteTask;
            _activeTasks.Remove(task);
            if (!_activeTasks.Any())
            {
                OnTasksCompleted?.Invoke();
            }
        }

        private void OnDestroy()
        {
            basket.OnItemCollect -= OnItemCollect;
        }
    }
}
