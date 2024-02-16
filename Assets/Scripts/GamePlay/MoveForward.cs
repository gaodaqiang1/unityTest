using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float _speed;
    public int _dir;
    private Vector2 _startPos;

    private void Start()
    {
        _startPos = transform.position;
        transform.localScale = new Vector3(_dir, 1, 1);
    }
    void Update()
    {
        if (Mathf.Abs(transform.position.x - _startPos.x) > 22)
        {
            Destroy(this.gameObject);
        }
        Move();

        }

    private void Move()
    {
        transform.position += transform.right * _dir * _speed * Time.deltaTime;
    }
}
