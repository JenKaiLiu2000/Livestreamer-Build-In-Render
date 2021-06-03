using UnityEngine;
/// <summary>
/// 包含Message外觀設計的所有參數，Message Creator依照此類別的參數生產Message物件。
/// </summary>
public class MessageDisplaySetting : MonoBehaviour
{
    [Header("Origin Prefab")]
    public GameObject _imagePrefab;
    public GameObject _textPrefab;
    public GameObject _dialogBoxPrefab;
    [Header("Dialog Box Setting")]
    [Tooltip("字級大小")]
    public int _fontSize = 30; //字級大小
    [Tooltip("對話框寬度")]
    public float _textBox_MaxWidth = 450; //對話框寬度
    [Tooltip("對話框上下padding距離")]
    public float _dialogBox_Top_Botton_Padding = 15; //對話框上下padding距離
    [Tooltip("對話框左右padding距離")]
    public float _dialogBox_Left_Right_Padding = 20; //對話框左右padding距離
    [Tooltip("文字顏色")]
    public Color _textColor = new Color(255, 255, 255, 195); //文字顏色
    [Tooltip("對話框顏色")]
    public Color _dialogBoxColor = new Color(0, 0, 0, 65); //對話框顏色
}
