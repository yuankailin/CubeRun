using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    private GameObject m_StartUI;
    private GameObject m_GameUI;

    private UILabel m_ScoreLabel;
    private UILabel m_GemLabel;
    private UILabel m_GameScoreLabel;
    private UILabel m_GameGemLabel;

    private GameObject m_PlayButton;

    private GameObject m_Left;
    private GameObject m_Right;

    private PlayerController m_PlayerController;

    void Start () {

        m_StartUI = GameObject.Find("Start_UI");
        m_GameUI = GameObject.Find("Game_UI");

        m_ScoreLabel = GameObject.Find("Score_Label").GetComponent<UILabel>();
        m_GemLabel = GameObject.Find("Gem_Label").GetComponent<UILabel>();
        m_GameScoreLabel = GameObject.Find("GameScoreLabel").GetComponent<UILabel>();
        m_GameGemLabel = GameObject.Find("GameGemLabel").GetComponent<UILabel>();

        m_PlayerController = GameObject.Find("cube_books").GetComponent<PlayerController>();

        m_PlayButton = GameObject.Find("Play_btn");
        UIEventListener.Get(m_PlayButton).onClick = PlayButtonClick;

        m_Left = GameObject.Find("Left");
        UIEventListener.Get(m_Left).onClick = Left;
        m_Right = GameObject.Find("Right");
        UIEventListener.Get(m_Right).onClick = Right;

        Init();

        m_GameUI.SetActive(false);
	}
	
	private void Init()
    {
        m_ScoreLabel.text = PlayerPrefs.GetInt("score", 0) + "";
        m_GemLabel.text = PlayerPrefs.GetInt("gem", 0) + "/100";
        m_GameScoreLabel.text = "0";
        m_GameGemLabel.text= PlayerPrefs.GetInt("gem", 0) + "/100";
    }

    public void UpdateData(int score, int gem)
    {
        m_GemLabel.text = gem + "/100";
        m_GameScoreLabel.text = score.ToString();
        m_GameGemLabel.text = gem + "/100";
    }

    private void PlayButtonClick(GameObject go)
    {
        Debug.Log("游戏开始");
        m_StartUI.SetActive(false);
        m_GameUI.SetActive(true);
        m_PlayerController.StartGame();
    }

    private void Left(GameObject go)
    {
        m_PlayerController.Left();
    }

    private void Right(GameObject go)
    {
        m_PlayerController.Right();
    }

    public void ResetUI()
    {
        m_StartUI.SetActive(true);
        m_GameUI.SetActive(false);
        m_GameScoreLabel.text = "0";
    }
}
