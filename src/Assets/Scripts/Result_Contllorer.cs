using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result_Contllorer : MonoBehaviour
{
    public GameObject[] phantom_riz;
    public GameObject detective_riz;
    public GameObject[] charapos;//キャラクターのポジション
    public GameObject cube;//デバッグ用
    GameObject fadeImage;//フェードのイメージ
    GameObject SinbunImage;//新聞のイメージ
    GameObject RankPanel;//ランキング表
    public GameObject toBeBackText;
    public Sprite[] sinbun;//新聞のスプライト
    float alfa = 1.0f;
    float a = 1.0f;
    float time_a = 1.0f;
    bool back_on = false;
    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GameObject.Find("FadePanel");
        SinbunImage = GameObject.Find("SinbunImage");
        RankPanel = GameObject.Find("RankPanel");
        //toBeBackText = GameObject.Find("ToBeBackText");
        Invoke("RankAnime", 3.2f);
        Invoke("ToBeBackText", 3.8f);
    }

    // Update is called once per frame
    void Update()
    {
        //フェード処理
        fadeImage.GetComponent<Image>().color = new Color(0,0,0,alfa);
        alfa -= Time.deltaTime / 5;
        //どっちの新聞紙を表示するか？0か1で切り替えられる
        SinbunImage.GetComponent<Image>().sprite = sinbun[1];
        //タイトルにバックできる
        if(back_on){
            toBeBackText.GetComponent<Text>().color = new Color(0,0,0,a);
            a = Mathf.Sin(time_a += Time.deltaTime * 4);

            //シーンを移動
            if(Input.GetAxis("Vertical") < 0)
            SceneManager.LoadScene("TitleScene");
        }
    }

    
    //真ん中にRankをアニメーション
    void RankAnime(){
        RankPanel.GetComponent<Animator>().enabled = true;
        //ランキングを検出
        for(int i = 0; i < 4;i++){
            //1位
            if(SceneResult.initDatas[i].rank == 1){
                Rank1(i);
            }
            //2位
            else if(SceneResult.initDatas[i].rank == 2){
                Rank2(i);
            }
            //3位
            else if(SceneResult.initDatas[i].rank == 3){
                Rank3(i);
            }
            //4位
            else{
                Rank4(i);
            }
        }
    }

    //1位のランキングの処理
    void Rank1(int rank){
        //探偵
        if((int)SceneResult.initDatas[rank].charaId == 1){
            GameObject obj = Instantiate(cube, charapos[0].transform.position, Quaternion.identity);
            obj.transform.parent = charapos[0].transform.parent;
        }
        //怪盗1
        if((int)SceneResult.initDatas[rank].charaId == 2){
            GameObject obj = Instantiate(cube, charapos[0].transform.position, Quaternion.identity);
            obj.transform.parent = charapos[0].transform.parent;
        }
        //怪盗2
        if((int)SceneResult.initDatas[rank].charaId == 3){
            GameObject obj = Instantiate(cube, charapos[0].transform.position, Quaternion.identity);
            obj.transform.parent = charapos[0].transform.parent;
        }
        //怪盗3
        if((int)SceneResult.initDatas[rank].charaId == 4){
            GameObject obj = Instantiate(cube, charapos[0].transform.position, Quaternion.identity);
            obj.transform.parent = charapos[0].transform.parent;
        }
        //
    }

    //1位のランキングの処理
    void Rank2(int rank){
        
    }

    //1位のランキングの処理
    void Rank3(int rank){
        
    }

    //1位のランキングの処理
    void Rank4(int rank){
        
    }

    //ToBeBackTextを表示させ、タイトルに戻れるようにする
    void ToBeBackText(){
        back_on = true;
        toBeBackText.SetActive(true);
    }
}
