using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enabler : MonoBehaviour
{

	[SerializeField] private GameObject target;

	public void Trigger(bool isEnable) {
		target.SetActive(isEnable);
	}
}
