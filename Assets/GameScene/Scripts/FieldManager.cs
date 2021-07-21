using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// フィールドの位置及びサイズの調整をする
/// </summary>
public class FieldManager : MonoBehaviour
{

    [Tooltip("フィールドの範囲\n右左上下")]
    [SerializeField] float[] m_fieldErea = new float[4];

    //  フィールドのサイズ
    Vector2 m_size;
    //  フィールドの中心位置
    Vector3 m_position;

    /// <summary>プレイ領域右端</summary>
    public float FieldEreaRX
    {
        get { return m_fieldErea[0]; }
    }
    /// <summary>プレイ領域左端</summary>
    public float FieldEreaLX
    {
        get { return m_fieldErea[1]; }
    }
    /// <summary>プレイ領域上端</summary>
    public float FieldEreaTY
    {
        get { return m_fieldErea[2]; }
    }
    /// <summary>プレイ領域下端</summary>
    public float FieldEreaUY
    {
        get { return m_fieldErea[3]; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //  フィールドの領域を取得し、サイズに直し、調整
        float fieldSizeX = FieldEreaRX - FieldEreaLX;
        float fieldSizeY = FieldEreaTY - FieldEreaUY;
        m_size = new Vector2(fieldSizeX, fieldSizeY);
        transform.localScale = m_size;

        //  フィールドの領域を取得し、中心の座標に直し、調整
        float fieldPositionX = FieldEreaRX - fieldSizeX / 2;
        float fieldPositionY = FieldEreaTY - fieldSizeY / 2;
        m_position = new Vector3(fieldPositionX, fieldPositionY, 20);
        transform.position = m_position;
    }

    private void Start()
    {
        transform.position = m_position;
    }

    /// <summary>フィールドのサイズ</summary>
    public Vector2 Size
    {
        get { return m_size; }
    }

    /// <summary>フィールドの中心位置</summary>
    public Vector2 Position
    {
        get { return m_position; }
    }
}
