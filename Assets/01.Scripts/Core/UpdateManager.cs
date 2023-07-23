using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomUpdateManager
{
    public static class UpdateManager
    {
        static UpdateManager()
        {
            var obj = new GameObject();
            _instance = obj.AddComponent<UpdateManagerInnerMonoBehvaiour>();
        }

        class UpdateManagerInnerMonoBehvaiour : MonoBehaviour
        {
            private void Update()
            {
                foreach(IUpdatable mono in _updateHashs)
                {
                    mono.CustomUpdate();
                }
            }
        }

        private static UpdateManagerInnerMonoBehvaiour _instance;
        private static HashSet<IUpdatable> _updateHashs = new HashSet<IUpdatable>();
        
        public static void Add(IUpdatable mono)
        {
            _updateHashs.Add(mono);
        }

        public static void Remove(IUpdatable mono)
        {
            _updateHashs.Remove(mono);
        }

    }
}
