using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAgent : Agent<ActionData>
{
    public PlayerBrain PlayerBrain
    {
        get
        {
            if (_playerBrain == null)
            {
                _playerBrain = _brain as PlayerBrain;
                if (_playerBrain == null)
                {
                    Debug.LogError("Can't casting correct brain");
                }
            }

            return _playerBrain;
        }
    }
    protected PlayerBrain _playerBrain;
    protected Input_PlayerInput _newInput;
    
    public virtual void SetUp(Transform agent)
    {
        base.SetUp(agent);
        _newInput = PlayerBrain.PlayerInput;
    }
}
