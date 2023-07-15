using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private List<Item> items;

        private Item _currentItem;

        public void ShowItem(ItemType type)
        {
            _currentItem = items.Find(i => i.type == type);
            _currentItem.gameObject.SetActive(true);
        }

        public void HideItem()
        {
            _currentItem.gameObject.SetActive(false);
            _currentItem = null;
        }
    }
}
