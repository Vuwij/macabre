﻿using UnityEngine;

namespace Objects
{
    public abstract partial class MacabreObjectController : MonoBehaviour {
        // This is the field that allows editing
        protected abstract MacabreObject model { get; }

        protected T GetNearestMacabreObject<T>()
            where T : MacabreObjectController
        {
            RaycastHit2D[] castStar = Physics2D.CircleCastAll(transform.position, GameSettings.inspectRadius, Vector2.zero);

            foreach (RaycastHit2D raycastHit in castStar)
            {
                T hit = raycastHit.collider.GetComponentInChildren<T>();
                if (hit != null) return hit;
            }

            Debug.Log("No objects within radius");
            return null;
        }

        protected virtual void Start()
        {
            CreateCollisionCircle();
            CreateProximityCircle();
            SetupBackEdgeCollider();
        }
    }
}
