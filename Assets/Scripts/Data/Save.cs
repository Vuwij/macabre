﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using Environment;
using Objects.Unmovable;
using System.Linq;
using Objects.Movable.Characters;

namespace Data
{
	[Serializable]
	public class Save
	{
		public string fileLocation
		{
			get { return saveURI + "/" + name; }
		}
		public string database
		{
			get { return fileLocation + "/MacabreDB.db3"; }
		}
		public string gameData
		{
			get { return fileLocation + "/gameData.json"; }
		}
		public string saveURI {
			get {
				return Game.dataPath + "/GameData/Saves";
			}
		}
		public string masterURI {
			get {
				return Game.dataPath + "/GameData/Master";
			}
		}

		// Save information
		public System.DateTime time;
		public string name;

		// Game information
		public List<Character> characters;
		protected Save(){}

		public Save(string name = "")
		{
			time = System.DateTime.Now;
			if (name != "") this.name = name;
			else this.name = "Save " + System.DateTime.Now;

			Directory.CreateDirectory(fileLocation);
			File.Copy(masterURI + "/MacabreDB.master.db3", fileLocation + "/MacabreDB.db3");
		}

		#region New, Load, Save, Delete 

		public void NewGame()
		{
		}

		public void LoadGame()
		{
		}

		public void SaveGame()
		{
		}

		public void DeleteGame()
		{
		}

		#endregion
	}
}