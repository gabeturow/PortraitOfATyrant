using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointsController : MonoBehaviour {
	[SerializeField]
	public float points=0;
	[SerializeField]
	public static int resistancePoints=50;

	public static PointsController main;

	public Text DisplayTextObject;
	public string rank;
	public string denominator;
	public Text DisplayTextObjectResist;
	// Use this for initialization
	void Start () {
		main=this;
	}
	
	// Update is called once per frame
	void Update () {

		if(points<1000f){
			denominator="1000";
			rank="NOVICE";
		}else if(2000f>points && points>999f){
			denominator="2000";
			rank="APPRENTICE";
		}
		else if(3500f>points && points>1999f){
			denominator="3500";
			rank="PROFESSIONAL";
		}
		else if(5500f>points && points>3499f){
			denominator="5500";
			rank="CRAFTSMAN";
		}
		else if(8000f>points && points>5499f){
			denominator="8000";
			rank="ARTISAN";
		}
		else if(10000f>points && points>7999f){
			denominator="10000";
			rank="EXPERT";
		}

		DisplayTextObject.text=""+rank+"\nXP: "+points+"/"+denominator;




		//DisplayTextObjectResist.text="RESISTANCE\n "+resistancePoints+"/100";
	}
}
