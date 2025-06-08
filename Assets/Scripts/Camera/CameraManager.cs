using UnityEngine;

/// <summary>
/// カメラ管理を担当するマネージャークラス
/// </summary>
public class CameraManager
{
    private PlayerCameraManager _playerCameraManager;
    private SwordManBase _targetPlayer;

    public void Init(PlayerCameraManager playerCameraManager)
    {
        _playerCameraManager = playerCameraManager;
    }

    public void SetTargetPlayer(SwordManBase player)
    {
        _targetPlayer = player;
        if (_playerCameraManager != null)
        {
            _playerCameraManager.SetPlayer(player);
        }
    }

    public void ResetCamera()
    {
        if (_playerCameraManager != null && _targetPlayer != null)
        {
            _playerCameraManager.SetPlayer(_targetPlayer);
        }
    }
} 