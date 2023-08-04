using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Tree : MonoBehaviour, IInteractable, IActionable, IGrowable
{
    #region Shake 관련 변수들
    private int _shakeCount = 3;
    [SerializeField] private float _shakeTimer = 3f;
    [SerializeField] private ItemObjListData _itemList;
    private float _dropRadius = 1.5f;

    private bool _canDrop = true;
    private bool _canShake = true;

    private MeshRenderer[] _meshRenderers;
    #endregion

    #region 과일 관련 변수들
    private int _maxLevel = 3;
    private int _curLevel = 0;

    private float _curTime = 0f;
    [SerializeField] private float _targetTime = 60f;
    public event Action OnFruitGrew;
    private Transform _fruitPos;

    private Transform[] _fruitPoses;

    #endregion
    public void SetUp()
    {

        _fruitPos = transform.Find("FruitPos");
        _fruitPoses = new Transform[_fruitPos.childCount];
        for(int i = 0; i < _fruitPoses.Length; i++)
        {
            _fruitPoses[i] = _fruitPos.GetChild(i);
        }

    }

    public void Interact(AgentBrain<ActionData> brain)
    {
        Debug.Log("Interact");
        brain.ChangeState(StateType.Push);
        PlayerBrain pb = brain as PlayerBrain;
        pb.AgentAnimator.OnPushAnimationEndTrigger += ChangeShakeTrue;
    }

    public void UnInteract(AgentBrain<ActionData> brain) => _shakeCount = 3; 

    private void DropItem()
    {
        Init();
        ItemObject item = _itemList.GetRandItemObj();
        for (int i = 0; i < 3; i++)
        {
            ItemObject obj = PoolManager.Instance.Pop(item.name) as ItemObject;
            obj.transform.position = transform.position;

            Vector3 offset = transform.position + new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 3), 0, Mathf.Sin(Mathf.PI * 2 * i / 3)) * _dropRadius;

            bool result = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f, 1 << LayerMask.NameToLayer("GROUND"));
            if (result)
            {
                offset.y = hit.point.y + 0.2f;
            }
            else
            {
                Debug.LogError("Can't Find the Ground");
                return;
            }
            obj.transform.DOJump(offset, 2f, 1, 1f).SetEase(Ease.InCirc);
        }
        _canDrop = false;
    }

    public void Shake(AgentBrain<ActionData> brain)
    {
        if (!_canDrop || !_canShake) return;
        Debug.Log("Shake");
        _shakeCount--;
        _shakeCount = Mathf.Clamp(_shakeCount, 0, 3);
        PlayerBrain pb = brain as PlayerBrain;
        pb.AgentAnimator.SetTriggerPush(true);
        _canShake = false;

        if (_shakeCount <= 0)
        {
            DropItem();
            pb.AgentAnimator.OnPushAnimationEndTrigger += ChangeToIdle;
            StopAllCoroutines();
            return;
        }
        StopAllCoroutines();
        StartCoroutine(ShakeCor(brain));
    }
    #region coroutines

    private IEnumerator ShakeTreeCor()
    {
        float timer = 0f;
        float target = 0.7f;
        foreach(MeshRenderer _meshRenderer in  _meshRenderers)
        {
            float originStr = _meshRenderer.material.GetFloat("_WindStrength");
            float originSpeed = _meshRenderer.material.GetFloat("_WindSpeed");
            float originScale = _meshRenderer.material.GetFloat("_WindScale");
            while (timer < target)
            {
                float value = timer / target;
                timer += Time.deltaTime;
                _meshRenderer.material.SetFloat("_WindStrength", value * 0.1f);
                _meshRenderer.material.SetFloat("_WindSpeed", value * 0.1f);
                _meshRenderer.material.SetFloat("_WindScale", 10f);

                yield return null;
            }
            _meshRenderer.material.SetFloat("_WindStrength", originStr);
            _meshRenderer.material.SetFloat("_WindSpeed", originSpeed);
            _meshRenderer.material.SetFloat("_WindSpeed", originScale);
        }
    }

    private IEnumerator ShakeCor(AgentBrain<ActionData> brain)
    {
        float timer = 0f;
        while (timer < _shakeTimer)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        brain.ChangeState(StateType.Idle);
    }
    #endregion
    public void DoAction(AgentBrain<ActionData> brain) => Shake(brain);
    public void UnAction(AgentBrain<ActionData> brain)
    {
        _shakeCount = 3;
        PlayerBrain pb = brain as PlayerBrain;
        pb.AgentAnimator.OnPushAnimationEndTrigger -= ChangeShakeTrue;
        pb.AgentAnimator.OnPushAnimationEndTrigger -= ChangeToIdle;
        ChangeShakeTrue(brain);
    }

    public void ChangeShakeTrue(AgentBrain<ActionData> brain) => _canShake = true;
    private void ChangeToIdle(AgentBrain<ActionData> brain) => brain.ChangeState(StateType.Idle);

    public void UpgradeLevel(int plus)
    {
        if (_curLevel >= _maxLevel) return;

        _curTime += Time.deltaTime;
        if (_curTime >= _targetTime)
        {
            _curLevel += plus;
            if (_curLevel >= _maxLevel)
            {
                OnFruitGrew?.Invoke();
                ItemObject item = _itemList.GetRandItemObj();
                for (int i = 0; i < _fruitPoses.Length; i++)
                {
                    ItemObject itemObj = PoolManager.Instance.Pop(item.name) as ItemObject;
                    itemObj.transform.SetParent(_fruitPoses[i]);
                    itemObj.GetComponent<ItemObject>().enabled = false;
                    itemObj.gameObject.layer = LayerMask.NameToLayer("Default");
                    itemObj.transform.localPosition = Vector3.zero;
                }
                _curLevel = 0;
            }
            _curTime = 0f;
        }
    }
    private void Init()
    {
        for(int i = 0; i < _fruitPoses.Length; i++)
        {
            ItemObject obj = _fruitPos.GetChild(i).GetComponentInChildren<ItemObject>();
            PoolManager.Instance.Push(obj);
        }
    }
}