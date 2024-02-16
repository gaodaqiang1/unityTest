using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum Direction
    {
        Up, Right, Left
    }

    private Rigidbody2D _rb;

    private Animator _anim;

    private SpriteRenderer sr;
    private PlayerInput playerInput;
    private BoxCollider2D coll;

    [Header("得分")]
    public int stepPoint;
    private int pointResult;
    [Header("跳跃")]
    public float JumpDistance;

    private float _moveDistance;
    private Vector2 _destination;
    private Vector2 _touchPosition;
    private Direction dir;
    private bool _buttonHold;

    private bool _isJump, _canJump, _isDead;
    // public TerrainManager terrainMnaManager;


    private Collider2D[] _result = new Collider2D[2];

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_isDead)
        {
            DisableInput();
            return;
        }
        if (_canJump)
        {
            TriggerJump();
        }

    }

    private void FixedUpdate()
    {
        if (_isJump)
            _rb.position = Vector2.Lerp(transform.position, _destination, 0.134f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Water") && !_isJump)
        {
            _result = Physics2D.OverlapCircleAll(transform.position, 0.1f);
            bool inWater = true;
            foreach (var hit in _result)
            {
                if (hit.CompareTag("Wood"))
                {
                    //TODO:����ľ���ƶ�
                    transform.parent = hit.transform;
                    inWater = false;
                }

            }

            if (inWater && !_isJump)
            {
                Debug.Log("In Water GAME OVER");
                _isDead = true;
            }
        }


        if (other.CompareTag("Border") || other.CompareTag("Car"))

        {
            Debug.Log("Game Over!");
            _isDead = true;
        }

        if (!_isJump && other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            _isDead = true;
        }

        if (_isDead)
        {
            EventHandler.CallGameOverEvent();
            coll.enabled = false;
        }
    }


    #region input ����ص�����
    public void Jump(InputAction.CallbackContext context)
    {
        //TODO:ִ执行跳跃，跳跃的距离，记录分数，播放跳跃音乐
        if (context.performed && _isJump == false)
        {
            _moveDistance = JumpDistance;

            //Debug.Log("jump!" +" "+ _moveDistance);
            _canJump = true;
            AudioManager.instance.SetJumpClip(0);
        }

        if (dir == Direction.Up && context.performed && _isJump == false)
        {
            pointResult += stepPoint;
        }
 





    }

    public void LongJump(InputAction.CallbackContext context)
    {
        if (context.performed && !_isJump)
        {
            _moveDistance = JumpDistance * 2;
            _buttonHold = true;
            AudioManager.instance.SetJumpClip(1);
        }
        if (context.canceled && _buttonHold == true && !_isJump)
        {
            if (dir == Direction.Up)
                pointResult += stepPoint * 2;

            _buttonHold = false;

            _canJump = true;
        }
    }

    public void GetTouchPosition(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _touchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            //Debug.Log(_touchPosition);
            var offset = ((Vector3)_touchPosition - transform.position).normalized;

            if (Mathf.Abs(offset.x) <= 0.7f)
            {
                dir = Direction.Up;
            }
            else if (offset.x < 0)
            {
                dir = Direction.Left;
            }
            else if (offset.x > 0)
            {
                dir = Direction.Right;
            }

        }
    }
    #endregion


    private void TriggerJump()
    {
        _canJump = false;
        switch (dir)
        {
            case Direction.Up:
                _anim.SetBool("isSide", false);
                _destination = new Vector2(transform.position.x, transform.position.y + _moveDistance);
                transform.localScale = Vector3.one;
                break;
            case Direction.Right:
                _anim.SetBool("isSide", true);
                _destination = new Vector2(transform.position.x + _moveDistance, transform.position.y);
                transform.localScale = new Vector3(-1, 1, 1);
                break;
            case Direction.Left:
                _anim.SetBool("isSide", true);
                _destination = new Vector2(transform.position.x - _moveDistance, transform.position.y);
                transform.localScale = Vector3.one;
                break;
        }
        _anim.SetTrigger("Jump");
    }

    #region Animation Event
    public void JumpAnimationEvent()
    {
        AudioManager.instance.PlayJumpFX();
        _isJump = true;

        sr.sortingLayerName = "Front";
        transform.parent = null;
    }

    public void FinishJumpAnimationEvent()
    {
        _isJump = false;
        sr.sortingLayerName = "Middle";
        if (dir == Direction.Up && !_isDead)
        {
            //TODO: 得分，触发地图检查
            // terrainMnaManager.CheckPosition();

            EventHandler.CallGetPointEvent(pointResult);
            Debug.Log("总得分：" + pointResult);
        }
    }
    #endregion

    private void DisableInput()
    {
        playerInput.enabled = false;
    }

    

}
