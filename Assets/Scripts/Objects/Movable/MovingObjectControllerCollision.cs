﻿using System;
using UnityEngine;

namespace Objects.Movable
{
    public abstract partial class MovingObjectController : MacabreObjectController
    {
        private SpriteRenderer spriteRenderer
        {
            get { return GetComponentInChildren<SpriteRenderer>(); }
        }

        // The collision circle for colliding with objects
        private EllipseCollider2D collisionCircle = null;
        protected virtual EllipseCollider2D CollisionCircle
        {
            get
            {
                if (collisionCircle == null)
                {
                    collisionCircle = gameObject.AddComponent<EllipseCollider2D>();
                    CreateCollisionCircle();
                }
                return collisionCircle;
            }
        }
        protected override Collider2D collisionBox
        {
            get { return collisionCircle; }
        }
        protected override Vector2[] SpriteColliderVectices
        {
            get
            {
                return CollisionCircle.points;
            }
        }

        protected virtual void CreateCollisionCircle()
        {
            float width = spriteRenderer.sprite.rect.width;
            CollisionCircle.radiusX = width / 100f;
            CollisionCircle.radiusY = width / 200f;
        }

        // The proximity circle for detecting if objects are nearby
        private EllipseCollider2D proximityCircle;
        protected virtual EllipseCollider2D ProximityCircle
        {
            get
            {
                if (proximityCircle == null)
                {
                    proximityCircle = gameObject.AddComponent<EllipseCollider2D>();
                    CreateProximityCircle();
                }
                
                return proximityCircle;
            }
        }
        protected override Collider2D proximityBox
        {
            get { return proximityCircle; }
        }
        protected override Vector2[] SpriteProximityVertices
        {
            get
            {
                return ProximityCircle.points;
            }
        }

        public virtual void CreateProximityCircle()
        {
            ProximityCircle.isTrigger = true;
            float width = spriteRenderer.sprite.rect.width;
            ProximityCircle.radiusX = width / 10f;
            ProximityCircle.radiusY = width / 20f;
        }
        
    }
}
