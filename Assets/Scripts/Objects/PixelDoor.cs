﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Objects
{
	public class PixelDoor : MonoBehaviour
    {
        public PixelRoom destination;
        public Vector2 dropOffLocation; // Specified in pixels

		public Direction interactionDirection = Direction.All;

		public PixelRoom source {
			get {
				PixelCollider c = this.GetComponentInChildren<PixelCollider>();
				Debug.Assert(c != null);
				return c.GetPixelRoom();
			}
		}

		public Vector2 dropOffWorldLocation {
			get {
				PixelCollider px = GetComponentInChildren<PixelCollider>();
				Vector2 s1;
				if (px != null)
				    s1 = PixelPoint.Shift(px.centerWorld, Direction.NE, dropOffLocation.y);
				else
					s1 = PixelPoint.Shift(transform.position, Direction.NE, dropOffLocation.y);
				
				Vector2 s2 = PixelPoint.Shift(s1, Direction.SE, dropOffLocation.x);

				return s2;
			}
		}

		public Vector2 dropInWorldLocation {
			get {
				Debug.Assert(destination != null);
				List<PixelDoor> otherDoorsToThisDoor = destination.pixelDoors.FindAll(x => (x.destination == source));
				if(otherDoorsToThisDoor.Count == 0) {
					Debug.LogWarning("No doors go to " + source);
					return transform.position;
				}

				PixelDoor closestDoor = otherDoorsToThisDoor.Aggregate((x, y) => Vector2.Distance(dropOffWorldLocation, x.dropOffWorldLocation) < Vector2.Distance(dropOffWorldLocation, y.dropOffWorldLocation) ? x : y);
				return closestDoor.dropOffWorldLocation;
			}
		}

		private void OnDrawGizmos()
		{
#if UNITY_EDITOR
			Gizmos.color = Color.white;
			Gizmos.DrawSphere(dropOffWorldLocation, 1.0f);
#endif
		}
	}
}