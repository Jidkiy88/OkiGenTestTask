using DG.Tweening;
using Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class Basket : MonoBehaviour
    {
        [SerializeField] private List<Item> items;
        [SerializeField] private Transform effectCanvas;
        [SerializeField] private TextMeshProUGUI effectLabel;
        [SerializeField] private Color fadeColor;

        public Action<ItemType> OnItemCollect;

        private Vector3 _defaultEffectPosition;
        private Color _defaultEffectColor;
        private Camera _camera;

        private void Start()
        {
            _defaultEffectPosition = effectCanvas.localPosition;
            _defaultEffectColor = effectLabel.color;
            _camera = Camera.main;
            effectCanvas.gameObject.SetActive(false);
        }

        public void CollectItem(Item item)
        {
            Item basketItem = items.Find(i => i.type == item.type);
            basketItem.gameObject.SetActive(true);
            OnItemCollect?.Invoke(item.type);
        }

        private void LateUpdate()
        {
            if (effectCanvas.gameObject.activeSelf)
            {
                effectLabel.transform.LookAt(_camera.transform.position);
            }
        }

        public void CollectEffect()
        {
            effectCanvas.gameObject.SetActive(true);
            float dur = 2f;
            var seq = DOTween.Sequence();
            seq.Append(effectCanvas.DOLocalMoveY(effectCanvas.position.y + 0.5f, dur))
                .Join(effectLabel.DOColor(fadeColor, dur))
                .AppendCallback(() => 
                {
                    effectCanvas.localPosition = _defaultEffectPosition;
                    effectLabel.color = _defaultEffectColor;
                    effectCanvas.gameObject.SetActive(false);
                });
        }
    }
}
