using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneResult : MonoBehaviour
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
			None,       // キャラ選択なし
			Detective,  // 探偵
			Phantom0,   // 怪盗 Red
			Phantom1,   // 怪盗 Blue
			Phantom2,   // 怪盗 Green
		};
		public int rank;							// 順位(1位が0)
		public eCharaId charaId;					// キャラID
		public int playerNo;						// プレイヤーNo(0から)
		public List<TreasureData> treasureDataList;	// 取得したお宝リスト(探偵はnull)
		public InitData(int rank, eCharaId charaId, int playerNo, List<TreasureData> treasureDataList)
		{
			this.rank = rank;
			this.charaId = charaId;
			this.playerNo = playerNo;
			this.treasureDataList = treasureDataList;
		}
	};
	public static List<InitData> initDatas = null;


	public static int m_winner;

	private float m_time = 5.0f;
											// Start is called before the first frame update

	public AudioSource winDetectiveSE;
	public AudioSource winPhantomSE;

	void Start()
    {
		var text = "";
		switch(m_winner) {
		case 0:
			text = "探偵";
			winDetectiveSE.enabled = true;
			break;
		case 1:
			winPhantomSE.enabled = true;
			text = "怪盗1";
			break;
		case 2:
			winPhantomSE.enabled = true;
			text = "怪盗2";
			break;
		case 3:
			winPhantomSE.enabled = true;
			text = "怪盗3";
			break;
		}
		//GameObject.Find("ResultText").GetComponent<Text>().text = string.Format("{0}の勝利！", text);
}

    // Update is called once per frame
    void Update()
    {
		

	
	}
}
