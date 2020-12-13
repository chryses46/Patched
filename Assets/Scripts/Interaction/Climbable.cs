using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Climbable : MonoBehaviour
{

	public Transform bottom;
	[SerializeField] private Transform rope;
	[SerializeField] private BoxCollider boxCollider;

	private void Update() {
#if UNITY_EDITOR
		if (EditorApplication.isPlaying) return;
		rope.transform.localPosition = rope.transform.localPosition.With(y: bottom.localPosition.y * 0.5f);
		rope.transform.localScale = rope.transform.localScale.With(y: bottom.localPosition.y);
		boxCollider.size = boxCollider.size.With(y: bottom.localPosition.y);
		boxCollider.center = boxCollider.center.With(y: bottom.localPosition.y * 0.5f);
#endif
	}
}
