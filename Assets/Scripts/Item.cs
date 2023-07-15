using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class Item : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private CharacterBehavior character;
        [SerializeField] private Transform grabPosition;

        private List<Transform> _clampedIKPoints = new List<Transform>();

        public ItemType type;

        public Action<Item> OnItemPick;

        private bool _isPicked = false;
        private Vector3 _defaultPosition;
        private Vector3 _defaultRotation;

        private void Awake()
        {
            _defaultPosition = transform.localPosition;
            _defaultRotation = transform.localRotation.eulerAngles;
        }

        public void AddIKPoint(Transform point)
        {
            if (!_clampedIKPoints.Contains(point))
            {
                _clampedIKPoints.Add(point);
            }
        }

        public void RemoveIKPoint(Transform point)
        {
            if (_clampedIKPoints.Contains(point))
            {
                _clampedIKPoints.Remove(point);
            }
        }

        private void Update()
        {
            if (_clampedIKPoints.Any())
            {
                _clampedIKPoints.ForEach(p => p.position = grabPosition.position);
            }
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ResetTransform()
        {
            _isPicked = false;
            transform.localPosition = _defaultPosition;
            transform.localRotation = Quaternion.Euler(_defaultRotation);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isPicked || character == null || character.isBusy)
            {
                return;
            }

            _isPicked = true;
            OnItemPick?.Invoke(this);
        }
    }

    public enum ItemType
    {
        Apple,
        Avocado,
        Banana,
        Bread,
        Can,
        Carrot,
        Lemon,
        Menu,
        Milk,
        Salad,
        Soap,
        Soil,
        Tomato,
        Water
    }
}
