using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CustomUpdateManager
{
    public interface IUpdatable
    {
        public abstract void CustomUpdate();
        public abstract void Add(IUpdatable updatable);
        public abstract void Remove(IUpdatable updatable);
    }

}

