using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class ItemsPool : MonoBehaviour
    {
        [SerializeField] private List<Item> items;
        [SerializeField] private ConveyorBehavior conveyor;
        [SerializeField] private CharacterBehavior character;
        [SerializeField] private float spawnDelay;

        private Coroutine _itemsSpawnerCoroutine;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            StartSpawn();
        }

        public void StartSpawn()
        {
            if (_itemsSpawnerCoroutine != null)
            {
                EndSpawn();
            }

            _itemsSpawnerCoroutine = StartCoroutine(ItemsSpawner());
        }

        public void EndSpawn()
        {
            StopCoroutine(_itemsSpawnerCoroutine);
        }

        private IEnumerator ItemsSpawner()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnDelay);
                var hidenItems = items.Where(g => !g.gameObject.activeSelf).ToList();
                var item = hidenItems[Random.Range(0, hidenItems.Count)];
                if (item != null)
                {
                    item.transform.parent = transform;
                    conveyor.AddItem(item);
                    item.OnItemPick += OnItemPick;
                }
            }
        }

        private void OnItemPick(Item item)
        {
            character.PickItem(item);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                var item = other.gameObject.GetComponent<Item>();
                item.OnItemPick -= OnItemPick;
                item.gameObject.SetActive(false);
                conveyor.RemoveItem(item);
            }
        }
    }
}
