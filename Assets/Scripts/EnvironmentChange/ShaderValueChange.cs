using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;



public class ShaderValueChange : EnvironmentChangeBase
{
    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private ShaderChangeType _changeType = ShaderChangeType.number;

    [Header("Property Name")]

    [SerializeField]
    private string _propertyName;

    [SerializeField]
    private int _materialIndex = 0;

    [Header("Number value Changes")]


    [SerializeField]
    private float _floatStart = 0f;

    [SerializeField]
    public float _floatEnd = 0f;

    [Header("Colour value Changes")]

    [ColorUsage(true, true)]
    [SerializeField]
    private Color _startColor = Color.white;

    [ColorUsage(true, true)]
    [SerializeField]
    public Color _endColor = Color.white;

    [Header("Vector value Changes")]


    [SerializeField]
    private Vector3 _startVector = Vector3.zero;

    [SerializeField]
    public Vector3 _endVector = Vector3.zero;


    private void Awake()
    {
        if(_target == null)
        {
        _target = this.gameObject;

        }

        if (_target.GetComponent<MeshRenderer>() != null)
        {
            MeshRenderer meshRenderer = _target.GetComponent<MeshRenderer>();
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(materialPropertyBlock, _materialIndex);

            meshRenderer.materials[_materialIndex].EnableKeyword("_EMISSION");

            switch (_changeType)
            {
                case ShaderChangeType.number:
                    _floatStart = meshRenderer.materials[_materialIndex].GetFloat(_propertyName);
                    materialPropertyBlock.SetFloat(_propertyName, _floatStart);

                    break;
                case ShaderChangeType.color:
                    _startColor = meshRenderer.materials[_materialIndex].GetColor(_propertyName);
                    materialPropertyBlock.SetColor(_propertyName, _startColor);
                    break;
                case ShaderChangeType.vector:
                    _startVector = meshRenderer.materials[_materialIndex].GetVector(_propertyName);
                    materialPropertyBlock.SetVector(_propertyName, _endVector);
                    break;



            }
           
            meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);

        }

