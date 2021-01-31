using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
	static FadeManager m_instance;
	public static FadeManager Instance { get { return m_instance; } }

	public GameObject m_tipsObj;
	enum ePhase
	{
		In,
		FadeIn,
		Out,
		FadeOut,
	};
	ePhase phase = ePhase.In;
	float m_fadeTime = 1.0f;
	float m_timer;
	Image m_image;
	Image m_tipsImage;

	// Start is called before the first frame update
	void Start()
    {
		DontDestroyOnLoad(gameObject);
		m_instance = this;
		m_image = GameObject.Find("FadeImage").GetComponent<Image>();
		m_tipsImage = m_tipsObj.GetComponent<Image>();
	}

    // Update is called once per frame
    void Update()
	{
		if (phase == ePhase.FadeIn || phase == ePhase.FadeOut) {
			Color colorOut = Color.black;
			Color colorIn = Color.black;
			Color color = Color.black;
			colorIn.a = 0.0f;
			switch (phase) {
			case ePhase.FadeIn:
				m_timer -= Time.deltaTime;
				if (m_timer <= 0.0f) {
					color.a = 0.0f;
					phase = ePhase.In;
				} else {
					color = Color.Lerp(colorIn, colorOut, m_timer / m_fadeTime);
				}
				Debug.Log(color.a);
				m_image.color = color;
				break;
			case ePhase.FadeOut:
				m_timer -= Time.deltaTime;
				if (m_timer <= 0.0f) {
					color.a = 1.0f;
					phase = ePhase.Out;
				} else {
					color = Color.Lerp(colorOut, colorIn, m_timer / m_fadeTime);
				}
				m_image.color = color;
				break;
			}
		}
    }
	public static void FadeIn()
	{
		if(m_instance==null) { return; }
		m_instance.phase = ePhase.FadeIn;
		m_instance.m_timer = m_instance.m_fadeTime;
	}
	public static void FadeOut()
	{
		if (m_instance == null) { return; }
		m_instance.phase = ePhase.FadeOut;
		m_instance.m_timer = m_instance.m_fadeTime;
	}
	public static bool IsFadeEnd()
	{
		if (m_instance == null) { return true; }
		return m_instance.phase == ePhase.In || m_instance.phase == ePhase.Out;
	}
	public static void TipsOn()
	{
		if (m_instance == null) { return; }
		m_instance.m_tipsImage.gameObject.SetActive(true);
	}
	public static void TipsOff()
	{
		if (m_instance == null) { return; }
		m_instance.m_tipsImage.gameObject.SetActive(false);
	}
}
