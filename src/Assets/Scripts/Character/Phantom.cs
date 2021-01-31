using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 怪盗
public class Phantom : MonoBehaviour
{
	public int m_phantomNo = 0;
	public Material m_normalMat;
	public Material m_fadeMat;


	enum eState {
        Normal, // 通常状態
        Stan,   // スタン
    }
	enum eAnimState
	{
		Stop,	// 停止
		Walk,	// 歩き
		Run,	// 走り
		Stan,	// スタン
	};
    eState m_state = eState.Normal;
	eAnimState m_animState = eAnimState.Stop;
    float m_stanTime = 0.0f;
	class ScoreData
	{
		public TreasureData m_treasureData;
		public GameObject m_obj;
		public ScoreData(TreasureData treasureData, GameObject obj)
		{
			this.m_treasureData = treasureData;
			this.m_obj = obj;
		}
	}
	List<ScoreData> m_scoreDatas = new List<ScoreData>();
	public List<TreasureData> m_treasures = new List<TreasureData>();
	private SceneMain m_sceneMain;
	private Animator m_animator;
	private const float AppearTime = 1.0f;	// Run後の強制出現時間
	private float m_appearTime = 0.0f;      // Run後の強制出現時間カウンタ
	private GameObject m_stunEffect = null;
	private GameObject m_runEffect = null;
	private GameObject m_scoreParent = null;

	float m_speed = 10; // スピード
	float m_runThreshold = 0.8f;		// 走り判定になる傾き
	float m_visibleThreshold = 0.2f;	// 完全透過になる傾き
	Renderer m_renderer;
    Rigidbody m_rigidbody;
	GameObject phantom;
	private GameObject m_textMeshObj;
	public SceneMain.InitData m_initData;
	AudioSource m_audio;
	public AudioClip m_hitSE;
	public void Init(SceneMain.InitData initData)
	{
		m_initData = initData;
		switch(initData.charaId) {
		case SceneMain.InitData.eCharaId.Phantom0:
			m_phantomNo = 0;
			break;
		case SceneMain.InitData.eCharaId.Phantom1:
			m_phantomNo = 1;
			break;
		case SceneMain.InitData.eCharaId.Phantom2:
			m_phantomNo = 2;
			break;
		}
	}
	// Start is called before the first frame update
	void Start()
	{
		phantom = transform.Find("phantom/phantom").gameObject;
		m_renderer = phantom.GetComponent<Renderer>();
        m_rigidbody = GetComponent<Rigidbody>();
		m_animator = phantom.GetComponent<Animator>();
		m_textMeshObj = transform.Find("TextMesh").gameObject;
		m_textMeshObj.GetComponent<TextMesh>().text = string.Format("{0}P", m_initData.playerNo+1);
		m_audio = this.GetComponent<AudioSource>();
		m_scoreParent = GameObject.Find(string.Format("Score{0}", m_phantomNo));

		m_sceneMain = GameObject.Find("SceneMain").GetComponent<SceneMain>();
		// 当たり判定関数を登録
		var hit = transform.Find("Hit").gameObject.GetComponent<HitClass>().hitCallback = Hit;

		UpdateScoreUi();
	}

