using UnityEngine;
using UnityEngine.Rendering;

namespace NoiseBall
{
    [ExecuteInEditMode]
    public class NoiseBallRendererFixed : MonoBehaviour
    {
        #region Exposed Parameters

        [SerializeField]
        Mesh _mesh;

        [Space]
        [SerializeField]
        float _radius = 1.0f;

        [SerializeField]
        float _noiseAmplitude = 0.05f;

        [SerializeField]
        float _noiseFrequency = 1.0f;

        [SerializeField]
        float _noiseMotion = 0.2f;

        [Space]
       [SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)]
		Color _surfaceColor = Color.white;

        [SerializeField, Range(0, 1)]
        float _metallic = 0.5f;

        [SerializeField, Range(0, 1)]
        float _smoothness = 0.5f;

        [Space]
        [SerializeField]
        ShadowCastingMode _castShadows;

        [SerializeField]
        bool _receiveShadows;

        #endregion

        #region Private Resources

        [SerializeField, HideInInspector]
        Shader _surfaceShader;

        #endregion

        #region Private Variables

        Material _surfaceMaterial;
        MaterialPropertyBlock _materialProperties;
        Vector3 _noiseOffset;

        #endregion

        #region MonoBehaviour Functions

        void Update()
        {
            if (_surfaceMaterial == null)
            {
                _surfaceMaterial = new Material(_surfaceShader);
                _surfaceMaterial.hideFlags = HideFlags.DontSave;
            }
			
            if (_materialProperties == null)
                _materialProperties = new MaterialPropertyBlock();

            _noiseOffset += new Vector3(0.13f, 0.82f, 0.11f) * _noiseMotion * Time.deltaTime;

            _surfaceMaterial.color = _surfaceColor;

            _surfaceMaterial.SetFloat("_Metallic", _metallic);
            _surfaceMaterial.SetFloat("_Glossiness", _smoothness);
            _surfaceMaterial.SetFloat("_Radius", _radius);

            _materialProperties.SetFloat("_NoiseAmplitude", _noiseAmplitude);
            _materialProperties.SetFloat("_NoiseFrequency", _noiseFrequency);
            _materialProperties.SetVector("_NoiseOffset", _noiseOffset);

            Graphics.DrawMesh(
                _mesh, transform.localToWorldMatrix,
                _surfaceMaterial, 0, null, 0, _materialProperties,
                _castShadows, _receiveShadows, transform
            );
			
        }

        #endregion
    }
}
