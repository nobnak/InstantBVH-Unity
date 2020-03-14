using nobnak.Gist.ObjectExt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recon.Examples {

	public class ParticleCtrl : MonoBehaviour {
		public const float CIRCLE_DEG = 360f;

		[SerializeField]
		protected GameObject fab = default;
		[SerializeField]
		protected int num = 100;
		[SerializeField]
		protected float speed = 0.1f;

		protected List<Transform> particles = new List<Transform>();
		protected List<Vector3> initialPositions = new List<Vector3>();
		protected float radius;

		#region unity
		private void OnEnable() {
			var c = Camera.main;
			var dfc = Vector3.Dot(transform.position - c.transform.position, c.transform.forward);
			radius = Mathf.Sin(c.fieldOfView * Mathf.Deg2Rad) * dfc;

			fab.SetActive(false);
		}
		private void OnDisable() {
			for (var i = 0; i < particles.Count; i++) {
				var p = particles[i];
				p.DestroyGo();
			}
			particles.Clear();
		}
		private void Update() {
			num = Mathf.Max(0, num);

			var t = Time.realtimeSinceStartup * speed;

			while (particles.Count > num) {
				var i = particles.Count - 1;
				var p = particles[i];
				particles.RemoveAt(i);
				initialPositions.RemoveAt(i);
				p.DestroyGo();
			}
			for (var i = particles.Count; i < num; i++) {
				var p = Instantiate(fab);
				p.hideFlags = HideFlags.DontSave;
				p.transform.SetParent(transform, false);
				var pos = radius * Random.insideUnitSphere;
				p.transform.localPosition = pos;
				particles.Add(p.transform);
				initialPositions.Add(pos);
				p.SetActive(true);
			}

			for (var i = 0; i < particles.Count; i++) {
				var p = particles[i];
				var rot = Quaternion.Euler(
					Mathf.PerlinNoise(i + t, 0) * CIRCLE_DEG,
					Mathf.PerlinNoise(i + t, 100) * CIRCLE_DEG,
					Mathf.PerlinNoise(i + t, 200) * CIRCLE_DEG
					);
				var pos = initialPositions[i];
				p.transform.localPosition = rot * pos;
			}
		}
		#endregion
	}
}
