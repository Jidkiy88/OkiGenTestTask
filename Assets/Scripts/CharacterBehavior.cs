using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Animations.Rigging;

namespace Scripts
{
    public class CharacterBehavior : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Basket basket;
        [SerializeField] private Hand hand;
        [SerializeField] private Rig handRig;
        [SerializeField] private Transform handIKPoint;
        [SerializeField] private ConveyorBehavior conveyor;

        public bool isBusy;

        private Item _currentItem;

        public void PickItem(Item item)
        {
            if (isBusy)
            {
                return;
            }

            isBusy = true;
            _currentItem = item;
            transform.DOLookAt(new Vector3(_currentItem.transform.position.x, transform.position.y, _currentItem.transform.position.z), 0.3f);
            SetHandIKPoint();
            animator.speed = conveyor.movingSpeed * 1.5f;
            animator.SetTrigger("Pick");
        }

        public void Victory()
        {
            isBusy = true;
            animator.SetTrigger("Dance");
        }

        public void Defeat()
        {
            isBusy = true;
            animator.SetTrigger("Defeat");
        }

        private void SetHandIKPoint()
        {
            _currentItem.AddIKPoint(handIKPoint);
            StartCoroutine(ChangeRigWeight(handRig));
        }

        private void RemoveHandIKPoint()
        {
            _currentItem.RemoveIKPoint(handIKPoint);
            StartCoroutine(ChangeRigWeight(handRig));
        }

        public void OnGrab()
        {
            RemoveHandIKPoint();
            conveyor.RemoveItem(_currentItem);
            hand.ShowItem(_currentItem.type);
        }

        private IEnumerator ChangeRigWeight(Rig rig)
        {
            var speed = 3f;
            var value = rig.weight == 0f ? 1f : 0f;
            var delta = value == 1f ? speed : -speed;

            while (rig.weight != value)
            {
                yield return new WaitForEndOfFrame();
                rig.weight += Time.deltaTime * delta;
            }
        }

        public void OnCollect()
        {
            basket.CollectItem(_currentItem);
            hand.HideItem();
            _currentItem = null;
        }

        public void OnPickingEnd()
        {
            isBusy = false;
            animator.speed = 1f;
            transform.DORotate(Vector3.zero, 0.3f);
        }
    }
}
