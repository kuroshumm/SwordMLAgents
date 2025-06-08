using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    //---------------------------------
    // Serialized Field
    //---------------------------------
    [SerializeField]
    private Camera _playerCamera;
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private Quaternion _rotation;
    [SerializeField]
    private Vector2 _rotationSpeed;

    //---------------------------------
    // 非公開 変数
    //---------------------------------
    private SwordManBase _player;

    //最後のマウス座標
    private Vector3 _lastMousePosition;
    //最後の追尾オブジェクトの座標
    private Vector3 _lastTargetPosition;     

    public void SetPlayer(SwordManBase player)
    {
        _player = player;
        _playerCamera.transform.position = _player.transform.position + _offset;
        _lastTargetPosition = _player.transform.position;

        _playerCamera.transform.rotation = _rotation;
        _lastMousePosition = Input.mousePosition;
    }

    void Start()
    {

    }

    void Update()
    {
        if (_playerCamera == null) return;
        if (_player == null) return;

        Rotate();
        FollowPlayer();
    }

    void FixedUpdate()
    {
        
    }

    void FollowPlayer()
    {
        _playerCamera.transform.position += _player.transform.position - _lastTargetPosition;
        _lastTargetPosition = _player.transform.position;
    }

    void Rotate()
    {
        //if (Input.GetMouseButton(0))
        {
            Vector3 nowMouseValue = Input.mousePosition - _lastMousePosition;

            var newAngle = Vector3.zero;
            newAngle.x = _rotationSpeed.x * nowMouseValue.x;
            newAngle.y = _rotationSpeed.y * nowMouseValue.y;

            _playerCamera.transform.RotateAround(_player.transform.position, Vector3.up, newAngle.x);
            //_playerCamera.transform.RotateAround(_player.transform.position, transform.right, -newAngle.y);
        }

        _lastMousePosition = Input.mousePosition;
    }
}