	// Update is called once per frame
	void Update()
	{
		if (m_sceneMain.IsGamePlay() == false) { return; }
		switch (m_state) {
		case eState.Normal:
			Move();
			break;
		case eState.Stan:
			m_stanTime -= Time.deltaTime;

			if (m_stanTime <= 0.0f) {
				// エフェクトを消す
				GameObject.Destroy(m_stunEffect);
				m_stunEffect = null;
				// 復活モーション
				m_animator.SetTrigger("pop");
				// どこかにワープ
				var random = new System.Random();
				var rand = random.Next(0, m_sceneMain.TresurePosDatas.Length);
				Vector3 pos = m_sceneMain.TresurePosDatas[rand].m_pos;
				pos.x += 1.0f;
				pos.y = 1.0f;
				transform.position = pos;
				m_rigidbody.velocity = Vector3.zero;
				m_state = eState.Normal;
				// 音を停止
				m_audio.Stop();
			}
			break;
		}
	}
	/// <summary>
	/// 当たり判定処理
	/// </summary>
	/// <param name="other">当たったオブジェクト</param>
	void Hit(Collider other)
	{
		// お宝を取得した時
		if (other.tag == "tresure") {
			var treasure = other.gameObject.GetComponent<Treasure>();
			m_treasures.Add(treasure.data);
			m_sceneMain.CaptureTresure(treasure);
		}
		// 探偵に捕まった時の処理
		if(other.gameObject.GetComponent<Detective>() != null) {
			if (m_state != eState.Stan) {
				Arrest();
			}
		}
		UpdateScoreUi();
	}
	/// <summary>
	/// 捕まった時の処理
	/// </summary>
	void Arrest()
	{
		// お宝を落とす
		if (m_treasures.Count > 0) {
			m_sceneMain.TropTresure(m_treasures[m_treasures.Count-1]);
			m_treasures.RemoveAt(m_treasures.Count - 1);
		}

		// スタン
		m_state = eState.Stan;
	    m_stanTime = 5.0f;
		m_rigidbody.velocity = Vector3.zero;
		if(m_stunEffect!=null) {
			GameObject.Destroy(m_stunEffect);
			m_stunEffect = null;
		}
		m_stunEffect = Instantiate(Resources.Load("Prefabs/Effect/stuneffect")) as GameObject;
		m_stunEffect.transform.position = transform.position;
		m_animator.SetTrigger("shock");

		// 姿を表す
		m_renderer.material = m_normalMat;
		var color = m_renderer.material.color;
		color.a = 1.0f;
		m_renderer.material.color = color;
		m_textMeshObj.SetActive(true);

		// 音を鳴らす
		m_audio.PlayOneShot(m_hitSE);
		m_audio.Play();
	}
	/// <summary>
	/// スコア計算
	/// </summary>
	/// <returns>スコア</returns>
	int CalcScore()
	{
		int sum = 0;
		for(int i=0;i<m_treasures.Count;i++) {
			sum += m_treasures[i].score;
		}
		return sum;
	}
	/// <summary>
	/// スコア取得
	/// </summary>
	/// <returns></returns>
	public int GetScore()
	{
		return CalcScore();
	}
	/// <summary>
	/// スコアのUI更新
	/// </summary>
	private void UpdateScoreUi()
	{
		// 削除処理
		while(m_scoreDatas.Count > m_treasures.Count) {
			int index = m_scoreDatas.Count - 1;
			GameObject.Destroy(m_scoreDatas[index].m_obj);
			m_scoreDatas.RemoveAt(index);
		}

		// 追加処理
		for (int i = 0; i < m_treasures.Count; i++) {
			if (m_scoreDatas.Count > i) {
				continue;
			}
			// モデル読み込み
			string fileName = "";
			switch (m_treasures[i].type) {
			case eTreasureType.Ring:
				fileName = "Prefabs/Treasure/ScoreTreasureRing";
				break;
			case eTreasureType.Bracelet:
				fileName = "Prefabs/Treasure/ScoreTreasureBracelet";
				break;
			case eTreasureType.Crown:
				fileName = "Prefabs/Treasure/ScoreTreasureCrown";
				break;
			}
			var tresure = Instantiate(Resources.Load(fileName)) as GameObject;

			m_scoreDatas.Add(new ScoreData(m_treasures[i], tresure));
			tresure.transform.parent = m_scoreParent.transform;
			Vector3 pos = Vector3.zero;
			pos.z += -2.0f * i;
			tresure.transform.localPosition = pos;
			tresure.transform.Rotate(0.0f, 90.0f, 0.0f);
		}
	}
	public enum Mode
	{
		Opaque,
		Cutout,
		Fade,
		Transparent,
	}
	/// <summary>
	/// 移動処理
	/// </summary>
	private void Move()
	{
		Vector3 vector = SceneMain.GetInput(m_initData.playerNo);
		Vector3 pos = transform.position;
		eAnimState animState = eAnimState.Stop;

		if (vector.magnitude > m_runThreshold) {
			animState = eAnimState.Run;
		} else if (vector.magnitude < 0.1f) {
			animState = eAnimState.Stop;
		} else {
			animState = eAnimState.Walk;
		}
		m_rigidbody.velocity = vector * m_speed;

		// 移動した方向を向く
		if (m_rigidbody.velocity.magnitude > 0.01f) //ベクトルの長さが0.01fより大きい場合にプレイヤーの向きを変える処理を入れる(0では入れないので）
		{
			phantom.transform.rotation = Quaternion.LookRotation(m_rigidbody.velocity);  //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
		}
		// アニメーション更新
		if (m_animState != animState) {
			switch(animState) {
			case eAnimState.Stop:

				m_animator.SetTrigger("idle");
				break;
			case eAnimState.Walk:
				m_animator.SetTrigger("walk");
				break;
			case eAnimState.Run:
				m_animator.SetTrigger("run");
				// エフェクト
				if (m_runEffect == null) {
					m_runEffect = Instantiate(Resources.Load("Prefabs/Effect/NoiseParticel")) as GameObject;
					m_runEffect.transform.parent = transform;
					m_runEffect.transform.localPosition = Vector3.zero;
					var particle = m_runEffect.GetComponent<ParticleSystem>();
					var color = particle.colorOverLifetime;
					switch (m_phantomNo) {
					case 0:
						color.color = Color.red;
						break;
					case 1:
						color.color = Color.blue;
						break;
					case 2:
						color.color = Color.green;
						break;
					}
				}
				break;
			}
			m_animState = animState;
		}

		// 透過処理
		{
			Color color;
			// 騒音状態更新
			if (m_animState == eAnimState.Run) {
				m_appearTime = AppearTime;
			}
			if (m_appearTime > 0.0f) {                          // 騒音状態
				m_appearTime -= Time.deltaTime;
				if (m_appearTime <= 0.0f) {
					if (m_runEffect != null) {
						GameObject.Destroy(m_runEffect);
						m_runEffect = null;
					}
				}
				m_renderer.material = m_normalMat;
				color = m_renderer.material.color;
				color.a = 1.0f;
				m_renderer.material.color = color;
				m_textMeshObj.SetActive(true);
			} else if (vector.magnitude < m_visibleThreshold) { // 完全透過状態
				m_renderer.material = m_fadeMat;
				color = m_renderer.material.color;
				color.a = 0.0f;
				m_renderer.material.color = color;
				m_textMeshObj.SetActive(false);
			} else {                                            // 歩き
				m_renderer.material = m_fadeMat;
				color = m_renderer.material.color;
				color.a = Mathf.Lerp(0.0f, 1.0f, (vector.magnitude - m_visibleThreshold) / (m_runThreshold - m_visibleThreshold));
				m_renderer.material.color = color;
				m_textMeshObj.SetActive(false);
			}
		}
	}
}
