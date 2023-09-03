using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintParticle : MonoBehaviour {
    Texture2D _workingTexture;
    Renderer _workingRenderer;
    Texture2D _texture;

    Color _white = new Color(1,1,1);
    Color _heatColor = new Color(1,1,1);
    Color[] _colors = new Color[9];
    Color[] _heatColors = new Color[9];

    Texture2D _heatTexture2D;
    public GameObject HeatMap2D;
    Renderer _heatRenderer2D;
    Texture2D _texture_2D;

    public GameObject HeatMap3D;
    Renderer _heatRenderer3D;

    public float ParticuleWeight = 0.1f;

    bool _isCollision;

    static Vector3 _particlePos;

    void Start () {
        for(int i = 0; i< _colors.Length; i++)
        {
            _colors[i] = Color.white;
        }

        _workingRenderer = GetComponent<Renderer>();
        _texture = _workingRenderer.material.mainTexture as Texture2D;
        _workingTexture = new Texture2D (_texture.width, _texture.height);

        Color32[] sourcePixels = _texture.GetPixels32();
        _workingTexture.SetPixels32(sourcePixels);
        _workingRenderer.material.mainTexture = _workingTexture;
        _workingTexture.Apply();

        _heatRenderer2D = HeatMap2D.GetComponent<Renderer>();
        _texture_2D = _heatRenderer2D.material.mainTexture as Texture2D;
        _heatTexture2D = new Texture2D(_texture_2D.width, _texture_2D.height);

        _heatRenderer3D = HeatMap3D.GetComponent<Renderer>();

        Color32[] sourcePixels_ = _texture_2D.GetPixels32();
        _heatTexture2D.SetPixels32(sourcePixels_);
        _heatRenderer2D.material.mainTexture = _heatTexture2D;
        _heatRenderer3D.material.mainTexture = _heatTexture2D;
        _heatTexture2D.Apply();

        InvokeRepeating("ApplyTexture", 0f, .1f);
    }
	
    void OnParticleCollision(GameObject other) {
        _isCollision = true;

        int num = other.GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
        ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[num];
        int result = other.GetComponent<ParticleSystem>().GetCollisionEvents(gameObject, collisionEvents);
        _particlePos = other.transform.position;

        RaycastHit hit;
        Vector3 pos = Vector3.zero;
        Vector2 pixelUV;
        Vector2 pixelPoint;
        _heatColor = new Color(1,1,1);
        for (int i=0; i < num; i++){
            if (Physics.Raycast(collisionEvents[i].intersection, (collisionEvents[i].intersection - _particlePos).normalized, out hit))
            {
                pos = hit.point;
                pixelUV = hit.textureCoord;
                pixelPoint = new Vector2(pixelUV.x * _texture.width,pixelUV.y * _texture.height);

                _workingTexture.SetPixel((int)pixelPoint.x, (int)pixelPoint.y, _white);

                _heatColor = _heatTexture2D.GetPixel((int)pixelPoint.x, (int)pixelPoint.y);
                if(_heatColor.r > 0 && _heatColor.g == 1 && _heatColor.b > 0 )
                {
                    _heatColor.r = _heatColor.r - ParticuleWeight;
                    _heatColor.b = _heatColor.b - ParticuleWeight;

                }

                if (_heatColor.r == 0 && _heatColor.g == 1 && _heatColor.b == 0)
                {
                    _heatColor.g = _heatColor.g - 0.01f;
                }

                if (_heatColor.g < 1)
                {
                    _heatColor.r = _heatColor.r + ParticuleWeight;
                    _heatColor.g = _heatColor.g - ParticuleWeight;
                }

                for (int j = 0; j < _colors.Length; j++)
                {
                    _heatColors[j].r = _heatColor.r;
                    _heatColors[j].g = _heatColor.g;
                    _heatColors[j].b = _heatColor.b;
                }

                _heatTexture2D.SetPixel((int)pixelPoint.x, (int)pixelPoint.y, _heatColor);
            }
        }
    }

    public void resetTexture()
    {
        Start();
    }

    private void ApplyTexture()
    {
        if(_isCollision)
        {
            _heatTexture2D.Apply();
            _workingTexture.Apply();
        }
        _isCollision = false;
    }
}
