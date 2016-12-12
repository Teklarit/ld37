using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCutscene : MonoBehaviour
{
	public Image Fader;
	public Transform Burglar;
	public GameObject Player;
	public Camera sittingCamera;
	public Transform[] GoTo;
	public float DramaticBeforeHitDelay;
	public float DramaticAfterHitDelay = 1.0f;
	public float BeforeStopmotionDelay;
	private Animator _animator;
	void Start ()
	{
		_animator = Burglar.GetComponent<Animator>();
		StartCoroutine(DoCutscene());
	}

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

	IEnumerator SleepWell()
	{
		var time = BeforeStopmotionDelay * 0.8f;
		yield return new WaitForSeconds(time);
		while (time < BeforeStopmotionDelay)
		{
			Fader.color = new Color(0, 0, 0, 2*time / BeforeStopmotionDelay);
			yield return new WaitForSeconds(Time.deltaTime);
		}
	}
	IEnumerator DoCutscene()
	{
		for (int i = 0; i < GoTo.Length; i++)
		{
			var distance = Vector3.Distance(Burglar.position, GoTo[i].position);
			while (distance > 0.5f)
			{
				Burglar.rotation = Quaternion.Lerp(Burglar.rotation, Quaternion.LookRotation(GoTo[i].position - Burglar.position),
					Time.deltaTime*10);
				_animator.SetFloat("VSpeed", Mathf.Clamp(distance*2, 0, 1));
				distance = Vector3.Distance(Burglar.position, GoTo[i].position);
				yield return new WaitForSeconds(Time.deltaTime);
			}
		}
		_animator.SetFloat("VSpeed",0);
		StartCoroutine(ChangeFov());
		yield return new WaitForSeconds(DramaticBeforeHitDelay);
		_animator.SetBool("Punching",true);
		StartCoroutine(SleepWell());
		yield return new WaitForSeconds(BeforeStopmotionDelay);
		_animator.speed = 0;
		yield return new WaitForSeconds(DramaticAfterHitDelay);
		Player.SetActive(true);
		sittingCamera.enabled = false;
		Fader.CrossFadeAlpha(0.0f,1.5f,true);
	}
}
