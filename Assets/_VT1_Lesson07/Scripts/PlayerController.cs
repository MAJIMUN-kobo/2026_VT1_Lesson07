using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("* * * 移動設定 * * *")]
    [SerializeField] private float _moveSpeed;  // 基本移動速度

    [Header("* * * カメラ設定 * * *")]
    [SerializeField] private CameraController _cameraController;

    [Header("* * * Player Input * * *")]
    [SerializeField] private PlayerInput _playerInput;

    // === Private 変数
    private Vector3 _velocity;          // 移動ベクトル
    private Vector3 _inputMoveVector;   // 移動用の入力値


    void Start()
    {
        MoveInputEnable();  // 移動入力の有効化
    }

    void Update()
    {
        Movement();         // 移動処理
    }


    /// <summary>
    /// 移動メソッド
    /// </summary>
    public void Movement()
    {
        // 入力値を実際の移動平面に変換
        var inputAxis = new Vector3( _inputMoveVector.x, 0, _inputMoveVector.y );

        // === 👇カメラの向いている方向を移動ベクトルとする処理
        // 1. カメラの正面ベクトルを取得
        var cameraForward = _cameraController.transform.forward;
        // 2. 入力値と合わせる
        var moveVector = (inputAxis.x * _cameraController.transform.right) + (inputAxis.z * cameraForward);
        moveVector.y = 0;

        // （仮）入力をそのまま移動ベクトルへ...
        _velocity = moveVector.normalized;

        Vector3 finalVelocity = _moveSpeed * _velocity * Time.deltaTime;
        transform.Translate(finalVelocity, Space.World);
    }


    /// <summary>
    /// 移動入力の有効化
    /// </summary>
    public void MoveInputEnable()
    {
        if( _playerInput == null )
        {
            Debug.LogError("PlayerInputが設定されていません。");
            return;
        }

        // 移動入力の登録
        _playerInput.actions["Move"].performed += OnMoveInput;
        _playerInput.actions["Move"].canceled += OnMoveCanceled;

        // アクションの有効化
        _playerInput.actions["Move"].Enable();
    }


    private void OnMoveInput(InputAction.CallbackContext context)
    {
        // 入力時の処理
        var value = context.ReadValue<Vector2>();   // 入力値の取得
        _inputMoveVector = value;                   // 変数に保存
    }


    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _inputMoveVector = Vector3.zero;    // 変数の値をリセット
    }
}
