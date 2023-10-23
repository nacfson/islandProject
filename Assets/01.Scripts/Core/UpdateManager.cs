using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomUpdateManager
{
    public class UpdateManager : Singleton<UpdateManager>
    {
        private static HashSet<IUpdatable> _updateHashes = new HashSet<IUpdatable>();
        private bool _isInited;
        public override void Init(GameManager root)
        {
            base.Init(root);
            _isInited = true;
        }
        public void CustomUpdate()
        {
            foreach(IUpdatable mono in _updateHashes)
            {
                mono.CustomUpdate();
            }
        }
        public void Add(IUpdatable mono)
        {
            _updateHashes.Add(mono);
        }

        public void Remove(IUpdatable mono)
        {
            _updateHashes.Remove(mono);
        }
    }
}
