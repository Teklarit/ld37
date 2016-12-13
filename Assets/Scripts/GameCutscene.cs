using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCutscene : MonoBehaviour
{
	public Image Fader;
	public Image Fader2;
	public Image Fader3;
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
	public float[] CameraWinFovs;
	public float[] CameraLoseFovs;
	public float DramaticWinCameraSequenceTime = 2.0f;
	public float DramaticLoseCameraSequenceTime = 4.0f;
	public GameObject staticHeroModel;
	public GameObject dynamicHeroModel;
	public GameObject Knife;
	public GameObject KnifeLose;
	public Transform EndCamera;

    public AudioSource audio1;
    public AudioClip audocl;

	public UiMessage UIMessage;

	private int endSecretAgent;

    void Start ()
	{
		//_animator = Burglar.GetComponent<Animator>();
		StartCoroutine(DoStartCutscene());
	}

	public void TriggerWinSequence()
	{
		StartCoroutine(DoWinCutscene());
	}
	public void TriggerLoseSequence(int secretAgent) {
		endSecretAgent = secretAgent;
		StartCoroutine(DoLoseCutscene());
	}

	IEnumerator DoWinCutscene()
	{
		Fader2.color = new Color(0, 0, 0, 1.0f);
		foreach (var anim in Animators)
		{
			var animator = anim.GetComponent<Animator>();
			animator.enabled = true;
			animator.StartPlayback();
			animator.speed = 1;
			animator.SetBool("WakeUp", true);
		}
		yield return new WaitForSeconds(1.0f);
		sittingCamera.transform.position = CameraWinTransitions[0].position;
		sittingCamera.transform.rotation = CameraWinTransitions[0].rotation;
		sittingCamera.fieldOfView = CameraWinFovs[0];
		Knife.SetActive(true);
		Player.SetActive(false);
		//sittingCamera.enabled = true;
		sittingCamera.gameObject.SetActive(true);
		Fader2.CrossFadeAlpha(0,1.0f,true);
		StartCoroutine(MoveCamera(CameraWinTransitions, CameraWinFovs, DramaticWinCameraSequenceTime));
		yield return new WaitForSeconds(DramaticWinCameraSequenceTime);
		StartCoroutine(ChangeFov());
		yield return new WaitForSeconds(0.75f);
		sittingCamera.transform.position = EndCamera.transform.position;
		sittingCamera.transform.rotation = EndCamera.transform.rotation;
		staticHeroModel.SetActive(false);
		dynamicHeroModel.SetActive(true);
		dynamicHeroModel.GetComponent<Animator>().SetBool("Attack", true);
		yield return new WaitForSeconds(0.65f);
		foreach (var anim in Animators)
		{
			var animator = anim.GetComponent<Animator>();
			animator.SetBool("Attack", true);
		}
		Fader2.color = new Color(0, 0, 0, 1.0f);
		yield return new WaitForSeconds(5.0f);
		print("Game Over");
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		Cursor.lockState = CursorLockMode.Confined;
		yield return null;
	}

	IEnumerator DoLoseCutscene()
	{
		Fader2.color = new Color(0, 0, 0, 1.0f);
		Player.SetActive(false);
		sittingCamera.transform.position = CameraLoseTransitions[0].position;
		sittingCamera.transform.rotation = CameraLoseTransitions[0].rotation;
		sittingCamera.fieldOfView = CameraLoseFovs[0];
		sittingCamera.gameObject.SetActive(true);
		//sittingCamera.enabled = true;
		Fader2.CrossFadeAlpha(0,1.0f,true);
		yield return new WaitForSeconds(1.0f);
		//staticHeroModel.SetActive(false);
		//dynamicHeroModel.SetActive(true);
		foreach (var anim in Animators)
		{
			var animator = anim.GetComponent<Animator>();
			animator.enabled = true;
			animator.StartPlayback();
			animator.speed = 1;
			animator.SetBool("WakeUp", true);
		}
		yield return new WaitForSeconds(2.0f);
		StartCoroutine(MoveCamera(CameraLoseTransitions, CameraLoseFovs, DramaticLoseCameraSequenceTime));
		yield return new WaitForSeconds(DramaticLoseCameraSequenceTime*0.8f);
		foreach (var anim in Animators)
		{
			var animator = anim.GetComponent<Animator>();
			animator.SetBool("Punch", true);
		}
		Fader3.color = new Color(0, 0, 0, 1.0f);
		KnifeLose.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		Fader3.CrossFadeAlpha(0,0.5f,true);
		yield return new WaitForSeconds(1.0f);
		UIMessage.show_message("No, this is not how it went. Ah, I remember now... \n(" + endSecretAgent + "/7 key memories found)");
		UIMessage.gameObject.SetActive(true);
		yield return new WaitForSeconds(10.0f);
		print("Game Over");
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		Cursor.lockState = CursorLockMode.Confined;
		yield return null;
	}

	IEnumerator ChangeFov()
	{
		var time = 0.0f;
		while(time < DramaticBeforeHitDelay)
		{
			sittingCamera.fieldOfView = Mathf.Clamp(sittingCamera.fieldOfView + 3 * Time.deltaTime, 50, 120);
			time += Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return null;
	}

	IEnumerator MoveCamera(Transform[] transitions,float[] fovs, float wholeTime)
	{
		for (int i = 1; i < transitions.Length; i++)
		{
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
		yield return null;
	}

	IEnumerator SleepWell()
	{
		var time = BeforeStopmotionDelay * 0.9f;
		yield return new WaitForSeconds(time);
		while (time < BeforeStopmotionDelay)
		{
			Fader.color = new Color(0, 0, 0, Mathf.Clamp(3*time / BeforeStopmotionDelay,0,1.0f));
			time += Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return null;
	}
	IEnumerator DoStartCutscene()
	{
        // 
        audio1.PlayOneShot(audocl, 1.0f);

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
		sittingCamera.gameObject.SetActive(false);
		Player.SetActive(true);
		Fader.CrossFadeAlpha(0.0f,1.5f,true);
		yield return null;
	}

	//void Update()
	//{
	//	print(Fader.color.a);
	//}
}
