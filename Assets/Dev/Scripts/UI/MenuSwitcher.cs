using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dev.Scripts.UI
{
    public class MenuSwitcher : MonoBehaviour
    {
        private readonly Dictionary<Type, Menu> _spawnedPrefabs = new Dictionary<Type, Menu>();
        public static MenuSwitcher Instance { get; private set; }

        public void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Menu[] menus = GetComponentsInChildren<Menu>(true);

            Instance = this;
            
            foreach (Menu popUp in menus)
            {
                Type type = popUp.GetType();

                _spawnedPrefabs.Add(type, popUp);
            }
        }

        public bool TryGetMenu<TMenu>(out TMenu popUp) where TMenu : Menu
        {
            popUp = null;

            Type popUpType = typeof(TMenu);

            if (_spawnedPrefabs.ContainsKey(popUpType))
            {
                Menu spawnedPrefab = _spawnedPrefabs[popUpType];

                popUp = spawnedPrefab as TMenu;
                return popUp;
            }

            _spawnedPrefabs.Add(typeof(TMenu), popUp);

            return popUp;
        }
    }
}