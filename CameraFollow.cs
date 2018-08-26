using UnityEngine;
using System.Collections;

/// <summary>
/// 摄像机跟随角色移动
/// </summary>
public class CameraFollow : MonoBehaviour {

    private Transform m_Transform;

    private Transform m_Player;

    public bool startFollow = false;

    private Vector3 normalPos;

	void Start () {

        m_Transform = gameObject.GetComponent<Transform>();
        normalPos = m_Transform.position;

        m_Player = GameObject.Find("cube_books").GetComponent<Transform>();
	}

	void Update () {

        CameraMove();
	}

    /// <summary>
    /// 摄像机移动
    /// </summary>
    void CameraMove()
    {
        if (startFollow)
        {
            //摄像机开始跟随
            Vector3 nextPos = new Vector3(m_Transform.position.x, m_Player.position.y + 1.5f, m_Player.position.z);
            //m_Transform.position = nextPos;
            m_Transform.position = Vector3.Lerp(m_Transform.position, nextPos, Time.deltaTime);
        }
    }

    public void ResetCamera()
    {
        m_Transform.position = normalPos;
    }
}
