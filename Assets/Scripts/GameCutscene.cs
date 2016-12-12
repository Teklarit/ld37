using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCutscene : MonoBehaviour
{
	public Image Fader;
	public GameObject Player;
	public Camera sittingCamera;
	public float StartTalkDelay = 1.0f;
	public float DramaticBeforeHitDelay =1.0f;
	public float DramaticAfterHitDelay = 1.0f;
	public float BeforeStopmotionDelay;
	public Transform[] Animators;
	public Transform[] CameraStartTransitions;
	public Transform[] CameraWinTransitions;
	public Transform[] CameraLoseTransitions;
	public float[] CameraStartFovs;
	void Start ()
	{
		//_animator = Burglar.GetComponent<Animator>();
		StartCoroutine(DoStartCutscene());
	}

	//public void TriggerWinSequenc()
	//{
	//	StartCoroutine(DoWinCutscene());
	//}
	//public void TriggerLoseSequenc()
	//{
	//	StartCoroutine(DoLoseCutscene());
	//}

	//IEnumerator DoWinCutscene()
	//{
	//	yield return;
	//}

	//IEnumerator DoLoseCutscene()
	//{
	//}

	IEnumerator ChangeFov()
	{
		var time = 0.0f;
		while(time < DramaticBeforeHitDelay)
		{
			sittingCamera.fieldOfView = Mathf.Clamp(sittingCamera.fieldOfView + 2 * Time.deltaTime, 50, 120);
			time += Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}

	IEnumerator MoveCamera(Transform[] transitions,float[] fovs, float wholeTime)
	{
		for (int i = 1; i < transitions.Length; i++)
		{
			print(i);
			var time = 0.0f;
			while (time < wholeTime/transitions.Length)
			{
				sittingCamera.transform.position = Vector3.Lerp(sittingCamera.transform.position,transitions[i].position, 2 * Time.deltaTime);
				sittingCamera.transform.rotation = Quaternion.Lerp(sittingCamera.transform.rotation, transitions[i].rotation, 2 * Time.deltaTime);
				sittingCamera.fieldOfView = Mathf.Lerp(sittingCamera.fieldOfView, CameraStartFovs[i], 2 * Time.deltaTime);
				time += Time.deltaTime;
				yield return new WaitForSeconds(Time.deltaTime);
			}
		}
	}

	IEnumerator SleepWell()
	{
		var time = BeforeStopmotionDelay * 0.9f;
		yield return new WaitForSeconds(time);
		while (time < BeforeStopmotionDelay)
		{
			Fader.color = new Color(0, 0, 0, 3*time / BeforeStopmotionDelay);
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
	IEnumerator DoStartCutscene()
	{
		sittingCamera.transform.position = CameraStartTransitions[0].position;
		sittingCamera.transform.rotation = CameraStartTransitions[0].rotation;
		sittingCamera.fieldOfView = CameraStartFovs[0];

		yield return new WaitForSeconds(1.0f);
		StartCoroutine(MoveCamera(CameraStartTransitions, CameraStartFovs, DramaticBeforeHitDelay));
		yield return new WaitForSeconds(DramaticBeforeHitDelay);
		Animators[1].GetComponent<Animator>().SetBool("Punch",true);
		StartCoroutine(ChangeFov());
		StartCoroutine(SleepWell());
		yield return new WaitForSeconds(BeforeStopmotionDelay);
		foreach (var anim in Animators)
		{
			anim.GetComponent<Animator>().speed = 0;
		}
		yield return new WaitForSeconds(DramaticAfterHitDelay);
		Player.SetActive(true);
		sittingCamera.enabled = false;
		Fader.CrossFadeAlpha(0.0f,1.5f,true);
	}
}
