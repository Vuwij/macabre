﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects.Unmovable.Building;
using Objects.Unmovable.Furniture;
using Objects.Unmovable.Path;

namespace Objects.Unmovable
{
    public class RoomController : UnmovableObject
    {
		// The Parent Building Controller
		public Building.Building buildingController
        {
            get
            {
				var room = GetComponentInParent<Building.Building>();
				if (room == null) throw new Exception("Room not specified for the furniture: " + name);
                return room;
            }
        }

		#region Paths

		public VirtualPath[] paths
		{
			get { return GetComponentsInChildren<VirtualPath>(); }
		}

		#endregion

		#region Furniture

		public List<AbstractFurniture> Furniture
		{
			get
			{
				return gameObject.GetComponentsInChildren<AbstractFurniture>().ToList();
			}
		}

		#endregion
    }
}
