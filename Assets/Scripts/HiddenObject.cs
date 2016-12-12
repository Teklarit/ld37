using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
	public Color mainColor;
	public Color attentionColor;
	public Transform particle;
	private Transform _particle;
	private MeshRenderer renderer;
	// Use this for initialization
	void Start ()
	{
		gameObject.layer = LayerMask.NameToLayer("Interactible");
		if (!GetComponent<BoxCollider>())	gameObject.AddComponent<BoxCollider>();
		renderer = GetComponentInChildren<MeshRenderer>();
		renderer.material.color = mainColor;
		_particle = Instantiate(particle);
		_particle.parent = gameObject.transform;
		_particle.localPosition = Vector3.zero;
		_particle.localScale = _particle.parent.lossyScale;
		var main = _particle.GetComponent<ParticleSystem>().main;
		var startColor = main.startColor;
		startColor.mode = ParticleSystemGradientMode.Color;
		startColor.color = renderer.material.color;
		main.startColor = startColor;
		var shape = _particle.GetComponent<ParticleSystem>().shape;
		shape.enabled = true;
		shape.shapeType = ParticleSystemShapeType.Mesh;
		shape.mesh = GetComponentInChildren<MeshFilter>().mesh;

	}

	private bool _attention;
	public bool Attention
	{
		set
		{
			_attention = value;
			renderer.material.color = value ? attentionColor : mainColor;
			var main = _particle.GetComponent<ParticleSystem>().main;
			var startColor = main.startColor;
			startColor.color = renderer.material.color;
			main.startColor = startColor;
		}
		get { return _attention; }
	}
}