        if (_target.GetComponent<SkinnedMeshRenderer>() != null)
        {
            SkinnedMeshRenderer meshRenderer = _target.GetComponent<SkinnedMeshRenderer>();
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(materialPropertyBlock, _materialIndex);
            meshRenderer.materials[_materialIndex].EnableKeyword("_EMISSION");

            switch (_changeType)
            {
                case ShaderChangeType.number:
                    _floatStart = meshRenderer.materials[_materialIndex].GetFloat(_propertyName);
                    materialPropertyBlock.SetFloat(_propertyName, _floatStart);

                    break;
                case ShaderChangeType.color:
                    _startColor = meshRenderer.materials[_materialIndex].GetColor(_propertyName);
                    materialPropertyBlock.SetColor(_propertyName, _startColor);
                    break;
                case ShaderChangeType.vector:
                    _startVector = meshRenderer.materials[_materialIndex].GetVector(_propertyName);
                    materialPropertyBlock.SetVector(_propertyName, _endVector);
                    break;



            }
            meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);

        }

    }

    public override void DoChange(float percentage)
    {
        if (_target.GetComponent<MeshRenderer>() != null)
        {
            MeshRenderer meshRenderer = _target.GetComponent<MeshRenderer>();

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            meshRenderer.GetPropertyBlock(materialPropertyBlock, _materialIndex);

            meshRenderer.materials[_materialIndex].EnableKeyword("_EMISSION");

            switch (_changeType)
            {
                case ShaderChangeType.number:

                    materialPropertyBlock.SetFloat(_propertyName, Mathf.Lerp(_floatStart, _floatEnd, percentage));
                    break;
                case ShaderChangeType.color:

                    materialPropertyBlock.SetVector(_propertyName, Color.Lerp(_startColor, _endColor, percentage));
                    break;
                case ShaderChangeType.vector:

                    materialPropertyBlock.SetVector(_propertyName, Vector3.Lerp(_startVector, _endVector, percentage));
                    break;

            }

            meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
        }


        if (_target.GetComponent<SkinnedMeshRenderer>() != null)
        {
            SkinnedMeshRenderer meshRenderer = _target.GetComponent<SkinnedMeshRenderer>();

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            meshRenderer.GetPropertyBlock(materialPropertyBlock, _materialIndex);


            switch (_changeType)
            {
                case ShaderChangeType.number:

                    materialPropertyBlock.SetFloat(_propertyName, Mathf.Lerp(_floatStart, _floatEnd, percentage));
                    break;
                case ShaderChangeType.color:

                    materialPropertyBlock.SetVector(_propertyName, Color.Lerp(_startColor, _endColor, percentage));
                    break;
                case ShaderChangeType.vector:

                    materialPropertyBlock.SetVector(_propertyName, Vector3.Lerp(_startVector, _endVector, percentage));
                    break;

            }
            meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
        }


      //  DynamicGI.UpdateEnvironment();

    }


    public override void TweenChange(float percentage, float duration)
    {
        if (_target.GetComponent<MeshRenderer>() != null)
        {
            MeshRenderer meshRenderer = _target.GetComponent<MeshRenderer>();

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            meshRenderer.GetPropertyBlock(materialPropertyBlock, _materialIndex);

            meshRenderer.materials[_materialIndex].EnableKeyword("_EMISSION");

            switch (_changeType)
            {
                case ShaderChangeType.number:

                    float targetFloat = Mathf.Lerp(_floatStart, _floatEnd, percentage);

                    DOVirtual.Float(materialPropertyBlock.GetFloat(_propertyName), targetFloat, duration, (float value) =>
                    {
                        materialPropertyBlock.SetFloat(_propertyName, value);
                        meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
                    });
                    break;
                case ShaderChangeType.color:

                    Color targetColor = Color.Lerp(_startColor, _endColor, percentage);

                    DOVirtual.Color(materialPropertyBlock.GetColor(_propertyName), targetColor, duration, (Color value) =>
                    {
                        materialPropertyBlock.SetColor(_propertyName, value);
                        meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
                    });
                    break;
                case ShaderChangeType.vector:

                    Vector3 targetVector = Vector3.Lerp(_startVector, _endVector, percentage);

                    DOVirtual.Vector3(materialPropertyBlock.GetVector(_propertyName), targetVector, duration, (Vector3 value) =>
                    {
                        materialPropertyBlock.SetVector(_propertyName, value);
                        meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
                    });
                    break;

            }





        }


        if (_target.GetComponent<SkinnedMeshRenderer>() != null)
        {
            SkinnedMeshRenderer meshRenderer = _target.GetComponent<SkinnedMeshRenderer>();

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            meshRenderer.GetPropertyBlock(materialPropertyBlock, _materialIndex);


            switch (_changeType)
            {
                case ShaderChangeType.number:
                    float targetFloat = Mathf.Lerp(_floatStart, _floatEnd, percentage);

                    DOVirtual.Float(materialPropertyBlock.GetFloat(_propertyName), targetFloat, duration, (float value) =>
                    {
                      materialPropertyBlock.SetFloat(_propertyName, value);
                      meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
                    });
                    break;
                case ShaderChangeType.color:

                    Color targetColor = Color.Lerp(_startColor, _endColor, percentage);

                    DOVirtual.Color(materialPropertyBlock.GetColor(_propertyName), targetColor, duration, (Color value) =>
                    {
                        materialPropertyBlock.SetColor(_propertyName, value);
                        meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
                    });
                    break;
                case ShaderChangeType.vector:

                    Vector3 targetVector = Vector3.Lerp(_startVector, _endVector, percentage);

                    DOVirtual.Vector3(materialPropertyBlock.GetVector(_propertyName), targetVector, duration, (Vector3 value) =>
                    {
                        materialPropertyBlock.SetVector(_propertyName, value);
                        meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
                    });
                    break;

            }
        }
      //  DynamicGI.UpdateEnvironment();
    }



    public void ChangeColorTweened(float duration, Color color)
    {
        if (_target.GetComponent<SkinnedMeshRenderer>() != null)
        {
            SkinnedMeshRenderer meshRenderer = _target.GetComponent<SkinnedMeshRenderer>();

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            meshRenderer.GetPropertyBlock(materialPropertyBlock, _materialIndex);
          


            DOVirtual.Color(materialPropertyBlock.GetColor(_propertyName), color, duration, (Color value) =>
             {
                 materialPropertyBlock.SetColor(_propertyName, value);
                 meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
             });


        }
        else
        {
            //Debug.Log("noskinnedmeshrenderer");
        }

        if (_target.GetComponent<MeshRenderer>() != null)
        {
            MeshRenderer meshRenderer = _target.GetComponent<MeshRenderer>();

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            meshRenderer.GetPropertyBlock(materialPropertyBlock, _materialIndex);


            DOVirtual.Color(materialPropertyBlock.GetColor(_propertyName), color, duration, (Color value) =>
            {
                materialPropertyBlock.SetColor(_propertyName, value);
                meshRenderer.SetPropertyBlock(materialPropertyBlock, _materialIndex);
            });


        }
        else
        {
           // Debug.Log("nomeshrenderer");

        }


    }

}


enum ShaderChangeType
{
    color,
    number,
    vector
}
