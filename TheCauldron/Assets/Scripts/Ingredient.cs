using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType { Potion, Eye, Ham, Cheese, Pizza, DragonClaw, Pig };

public class Ingredient : MonoBehaviour {

    
    public IngredientType ingredientType;

    public Color soupColor;

    [SerializeField]
    private Rigidbody rigidbody;

    private Vector3 startPosition;
    private Quaternion startRotation;

	// Use this for initialization
	void Start () {
        if(rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();


        startPosition = transform.position;
        startRotation = transform.rotation;
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Floor"))
        {
            SoundManager.Instance.PlayRandomFloorDrop();
        }
        else if (collision.collider.tag.Equals("Table"))
        {
            SoundManager.Instance.PlayTableDrop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();

        if (!GameManager.gameOver)
        {
            if (other.gameObject.tag == "IngredientHit")
            {
                Cauldron.Instance.Mixer.AddIngredient(ingredientType);

                //soup.GetComponent<SoupColor>().NextColor = SoupColor;
                Cauldron.Instance.Soup.NextColor = soupColor;

                // TODO remove this and make "Recyclable"
                SetInactive();
            }
            else if (other.gameObject.tag == "Enemy")
            {
                if (!GetComponent<Rigidbody>().isKinematic)
                {
                    other.gameObject.GetComponent<Enemy>().OnHit();
                    SoundManager.Instance.PlayLevelComplete();
                }
            }
            else if (other.gameObject.tag == "Floor")
            {
                // TODO remove this and make "Recyclable"
                SetInactive();
            }
        }
    }

    public void FlyTo(Vector3 target)
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
        Vector3 direction = target - transform.position;
        direction.Normalize();
        rigidbody.AddForce(direction * 500);
    }

    private void SetInactive()
    {
        //deactivate physics
        rigidbody.isKinematic = true;
        //move out of view
        transform.position = new Vector3(0, -20, 0);
    }

    public void Reset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }


}
