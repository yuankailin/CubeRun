using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 地图管理器
/// </summary>
public class MapManager : MonoBehaviour {

    private GameObject m_prefab_tile;
    private GameObject m_prefab_wall;
    private GameObject m_prefab_spikes;
    private GameObject m_prefab_sky_spikes;
    private GameObject m_prefab_gem;

    //概率
    private int pr_hole = 0;
    private int pr_spikes = 0;
    private int pr_sky_spikes = 0;
    private int pr_gem = 2;

    //地图数据存储
    public List<GameObject[]> mapList = new List<GameObject[]>();

    private Transform m_Transform;
    private PlayerController m_PlayerController;

    public float bottomLength = Mathf.Sqrt(2) * 0.254f;

    private int index = 0;

    private Color colorWall = new Color(87 / 255f, 93 / 255f, 169 / 255f);
    private Color colorOne = new Color(124 / 255f, 155 / 255f, 230 / 255f);
    private Color colorTwo = new Color(125 / 255f, 169 / 255f, 233 / 255f);

    void Start () {

        m_prefab_tile = Resources.Load("tile_white") as GameObject;
        m_prefab_wall = Resources.Load("wall2") as GameObject;
        m_prefab_spikes = Resources.Load("moving_spikes") as GameObject;
        m_prefab_sky_spikes = Resources.Load("smashing_spikes") as GameObject;
        m_prefab_gem = Resources.Load("gem 2") as GameObject;

        m_Transform = gameObject.GetComponent<Transform>();
        m_PlayerController = GameObject.Find("cube_books").GetComponent<PlayerController>();

        CreateMapItem(0);
	}
	
    /// <summary>
    /// 创建地图元素
    /// </summary>
    public void CreateMapItem(float offsetZ)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject[] item = new GameObject[6];
            for (int j = 0; j < 6; j++)
            {
                Vector3 pos = new Vector3(j * bottomLength, 0, i * bottomLength + offsetZ);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = null;
                if (j == 0 || j == 5)
                {
                    tile = GameObject.Instantiate(m_prefab_wall, pos, Quaternion.Euler(rot)) as GameObject;
                    tile.GetComponent<MeshRenderer>().material.color = colorWall;
                }
                else
                {
                    int pr = CalcPR();
                    if (pr == 0)
                    {
                        tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot)) as GameObject;
                        tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorOne;
                        tile.GetComponent<MeshRenderer>().material.color = colorOne;
                    }
                    else if (pr == 1) //坑洞
                    {
                        tile = new GameObject();
                        tile.GetComponent<Transform>().position = pos;
                        tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                    }
                    else if (pr == 2) //地面陷阱
                    {
                        tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot)) as GameObject;
                    }
                    else if (pr == 3) //天空陷阱
                    {
                        tile = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot)) as GameObject;
                    }
                }
                
                tile.GetComponent<Transform>().SetParent(m_Transform);
                item[j] = tile;
            }
            mapList.Add(item);

            GameObject[] item2 = new GameObject[5];
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos = new Vector3(j * bottomLength + bottomLength / 2, 0, i * bottomLength + bottomLength / 2 + offsetZ);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = null;

                int pr = CalcPR();
                if (pr == 0)
                {
                    tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot)) as GameObject;
                    tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorTwo;
                    tile.GetComponent<MeshRenderer>().material.color = colorTwo;
                    int gemPr = CalcGemPR();
                    if(gemPr==1)
                    {
                        //生成宝石
                        GameObject gem = GameObject.Instantiate(m_prefab_gem, tile.GetComponent<Transform>().position + new Vector3(0, 0.06f, 0), Quaternion.identity) as GameObject;
                        gem.GetComponent<Transform>().SetParent(tile.GetComponent<Transform>());
                    }
                }
                else if (pr == 1) //坑洞
                {
                    tile = new GameObject();
                    tile.GetComponent<Transform>().position = pos;
                    tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                }
                else if (pr == 2) //地面陷阱
                {
                    tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot)) as GameObject;
                }
                else if (pr == 3) //天空陷阱
                {
                    tile = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot)) as GameObject;
                }

                tile.GetComponent<Transform>().SetParent(m_Transform);
                item2[j] = tile;
            }
            mapList.Add(item2);
        }
    }
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            string str = "";
            for (int i = 0; i < mapList.Count; i++)
            {
                for (int j = 0; j < mapList[i].Length; j++)
                {
                    str += mapList[i][j].name;
                    mapList[i][j].name = i + "--" + j;
                }
                str += "\n";
            }
            Debug.Log(str);
        }
    }

    /// <summary>
    /// 开启地面塌陷效果
    /// </summary>
    public void StartTileDown()
    {
        StartCoroutine("TileDown");
    }

    /// <summary>
    /// 停止地面塌陷效果
    /// </summary>
    public void StopTileDown()
    {
        StopCoroutine("TileDown");
    }

    /// <summary>
    /// 地面塌陷
    /// </summary>
    /// <returns></returns>
    private IEnumerator TileDown()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < mapList[index].Length; i++)
            {
                Rigidbody rb = mapList[index][i].AddComponent<Rigidbody>();
                rb.angularVelocity = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * Random.Range(1.0f, 10.0f);
                GameObject.Destroy(mapList[index][i], 1.0f);
            }
            
            if (m_PlayerController.z == index) 
            {
                StopTileDown();
                m_PlayerController.gameObject.AddComponent<Rigidbody>();
                m_PlayerController.StartCoroutine("GameOver", true);
            }

            index++;
        }
    }

    /// <summary>
    /// 计算概率
    /// 0：瓷砖
    /// 1：坑洞
    /// 2：地面陷阱
    /// 3：天空陷阱
    /// </summary>
    /// <returns></returns>
    private int CalcPR()
    {
        int pr = Random.Range(1, 100);
        if (pr <= pr_hole)
        {
            return 1;
        }
        else if (31 < pr && pr < pr_spikes + 30)
        {
            return 2;
        }
        else if (61 < pr && pr < pr_sky_spikes + 60)
        {
            return 3;
        }
        return 0;
    }

    /// <summary>
    /// 计算宝石生成概率
    /// </summary>
    /// <returns>0：不生成；1：生成</returns>
    private int CalcGemPR()
    {
        int pr = Random.Range(1, 100);
        if (pr < pr_gem)
        {
            return 1;
        }
        return 0;
    }

    /// <summary>
    /// 增加概率
    /// </summary>
    public void AddPR()
    {
        pr_hole += 2;
        pr_spikes += 2;
        pr_sky_spikes += 2;
    }

    public void ResetGameMap()
    {
        Transform[] sonTransform = m_Transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < sonTransform.Length; i++)
        {
            GameObject.Destroy(sonTransform[i].gameObject);
        }

        pr_hole = 0;
        pr_spikes = 0;
        pr_sky_spikes = 0;
        pr_gem = 2;

        index = 0;

        mapList.Clear();

        CreateMapItem(0);
    }
}
