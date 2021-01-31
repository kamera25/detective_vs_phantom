using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMain : MonoBehaviour
{
	/// <summary>
	/// 初期化用データ
	/// </summary>
	public class InitData
	{
		/// <summary>
		/// キャラID
		/// </summary>
		public enum eCharaId
		{
			None,		// キャラ選択なし
			Detective,	// 探偵
			Phantom0,	// 怪盗 Red
			Phantom1,	// 怪盗 Blue
			Phantom2,	// 怪盗 Green
		};
		public eCharaId charaId;
		public int playerNo;
		public InitData(eCharaId charaId, int playerNo)
		{
			this.charaId = charaId;
			this.playerNo = playerNo;
		}
	};
	public static List<InitData> initDatas = null;

	enum State
	{
		Tips,	// tips表示
		Start,  // ゲーム開始時演出
		Main,   // ゲームメイン
		End     // ゲーム終了演出
	}
	private State m_state = State.Tips;    // 状態遷移
	float m_tipsTimer = 2.0f;
	private int m_nowTresureIndex = 0;      // 現在のお宝
	private float m_playGameTime = 120.0f;       // ゲームの制限時間
	private float m_playTimeCount = 0.0f;       // ゲームの経過時間
	private List<TreasureData> m_stockTreasures = new List<TreasureData>();    // 残りお宝
	private List<TreasureData> m_fieldTreasures = new List<TreasureData>();    // 出現中お宝
	private Text m_treasureNumText;
	private RectTransform m_moonBeginTransform;	// 残り時間UIの行列 開始位置
	private RectTransform m_moonEndTransform;	// 残り時間UIの行列 終了位置
	private RectTransform m_moonTransform;		// 残り時間UIの行列
	private Text m_timeText;

	private int m_treasureMax = 2;			// ステージに同時に存在する数
	private int m_treasureMaxAll;			// ゲーム全体のお宝の数
											// Start is called before the first frame update
											/// <summary>
											/// お宝データベース
											/// </summary>
	public class TresurePosDataBase
	{
		public Vector3 m_pos;
		public bool m_isPut;    // お宝がある？
		public TresurePosDataBase(Vector3 pos)
		{
			m_pos = pos;
			m_isPut = false;
		}
	};
	public class PosDataBase
	{
		public Vector3 m_pos;
		public PosDataBase(Vector3 pos)
		{
			m_pos = pos;
		}
	};
	private TresurePosDataBase[] m_TresurePosDatas;
    public TresurePosDataBase[] TresurePosDatas{get{return m_TresurePosDatas;} }
	private Vector3[] m_phantomRespawnPosDatas;
	public Vector3[] PhantomRespawnPosDatas { get { return m_phantomRespawnPosDatas; } }
	Vector3 m_detectivePosData;
	private Detective m_detective = null;
	private Phantom m_phantom1 = null;
	private Phantom m_phantom2 = null;
	private Phantom m_phantom3 = null;

	void Awake()
	{
		SceneManager.LoadScene("MainUi", LoadSceneMode.Additive);
	}
	void Start()
	{
		// デバッグ用
		if(initDatas==null) {
			initDatas = new List<InitData>();
			initDatas.Add(new InitData(InitData.eCharaId.Detective, 0));
			initDatas.Add(new InitData(InitData.eCharaId.Phantom0, 1));
			initDatas.Add(new InitData(InitData.eCharaId.Phantom1, 2));
			initDatas.Add(new InitData(InitData.eCharaId.Phantom2, 3));
		}

		// 座標取得
		var pointTrans = GameObject.Find("Points").transform;
		// お宝の座標
		m_TresurePosDatas = new TresurePosDataBase[4];
		m_TresurePosDatas[0] = new TresurePosDataBase(pointTrans.Find("ItemRespawnPoints/P1").position);
		m_TresurePosDatas[1] = new TresurePosDataBase(pointTrans.Find("ItemRespawnPoints/P2").position);
		m_TresurePosDatas[2] = new TresurePosDataBase(pointTrans.Find("ItemRespawnPoints/P3").position);
		m_TresurePosDatas[3] = new TresurePosDataBase(pointTrans.Find("ItemRespawnPoints/P4").position);
		// 怪盗リスポーン座標
		m_phantomRespawnPosDatas = new Vector3[4];
		m_phantomRespawnPosDatas[0] = pointTrans.Find("PhantomRespawnPoints (1)/P1").position;
		m_phantomRespawnPosDatas[1] = pointTrans.Find("PhantomRespawnPoints (1)/P2").position;
		m_phantomRespawnPosDatas[2] = pointTrans.Find("PhantomRespawnPoints (1)/P3").position;
		m_phantomRespawnPosDatas[3] = pointTrans.Find("PhantomRespawnPoints (1)/P4").position;
		// 探偵の座標
		m_detectivePosData = pointTrans.Find("DetectiveRespawnPoints (2)/P1").position;

		// 全体のお宝
		m_stockTreasures.Add(new TreasureData(eTreasureType.Ring));
		m_stockTreasures.Add(new TreasureData(eTreasureType.Ring));
		m_stockTreasures.Add(new TreasureData(eTreasureType.Ring));
		m_stockTreasures.Add(new TreasureData(eTreasureType.Bracelet));
		m_stockTreasures.Add(new TreasureData(eTreasureType.Bracelet));
		m_stockTreasures.Add(new TreasureData(eTreasureType.Crown));
		m_treasureMaxAll = m_stockTreasures.Count;

		m_treasureNumText = GameObject.Find("TreasureNum").GetComponent<Text>();
		m_treasureNumText.gameObject.SetActive(false);
		m_moonBeginTransform = GameObject.Find("MoonBegin").GetComponent<RectTransform>();
		m_moonEndTransform = GameObject.Find("MoonEnd").GetComponent<RectTransform>();
		m_moonTransform = GameObject.Find("moon").GetComponent<RectTransform>();
		m_moonTransform.gameObject.SetActive(false);
		m_timeText = GameObject.Find("timeText").GetComponent<Text>();

		UpdateTreasureNumText();

		for (int i = 0; i < initDatas.Count; i++) {
			if(initDatas[i].charaId == InitData.eCharaId.None) { continue; }
			string path = "";
			Vector3 pos = Vector3.zero;
			switch(initDatas[i].charaId) {
			case InitData.eCharaId.Detective:
				path = "Prefabs/Character/Detective";
				pos = m_detectivePosData;
				break;
			case InitData.eCharaId.Phantom0:
				path = "Prefabs/Character/Phantom";
				pos = m_phantomRespawnPosDatas[0];
				break;
			case InitData.eCharaId.Phantom1:
				path = "Prefabs/Character/Phantom2";
				pos = m_phantomRespawnPosDatas[1];
				break;
			case InitData.eCharaId.Phantom2:
				path = "Prefabs/Character/Phantom3";
				pos = m_phantomRespawnPosDatas[2];
				break;
			}
			var obj = Instantiate(Resources.Load(path)) as GameObject;
			obj.transform.position = pos;
			switch (initDatas[i].charaId) {
			case InitData.eCharaId.Detective:
				m_detective = obj.GetComponent<Detective>();
				m_detective.Init(initDatas[i]);
				break;
			case InitData.eCharaId.Phantom0:
				m_phantom1 = obj.GetComponent<Phantom>();
				m_phantom1.Init(initDatas[i]);
				break;
			case InitData.eCharaId.Phantom1:
				m_phantom2 = obj.GetComponent<Phantom>();
				m_phantom2.Init(initDatas[i]);
				break;
			case InitData.eCharaId.Phantom2:
				m_phantom3 = obj.GetComponent<Phantom>();
				m_phantom3.Init(initDatas[i]);
				break;
			}
		}
		//Instantiate(Resources.Load("Prefabs/Stage/Stage00"));
		CreateTresure();

		FadeManager.TipsOn();
	}
	class ScoreData
	{
		public int score;
		public Phantom phantom;
		public SceneResult.InitData.eCharaId charaId;
		public ScoreData(int score, Phantom phantom, SceneResult.InitData.eCharaId charaId)
		{
			this.score = score;
			this.phantom = phantom;
			this.charaId = charaId;
		}
	}
	// Update is called once per frame
	void Update()
	{
		switch (m_state) {
		case State.Tips:
			if(m_tipsTimer > 0.0f) {
				m_tipsTimer -= Time.deltaTime;
				break;
			}
			if (GetInput(0).magnitude > 0.5f || GetInput(1).magnitude > 0.5f || GetInput(2).magnitude > 0.5f || GetInput(3).magnitude > 0.5f) {
				FadeManager.TipsOff();
				FadeManager.FadeIn();
				m_state = State.Start;
			}
			break;
		case State.Start:
			if (FadeManager.IsFadeEnd()==false) {
				break;
			}
			m_treasureNumText.gameObject.SetActive(true);
			m_moonTransform.gameObject.SetActive(true);
			m_state = State.Main;
			break;
		case State.Main:
			m_playTimeCount += Time.deltaTime;
			// 残り個数表示
			UpdateTreasureNumText();
			// 残り時間表示
			{
				var moonBegin = m_moonBeginTransform.localPosition;
				var moonEnd = m_moonEndTransform.localPosition;
				float ratio = m_playTimeCount / m_playGameTime;
				var x = Mathf.Lerp(moonBegin.x, moonEnd.x, ratio);
				m_moonTransform.localPosition = new Vector3(x, moonBegin.y, 0);
				m_timeText.text = Math.Ceiling(m_playGameTime - m_playTimeCount).ToString();
			}

			// ゲームメイン終了
			{
				var isGameEnd = false;
				var isWinDetective = false;
				if (m_playTimeCount >= m_playGameTime) {
					isGameEnd = true;
					isWinDetective = true;
				}
				if (m_stockTreasures.Count == 0 && m_fieldTreasures.Count == 0) {
					isGameEnd = true;
				}

				if (isGameEnd) {
					int rank = 0;
					List<int> phantomRank = new List<int>();
					// 怪盗の順位を判定
					int score1 = m_phantom1 != null ? m_phantom1.GetScore() : 0;
					int score2 = m_phantom2 != null ? m_phantom2.GetScore() : 0;
					int score3 = m_phantom3 != null ? m_phantom3.GetScore() : 0;
					List<ScoreData> scoreDatas = new List<ScoreData>();
					scoreDatas.Add(new ScoreData(score1, m_phantom1, SceneResult.InitData.eCharaId.Phantom0));
					scoreDatas.Add(new ScoreData(score2, m_phantom2, SceneResult.InitData.eCharaId.Phantom1));
					scoreDatas.Add(new ScoreData(score3, m_phantom3, SceneResult.InitData.eCharaId.Phantom2));
					scoreDatas.Sort((a, b) => b.score - a.score);

					// リザルト用のデータを格納
					SceneResult.initDatas = new List<SceneResult.InitData>();
					if (isWinDetective) {	// 探偵勝利
						SceneResult.initDatas.Add(new SceneResult.InitData(++rank, SceneResult.InitData.eCharaId.Detective, m_detective.m_initData.playerNo, null));
						for (int i = 0; i < scoreDatas.Count; i++) {
							var scoreData = scoreDatas[i];
							if (scoreData.phantom != null) {
								SceneResult.initDatas.Add(new SceneResult.InitData(++rank, scoreData.charaId, scoreData.phantom.m_initData.playerNo, scoreData.phantom.m_treasures));
							}
						}
					}else { // 怪盗勝利
						for (int i = 0; i < scoreDatas.Count; i++) {
							var scoreData = scoreDatas[i];
							if (scoreData.phantom != null) {
								SceneResult.initDatas.Add(new SceneResult.InitData(++rank, scoreData.charaId, scoreData.phantom.m_initData.playerNo, scoreData.phantom.m_treasures));
							}
						}
						SceneResult.initDatas.Add(new SceneResult.InitData(++rank, SceneResult.InitData.eCharaId.Detective, m_detective.m_initData.playerNo, null));
					}
					m_treasureNumText.gameObject.SetActive(false);
					m_moonTransform.gameObject.SetActive(true);
					m_state = State.End;
				}
			}
			break;
		case State.End:
			m_timeText.gameObject.SetActive(false);
			SceneManager.LoadScene("ResultScene");
			break;
		}
	}
	/// <summary>
	/// お宝生成
	/// </summary>
	public void CreateTresure()
	{
		while (m_fieldTreasures.Count < m_treasureMax && m_stockTreasures.Count > 0) {
			var data = m_stockTreasures[m_stockTreasures.Count - 1];
			m_stockTreasures.RemoveAt(m_stockTreasures.Count - 1);
			string fileName = "";
			switch (data.type) {
			case eTreasureType.Ring:
				fileName = "Prefabs/Treasure/TreasureRing";
				break;
			case eTreasureType.Bracelet:
				fileName = "Prefabs/Treasure/TreasureBracelet";
				break;
			case eTreasureType.Crown:
				fileName = "Prefabs/Treasure/TreasureCrown";
				break;
			}
			var tresure = Instantiate(Resources.Load(fileName)) as GameObject;

			// 出現していない場所にお宝を生成
			List<int> indexList = new List<int>();
			for (int i = 0; i < m_TresurePosDatas.Length; i++) {
				if (m_TresurePosDatas[i].m_isPut == false) {
					indexList.Add(i);
				}
			}
			var random = new System.Random();
			var rand = random.Next(0, indexList.Count);
			var index = indexList[rand];
			m_TresurePosDatas[index].m_isPut = true;
			tresure.transform.position = m_TresurePosDatas[index].m_pos;
			tresure.tag = "tresure";
			tresure.GetComponent<Treasure>().data = data;

			m_nowTresureIndex = index;
			m_fieldTreasures.Add(data);
		}
	}
	/// <summary>
	/// お宝を落とした時の処理
	/// </summary>
	public void TropTresure(TreasureData treasureData)
	{
		m_stockTreasures.Add(treasureData);
		CreateTresure();
	}
	/// <summary>
	/// お宝を獲得した時の処理
	/// </summary>
	public void CaptureTresure(Treasure treasure)
	{
		m_fieldTreasures.Remove(treasure.data);
		Destroy(treasure.gameObject);
		CreateTresure();
		for (int i = 0; i < m_TresurePosDatas.Length; i++) {
			if (m_TresurePosDatas[i].m_pos == treasure.transform.position) {
				m_TresurePosDatas[i].m_isPut = false;
				break;
			}
		}
	}

	/// <summary>
	/// お宝の残り数表示
	/// </summary>
	void UpdateTreasureNumText()
	{
		m_treasureNumText.text = string.Format("{0}/{1}", m_fieldTreasures.Count + m_stockTreasures.Count, m_treasureMaxAll);
	}

	/// <summary>
	/// プレイヤーNoに応じたキー入力取得
	/// </summary>
	/// <param name="playerNo">プレイヤーNo</param>
	/// <returns>移動量</returns>
	public static Vector3 GetInput(int playerNo)
	{
		float workspeed = 0.2f;//歩くスピード
		float runspeed = 0.9f;//走るスピード
		int key1 = 0;
		int key2 = 0;
		Vector3 vector = Vector3.zero;
		vector.x = Input.GetAxis(string.Format("GamePad{0}_X", playerNo));
		vector.z = Input.GetAxis(string.Format("GamePad{0}_Y", playerNo));


		switch (playerNo) {
		case 0:
			//左
			if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.Tab)) {
				key1 = -3;
			} else if (Input.GetKey(KeyCode.LeftArrow)) {
				key1 = -1;
			}
			//右
			if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.Tab)) {
				key1 = 3;
			} else if (Input.GetKey(KeyCode.RightArrow)) {
				key1 = 1;
			}
			//上
			if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.Tab)) {
				key2 = 3;
			} else if (Input.GetKey(KeyCode.UpArrow)) {
				key2 = 1;
			}
			//下
			if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.Tab)) {
				key2 = -3;
			} else if (Input.GetKey(KeyCode.DownArrow)) {
				key2 = -1;
			}
			break;
		case 1://左
			if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.RightShift)) {
				key1 = -3;
			} else if (Input.GetKey(KeyCode.A)) {
				key1 = -1;
			}
			//右
			if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.RightShift)) {
				key1 = 3;
			} else if (Input.GetKey(KeyCode.D)) {
				key1 = 1;
			}
			//上
			if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.RightShift)) {
				key2 = 3;
			} else if (Input.GetKey(KeyCode.W)) {
				key2 = 1;
			}
			//下
			if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.RightShift)) {
				key2 = -3;
			} else if (Input.GetKey(KeyCode.S)) {
				key2 = -1;
			}
			break;
		case 2:
			//左
			if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.P)) {
				key1 = -3;
			} else if (Input.GetKey(KeyCode.F)) {
				key1 = -1;
			}
			//右
			if (Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.P)) {
				key1 = 3;
			} else if (Input.GetKey(KeyCode.H)) {
				key1 = 1;
			}
			//上
			if (Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.P)) {
				key2 = 3;
			} else if (Input.GetKey(KeyCode.T)) {
				key2 = 1;
			}
			//下
			if (Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.P)) {
				key2 = -3;
			} else if (Input.GetKey(KeyCode.G)) {
				key2 = -1;
			}

			break;
		case 3:

			//左
			if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.Q)) {
				key1 = -3;
			} else if (Input.GetKey(KeyCode.J)) {
				key1 = -1;
			}
			//右
			if (Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.Q)) {
				key1 = 3;
			} else if (Input.GetKey(KeyCode.L)) {
				key1 = 1;
			}
			//上
			if (Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.Q)) {
				key2 = 3;
			} else if (Input.GetKey(KeyCode.I)) {
				key2 = 1;
			}
			//下
			if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.Q)) {
				key2 = -3;
			} else if (Input.GetKey(KeyCode.K)) {
				key2 = -1;
			}
			break;
		}
		//加算する
		if (key1 != 0 || key2 != 0) {
			float s = 0.0f;
			float s2 = 0.0f;
			switch (key1) {
			case 0:
				s = 0.0f;
				break;
			case 1:
				s = workspeed;
				break;
			case 3:
				s = runspeed;
				break;
			case -1:
				s = -workspeed;
				break;
			case -3:
				s = -runspeed;
				break;
			}
			switch (key2) {
			case 0:
				s2 = 0.0f;
				break;
			case 1:
				s2 = workspeed;
				break;
			case 3:
				s2 = runspeed;
				break;
			case -1:
				s2 = -workspeed;
				break;
			case -3:
				s2 = -runspeed;
				break;
			}
			vector = new Vector3(s, 0, s2);
		}

		return vector;
	}
	/// <summary>
	/// ゲーム中
	/// </summary>
	/// <returns></returns>
	public bool IsGamePlay()
	{
		return m_state == State.Main || m_state == State.Start;
	}
}
