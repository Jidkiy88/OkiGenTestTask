using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class Task : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private string labelTextTemplate;

        public ItemType itemType;
        private int _itemsValue;
        private int _itemCount;
        private bool _isCompleted = false;

        public Action<Task> OnTaskComplete;

        public void SetTask(ItemType type, int count)
        {
            _itemsValue = 0;
            _isCompleted = false;
            itemType = type;
            _itemCount = count;
            UpdateLabel();
        }

        public bool IsCompleted()
        {
            return _isCompleted;
        }

        public void ProgressTask()
        {
            _itemsValue++;
            if (_itemsValue >= _itemCount)
            {
                _isCompleted = true;
                OnTaskComplete?.Invoke(this);
            }

            UpdateLabel();
        }

        private void UpdateLabel()
        {
            label.text = string.Format(labelTextTemplate, itemType.ToString(), _itemsValue.ToString(), _itemCount.ToString());
        }
    }
}