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

		if(points<400){
			denominator="400";
			rank="NOVICE";
		}else if(800f>points && points>399f){
			denominator="800";
			rank="APPRENTICE";
		}
		else if(1200f>points && points>799f){
			denominator="1200";
			rank="JOURNEYMAN";
		}
		else if(1600f>points && points>1199f){
			denominator="1600";
			rank="ARTISAN";
		}
		else if(2000f>points && points>1599f){
			denominator="2000";
			rank="PROFESSIONAL";
		}
		else if(2400f>points && points>1999f){
			denominator="2400";
			rank="EXPERT";
		}

		DisplayTextObject.text=""+rank+"   "+points+"/"+denominator;




		//DisplayTextObjectResist.text="RESISTANCE\n "+resistancePoints+"/100";
	}
}
