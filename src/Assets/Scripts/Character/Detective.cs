using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 探偵
public class Detective : MonoBehaviour
{
    private float m_speed = 15;
	private GameObject m_textMeshObj;
	private GameObject detective;
	Rigidbody rigidbody;
	public SceneMain.InitData m_initData;
	AudioSource m_audio;
	public AudioSource walkSEObj;
	private SceneMain m_sceneMain;
	public void Init(SceneMain.InitData initData)
	{
		m_initData = initData;
	}
	// Start is called before the first frame update
	void Start()
    {
		detective = transform.Find("detective/detective").gameObject;
		rigidbody = GetComponent<Rigidbody>();
		m_textMeshObj = transform.Find("TextMesh").gameObject;
		m_textMeshObj.GetComponent<TextMesh>().text = string.Format("{0}P", m_initData.playerNo+1);
		// 当たり判定関数を登録
		var hit = transform.Find("Hit").gameObject.GetComponent<HitClass>().hitCallback = Hit;
		m_audio = this.GetComponent<AudioSource>();
		m_sceneMain = GameObject.Find("SceneMain").GetComponent<SceneMain>();
	}

    // Update is called once per frame
    void Update()
	{
		if (m_sceneMain.IsGamePlay() == false) { return; }
		Vector3 vector = SceneMain.GetInput(m_initData.playerNo);

		rigidbody.velocity = vector * m_speed;

		// 歩行音を鳴らす
		if(rigidbody.velocity.sqrMagnitude > 0F)
		{
			walkSEObj.enabled = true;
		}
		else
		{
			walkSEObj.enabled = false;
		}

		// 移動した方向を向く
		if (rigidbody.velocity.magnitude > 0.01f) //ベクトルの長さが0.01fより大きい場合にプレイヤーの向きを変える処理を入れる(0では入れないので）
		{
			detective.transform.rotation = Quaternion.LookRotation(rigidbody.velocity);  //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
		}
	}
    void Hit(Collider other) 
	{
    }
}
