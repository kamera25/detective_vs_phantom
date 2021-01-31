using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

enum Character
{
    none = 0,
    detective,
    phantom, 
}

public class Title_Contolloer : MonoBehaviour
{
    public Text countText;
    public GameObject detective;//探偵のオブジェ
    public GameObject[] phantom;//怪盗のオブジェ×３
    public SceneTitle title;
    float timer = 4.0f;//全員が選択して◯秒後に決定
    public AudioClip selectSE;
    AudioSource selectAudioSource;
    public GameObject[] textMexh;

    List<Character> key; // キャラクター選択情報・Enum型
    List<int> phantom_select = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        selectAudioSource = this.GetComponent<AudioSource>();

        //それぞれのPlayerが入力しているか確認する変数
        key = new List<Character>() { Character.none, Character.none, Character.none, Character.none} ;
    }

	int[] axis = { 0, 0, 0, 0 };
    // Update is called once per frame
    void Update()
	{
		bool[] axisChange = { false, false, false, false };
		var Threshold = 0.5f;
		// スティック情報の前回値を取得
		for (int i = 0; i < 4; i++) {
			int now = 0;
			if(Input.GetAxis(string.Format("GamePad{0}_X", i)) <= -Threshold){
				now = -1;
			}
			if (Input.GetAxis(string.Format("GamePad{0}_X", i)) >= Threshold) {
				now = 1;
			}
			if(now != axis[i]) {
				axisChange[i] = true;
			}
			axis[i] = now;
		}

		/****************************
        スティックを倒して選択する処理
        *****************************/
		CheckKeyOnTitleScene( KeyCode.LeftArrow, KeyCode.RightArrow, 0, Threshold);
        CheckKeyOnTitleScene( KeyCode.A, KeyCode.D, 1, Threshold);
        CheckKeyOnTitleScene( KeyCode.F, KeyCode.H, 2, Threshold);
        CheckKeyOnTitleScene( KeyCode.J, KeyCode.L, 3, Threshold);
		//怪盗の選択をListにAddさせたりRemoveしたりする
		if ((Input.GetKeyDown(KeyCode.RightArrow) || (axisChange[0] && axis[0]==1)) && phantom_select.Count <= 2){
            phantom_select.Add(0);
        }
        if((Input.GetKeyUp(KeyCode.RightArrow) || (axisChange[0] && axis[0] != 1))) {
            phantom_select.Remove(0);
        }
        if((Input.GetKeyDown(KeyCode.D) || (axisChange[1] && axis[1] == 1)) && phantom_select.Count <= 2){
			phantom_select.Add(1);
        }
        if(Input.GetKeyUp(KeyCode.D) || (axisChange[1] && axis[1] != 1)) {
            phantom_select.Remove(1);
        }
        if((Input.GetKeyDown(KeyCode.H) || (axisChange[2] && axis[2] == 1)) && phantom_select.Count <= 2){
            phantom_select.Add(2);
        }
        if(Input.GetKeyUp(KeyCode.H) || (axisChange[2] && axis[2] != 1)) {
            phantom_select.Remove(2);
        }
        if((Input.GetKeyDown(KeyCode.L) || (axisChange[3] && axis[3] == 1)) && phantom_select.Count <= 2){
            phantom_select.Add(3);
        }
        if(Input.GetKeyUp(KeyCode.L) || (axisChange[3] && axis[3] != 1)) {
            phantom_select.Remove(3);
        }

        //textMexh[0].GetComponent<TextMesh>().text = "P" + phantom_select[0];

        for(int i = 0; i < phantom_select.Count; i++)
        {
            textMexh[i].GetComponent<TextMesh>().text = "P" + (phantom_select[i] + 1);
            //Debug.Log(phantom_select[i]);
        }
        //phantom_select[0]=最初に選択したPlayerの数字が0からスタートしている
        //phantom_select[1]=2番目に選択したPlayer
        //phantom_select[0]=3番目に選択したPlayer
        //Playerは0スタート
      
        

        // 探偵を利用選択している人がいるか確認する。
        bool _isDetectiveVisible = IsSelectedDetective();
        detective.SetActive(_isDetectiveVisible);

        //怪盗を利用し選択している人がいるか確認する。
        for(int i = 0; i < key.Count( _val => _val == Character.phantom); i++){
            int a = key.Count( _val => _val == Character.phantom);
            phantom[i].SetActive(true);
        }
        for(int j = key.Count( _val => _val == Character.phantom); j < 3; j++){
            phantom[j].SetActive(false);
        }

        //条件クリアでスタート
        bool _isStart = CanStart();
        if( _isStart)
        {
            timer -= Time.deltaTime;
			if (timer > 3) {
				countText.text = "";
			}
			else if (timer > 2){
                countText.text = "3";
            }
            else if(timer > 1){
                countText.text = "2";
            }
            else if(timer > 0){
                countText.text = "1";
            }
            //決定
            if(timer <= 0)
            {
                countText.text = "0";
				FadeManager.FadeOut();
				Invoke("mainScene",2);
                enabled = false;
            }       
        }else{
            timer = 4.0f;
            countText.text = "";
        }
        //Debug.Log( key.Count( _val => _val == Character.detective) + " / " + key.Count( _val => _val == Character.phantom));
        
    }
    //MainSeaneに移動
    void mainScene()
	{
		SceneMain.initDatas = new List<SceneMain.InitData>();
		SceneMain.initDatas.Add(new SceneMain.InitData(SceneMain.InitData.eCharaId.Detective, key.IndexOf(Character.detective)));
		if (phantom_select.Count >= 1) {
			SceneMain.initDatas.Add(new SceneMain.InitData(SceneMain.InitData.eCharaId.Phantom0, phantom_select[0]));
		}
		if (phantom_select.Count >= 2) {
			SceneMain.initDatas.Add(new SceneMain.InitData(SceneMain.InitData.eCharaId.Phantom1, phantom_select[1]));
		}
		if (phantom_select.Count >= 3) {
			SceneMain.initDatas.Add(new SceneMain.InitData(SceneMain.InitData.eCharaId.Phantom2, phantom_select[2]));
		}
		SceneManager.LoadScene("MainScene");
	}


    // PlayerNum番目のプレイヤーがキーを押しているか確認
    void CheckKeyOnTitleScene( KeyCode _left, KeyCode _right, int _playerNum, float threshold)
    {
        Character _selectedChara = key[_playerNum];
        string _debugStr = "P" + _playerNum;
		var axis = Input.GetAxis(string.Format("GamePad{0}_X", _playerNum));

		//Player--(探偵選択)
		if (Input.GetKey(_left) || axis <= -threshold)
        {
            // 自分が探偵なら(左に倒し続けている)
            if( _selectedChara == Character.detective)
            {
                _debugStr += "L";
            }
            // 今は探偵じゃないけど、誰も探偵に手挙げしてないなら
            else if( !IsSelectedDetective())
            {
                _selectedChara = Character.detective;
                selectAudioSource.PlayOneShot(selectSE);
                _debugStr += "L";
            }
            // もう探偵が居るので、君は探偵になれないよ
            else
            {
                _selectedChara = Character.none;
                _debugStr += "N";
            }
        }         
        //Player1--(怪盗選択)
        else if( Input.GetKey(_right) || axis >= threshold)
        {
            // 自分が怪盗なら(前から右に倒し続けている)
            if( _selectedChara == Character.phantom )
            {
                _debugStr += "R";
            }
            // 今は怪盗じゃないけど、怪盗になっても良い条件を満たしていれば
            else if( CheckCanSelectPhantom())
            {
                _selectedChara = Character.phantom;
                selectAudioSource.PlayOneShot(selectSE);
                _debugStr += "R";
                //phantom_select.Add(_playerNum);//追加
            }
            // 怪盗が多すぎるので、君は怪盗になれないよ
            else
            {
                _selectedChara = Character.none;
                _debugStr += "N";
            }
        }
        //Player1--(選択していない)
        else
        {
            _selectedChara = Character.none;
            _debugStr += "N";
        }

        // 配列に入れる
        key[_playerNum] = _selectedChara;
        // キー入力情報
        //Debug.Log (_debugStr);
    }

    // 誰かが探偵を選んでいるか
    bool IsSelectedDetective()
    {
        return key.Contains(Character.detective);
    }

    // 4人のプレイヤーが選択したか
    bool IsSelected4Player()
    {
        return !key.Contains(Character.none);
    }

    // 3人の怪盗が居るか
    bool CheckCanSelectPhantom()
    {
        return key.Count( _val => _val == Character.phantom) <= 2;
	}
	// 開始できる？
	bool CanStart()
	{
		return key.Contains(Character.phantom) && key.Contains(Character.detective);
	}

}