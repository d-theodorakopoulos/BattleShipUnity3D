using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageControl : MonoBehaviour 
{
	public GameObject label;
	public GameObject warningMessageLabel;
	public float smoothFactor = 1;
	private Text labelTextScript;
	private Text warningMessageTextScript;

	void Awake()
	{
		labelTextScript = label.GetComponent<Text>(); 
		warningMessageTextScript = warningMessageLabel.GetComponent<Text>();
	}
	void Start()
	{
		IEnumerator coroutine = ShowWarningMessageCoroutine("Deploy your ships!");
		StartCoroutine(coroutine);
	}

	public IEnumerator ShowTileName(string name)
	{
		labelTextScript.enabled = true;
		labelTextScript.color = Color.black;	
		labelTextScript.text = name;

		yield return new WaitForSeconds(1f);
		while(labelTextScript.color.a >= 0.1)
		{
			yield return null;
			labelTextScript.color = Color.Lerp(labelTextScript.color, Color.clear ,Time.fixedDeltaTime);
			//Debug.Log(Time.deltaTime + " " + LabelTextScript.color);
		}
		yield return null;
		labelTextScript.enabled = false;
	}
	public void showWarningMessage(string message)
	{
		StopCoroutine("ShowWarningMessageCoroutine");
		StartCoroutine("ShowWarningMessageCoroutine",message);
	}
	private IEnumerator ShowWarningMessageCoroutine(string message)
	{
		warningMessageTextScript.enabled = true;
		warningMessageTextScript.color = Color.black;
		warningMessageTextScript.text = message;
		
		yield return new WaitForSeconds(2f);
		while(warningMessageTextScript.color.a >= 0.1)
		{
			yield return null;
			warningMessageTextScript.color = Color.Lerp(warningMessageTextScript.color, Color.clear ,Time.fixedDeltaTime * smoothFactor);
			//Debug.Log(Time.deltaTime + " " + warningMessageTextScript.color);
		}
		yield return null;
		warningMessageTextScript.enabled = false;
	}
}
