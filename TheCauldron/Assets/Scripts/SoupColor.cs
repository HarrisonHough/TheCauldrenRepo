using UnityEngine;
using System.Collections;

public class SoupColor : MonoBehaviour {

  public Color NextColor;
  public Material SoupMaterial;
  public ParticleSystem Smoke;
  public Light SoupLight;
  private Color LastColor;
  private float timer;
  private float duration;

	// Use this for initialization
	void Start () {
    duration = 3;
    timer = 0;
    LastColor = new Color();
	}

	// Update is called once per frame
	void Update () {
    if (timer == 0) {
      LastColor = SoupMaterial.color;
    }

    if (!NextColor.Equals(SoupMaterial.color)) {
      timer += Time.deltaTime;
      Color c = Color.Lerp(LastColor, NextColor, timer / duration);
      SoupMaterial.color = c;
      SoupLight.color = c;
      SoupLight.intensity = Mathf.Max(duration - timer, 1);

      var col = Smoke.colorOverLifetime;
      col.enabled = true;

      Gradient grad = new Gradient();
      grad.SetKeys( new GradientColorKey[] { new GradientColorKey(c, 0.0f), new GradientColorKey(Color.gray, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );

      col.color = new ParticleSystem.MinMaxGradient(grad);

      if (timer >= duration) {
        SoupMaterial.color = NextColor;
        SoupLight.intensity = 1;
        timer = 0;
      }
    }
	}
}
