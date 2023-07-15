using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class ConveyorBehavior : MonoBehaviour
    {
        [SerializeField] private Transform spawnPosition;

        public float movingSpeed;

        private List<Item> _itemsOnConveyor = new List<Item>();
        private Coroutine _itemsMoverCoroutine;

        private void Start()
        {
            Initialize();   
        }

        private void Initialize()
        {
            if (_itemsMoverCoroutine != null)
            {
                EndMoveItems();
            }

            StartMoveItems();
        }

        private void StartMoveItems()
        {
            _itemsMoverCoroutine = StartCoroutine(ItemsMover());
        }

        public void EndMoveItems()
        {
            _itemsOnConveyor.ForEach(i => i.transform.parent = transform);
            StopCoroutine(_itemsMoverCoroutine);
        }

        public void AddItem(Item item)
        {
            item.gameObject.SetActive(true);
            _itemsOnConveyor.Add(item);
            item.ResetTransform();
            item.transform.position = spawnPosition.position;
        }

        public void RemoveItem(Item item)
        {
            item.gameObject.SetActive(false);
            _itemsOnConveyor.Remove(item);
        }

        private IEnumerator ItemsMover()
        {
            while (true)
            {
                foreach (Item item in _itemsOnConveyor)
                {
                    item.transform.position += -transform.right * (movingSpeed / 100f);
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
