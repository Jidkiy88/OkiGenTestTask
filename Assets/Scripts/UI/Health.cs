using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private List<GameObject> heartIcons;

        private int _currentLivesCount = 3;

        public Action OnLose;

        public void GetHit()
        {
            _currentLivesCount--;
            UpdateIcons();
            if (_currentLivesCount <= 0)
            {
                _currentLivesCount = 0;
                OnLose?.Invoke();
            }
        }

        private void UpdateIcons()
        {
            heartIcons.ForEach(i => i.gameObject.SetActive(false));
            for (int i = 0; i < _currentLivesCount; i++)
            {
                heartIcons[i].gameObject.SetActive(true);
            }
        }
    }
}