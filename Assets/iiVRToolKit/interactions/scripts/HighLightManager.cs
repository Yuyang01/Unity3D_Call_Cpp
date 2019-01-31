using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightManager : MonoBehaviour
{
    public Color _outlineColor = Color.green;
    float _thicknessValue = 0.0f;
    float _thicknessMax = 0.25f;
    float _thicknessMin = 0.1f;
    float _speedHighLight = 1.0f;

    List<Renderer> _currentRenderers = new List<Renderer>();

    bool _highLighting = false;

    // Use this for initialization
    void Start ()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend)
            _currentRenderers.Add(rend);

        _currentRenderers.AddRange(GetComponentsInChildren<Renderer>());
        
        _highLighting = false;
        _thicknessValue = 0.0f;

        for (int i = 0; i < _currentRenderers.Count; i++)
        {
            _currentRenderers[i].material.SetColor("_OutlineColor", _outlineColor);
            _currentRenderers[i].material.SetFloat("_Thickness", _thicknessValue);

            for (int j = 0; j < _currentRenderers[i].materials.Length; j++)
            {
                _currentRenderers[i].materials[j].SetColor("_OutlineColor", _outlineColor);
                _currentRenderers[i].materials[j].SetFloat("_Thickness", _thicknessValue);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_highLighting)
        {
            _thicknessValue += _speedHighLight * Time.deltaTime;
            if (_thicknessValue > _thicknessMax)
            {
                _thicknessValue = _thicknessMax;
                _speedHighLight *= -1.0f;
            }

            if (_thicknessValue < _thicknessMin)
            {
                _thicknessValue = _thicknessMin;
                _speedHighLight *= -1.0f;
            }

            for (int i = 0; i < _currentRenderers.Count; i++)
            {
                _currentRenderers[i].material.SetColor("_OutlineColor", _outlineColor);
                _currentRenderers[i].material.SetFloat("_Thickness", _thicknessValue);

                for (int j = 0; j < _currentRenderers[i].materials.Length; j++)
                {
                    _currentRenderers[i].materials[j].SetColor("_OutlineColor", _outlineColor);
                    _currentRenderers[i].materials[j].SetFloat("_Thickness", _thicknessValue);
                }
            }

            for (int i = 0; i < _currentRenderers.Count; i++)
            {
                _currentRenderers[i].material.SetFloat("_Thickness", _thicknessValue);

                for (int j = 0; j < _currentRenderers[i].materials.Length; j++)
                {
                    _currentRenderers[i].materials[j].SetFloat("_Thickness", _thicknessValue);
                }
            }
        }
    }

    public void setHighLight(bool set)
    {
        _highLighting = set;
        if (_highLighting)
            _thicknessValue = _thicknessMin;
        else
            _thicknessValue = 0.0f;

        for (int i = 0; i < _currentRenderers.Count; i++)
        {
            _currentRenderers[i].material.SetFloat("_Thickness", _thicknessValue);


            for (int j = 0; j < _currentRenderers[i].materials.Length; j++)
            {
                _currentRenderers[i].materials[j].SetFloat("_Thickness", _thicknessValue);
            }
        }
    }
}
