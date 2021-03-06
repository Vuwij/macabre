﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Objects;
using System.Linq;
using Objects.Movable.Characters;

public class MacabreFootprint : EditorWindow {
    
	static int leftExtension = 2; // Must be even
	static int rightExtension = 2;
	static int upExtension = 2;
	static int downExtension = 2;


	[MenuItem ("Macabre/Footprint/Find Object Footprints")]
	static void FindObjectFootprint() {
		if (Selection.gameObjects.Count() != 1) return;

		GameObject o = Selection.gameObjects[0];
        
        // Get original sprite, continue if fail
        SpriteRenderer originalSpriteRenderer = o.GetComponent<SpriteRenderer>();
		if (originalSpriteRenderer == null) return;
		if (originalSpriteRenderer.sprite == null) return;
		if (o.GetComponent<Character>() != null) return;

        // Undo if made mistake
        Undo.RecordObject(o, "Set Object Footprint");

        // Recreate the footprint if there isn't one               
        if (o.transform.GetComponentInChildren<PixelCollider>() != null)
            DestroyImmediate(o.transform.GetComponentInChildren<PixelCollider>().gameObject);

        GameObject footprint = new GameObject("Footprint");
        footprint.AddComponent<PixelCollider>();
        footprint.AddComponent<SpriteRenderer>();
        footprint.transform.parent = o.transform;

        PixelCollider pixelCollider = o.GetComponentInChildren<PixelCollider>();
        SpriteRenderer footprintSpriteRenderer = pixelCollider.GetComponent<SpriteRenderer>();

        // Find the footprint
        string textureName = AssetDatabase.GetAssetPath(originalSpriteRenderer.sprite.texture);
        string textureNameWithFootprint = textureName.Replace(".png", "");
        string footprintTextureName = textureNameWithFootprint + "Footprint.png";
        if (!File.Exists(footprintTextureName))
        {
            Debug.LogError("Footprint Texture not found: " + footprintTextureName);
        }
        Debug.Assert(File.Exists(footprintTextureName));

        // Make a copy of the texture's footprint
        AssetImporter originalImporter = AssetImporter.GetAtPath(textureName);
        AssetImporter footprintImporter = AssetImporter.GetAtPath(footprintTextureName);
        EditorUtility.CopySerializedIfDifferent(originalImporter, footprintImporter);
        footprintImporter.SaveAndReimport();

        TextureImporter footprintTextureImporter = (TextureImporter)AssetImporter.GetAtPath(footprintTextureName);
        SpriteMetaData[] spriteMetaDatas = footprintTextureImporter.spritesheet;
        for (int s = 0; s < spriteMetaDatas.Length; ++s)
        {
            Rect rect = spriteMetaDatas[s].rect;
			rect.x = rect.x - leftExtension;
            rect.width = rect.width + leftExtension + rightExtension;
            rect.y = rect.y - downExtension;
            rect.height = rect.height + downExtension + upExtension;
            spriteMetaDatas[s].rect = rect;
        }
        footprintTextureImporter.spritesheet = spriteMetaDatas;

        EditorUtility.SetDirty(footprintTextureImporter);
        footprintTextureImporter.SaveAndReimport();
        AssetDatabase.ImportAsset(footprintTextureName, ImportAssetOptions.ForceUpdate);

        UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(footprintTextureName);
        footprintSpriteRenderer.sprite = (Sprite)sprites.Where((x) => x.name == originalSpriteRenderer.sprite.name).First();
        footprintSpriteRenderer.sortingLayerName = "Background";
        footprintSpriteRenderer.sortingOrder = 10;
        footprint.transform.localPosition = Vector3.zero;
        footprintSpriteRenderer.color = new Color(255, 255, 255, 0.5f);

		// Pixel Perfect
		footprint.AddComponent<PixelPerfectSprite>();

        // Collider
        PolygonCollider2D collider = footprint.AddComponent<PolygonCollider2D>();
        CalculateFootprintPolygonCollider(originalSpriteRenderer, footprintSpriteRenderer, collider);
	}

	[MenuItem ("Macabre/Footprint/Find Room Footprints")]
    static void FindRoomFootprints () {

        foreach (GameObject objs in Selection.gameObjects) {

			if (objs.GetComponent<PixelRoom>() == null) continue;
			SpriteRenderer roomSpriteRenderer = objs.GetComponent<PixelRoom>().GetComponent<SpriteRenderer>();
			if(roomSpriteRenderer != null) roomSpriteRenderer.sortingOrder = 0;

			for (int i = 0; i < objs.transform.childCount; ++i)
			{
				Transform o = objs.transform.GetChild(i);
				if (o.name == "Room Effects") continue;

				// Get original sprite, continue if fail
				SpriteRenderer originalSpriteRenderer = o.GetComponent<SpriteRenderer>();
                if (originalSpriteRenderer == null) continue;
                if (originalSpriteRenderer.sprite == null) continue;
				if (o.GetComponent<Character>() != null) continue;

                // Undo if made mistake
                Undo.RecordObject(o, "Set Object Footprint");

                // Recreate the footprint if there isn't one               
                if (o.transform.GetComponentInChildren<PixelCollider>() != null)
					DestroyImmediate(o.transform.GetComponentInChildren<PixelCollider>().gameObject);

				GameObject footprint = new GameObject("Footprint");
                footprint.AddComponent<PixelCollider>();
                footprint.AddComponent<SpriteRenderer>();
				footprint.transform.parent = o.transform;

                PixelCollider pixelCollider = o.GetComponentInChildren<PixelCollider>();
                SpriteRenderer footprintSpriteRenderer = pixelCollider.GetComponent<SpriteRenderer>();
                
                // Find the footprint
                string textureName = AssetDatabase.GetAssetPath(originalSpriteRenderer.sprite.texture);
                string textureNameWithFootprint = textureName.Replace(".png", "");
                string footprintTextureName = textureNameWithFootprint + "Footprint.png";
				if(!File.Exists(footprintTextureName)) {
					Debug.LogError("Footprint Texture not found: " + footprintTextureName);
				}
                Debug.Assert(File.Exists(footprintTextureName));

                // Make a copy of the texture's footprint
				AssetImporter originalImporter = AssetImporter.GetAtPath(textureName);
				AssetImporter footprintImporter = AssetImporter.GetAtPath(footprintTextureName);
				EditorUtility.CopySerializedIfDifferent(originalImporter, footprintImporter);
				footprintImporter.SaveAndReimport();

				TextureImporter footprintTextureImporter = (TextureImporter) AssetImporter.GetAtPath(footprintTextureName);
				SpriteMetaData[] spriteMetaDatas = footprintTextureImporter.spritesheet;
				for (int s = 0; s < spriteMetaDatas.Length; ++s) {
					Rect rect = spriteMetaDatas[s].rect;
					rect.x = rect.x - leftExtension;
					rect.width = rect.width + leftExtension + rightExtension;
					rect.y = rect.y - downExtension;
					rect.height = rect.height + downExtension + upExtension;
					spriteMetaDatas[s].rect = rect;
				}
				footprintTextureImporter.spritesheet = spriteMetaDatas;

				EditorUtility.SetDirty(footprintTextureImporter);
				footprintTextureImporter.SaveAndReimport();
				AssetDatabase.ImportAsset(footprintTextureName, ImportAssetOptions.ForceUpdate);
                
				UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(footprintTextureName);
                footprintSpriteRenderer.sprite = (Sprite) sprites.Where((x) => x.name == originalSpriteRenderer.sprite.name).First();
				footprintSpriteRenderer.sortingLayerName = "Background";
				footprintSpriteRenderer.sortingOrder = 10;
				footprint.transform.localPosition = Vector3.zero;
				footprintSpriteRenderer.color = new Color(0, 0, 0, 0.5f);
    
				// Pixel Perfect
                footprint.AddComponent<PixelPerfectSprite>();

                // Collider
				PolygonCollider2D collider = footprint.AddComponent<PolygonCollider2D>();
				CalculateFootprintPolygonCollider(originalSpriteRenderer, footprintSpriteRenderer, collider);            
			}
        }
    }
       
	[MenuItem("Macabre/Footprint/Find Room Shader")]
    static void FindRoomShader()
    {
        foreach (GameObject objs in Selection.gameObjects)
        {
            if (objs.GetComponent<PixelRoom>() == null) continue;
            SpriteRenderer roomSpriteRenderer = objs.GetComponent<PixelRoom>().GetComponent<SpriteRenderer>();
            if (roomSpriteRenderer != null) roomSpriteRenderer.sortingOrder = 0;

			List<Transform> inspectedTransforms = new List<Transform>();
			for (int i = 0; i < objs.transform.childCount; ++i)
				inspectedTransforms.Add(objs.transform.GetChild(i));
			inspectedTransforms.Add(objs.transform);

			foreach(Transform o in inspectedTransforms)
            {
                if (o.name == "Room Effects") continue;

                // Get original sprite, continue if fail
                SpriteRenderer originalSpriteRenderer = o.GetComponent<SpriteRenderer>();
                if (originalSpriteRenderer == null) continue;
                if (originalSpriteRenderer.sprite == null) continue;
                if (o.GetComponent<Character>() != null) continue;

                // Undo if made mistake
                Undo.RecordObject(o, "Set Object Footprint");
                
                // Find the normal texture
                string textureName = AssetDatabase.GetAssetPath(originalSpriteRenderer.sprite.texture);
				string textureNameWithNormal = textureName.Replace(".png", "");
				string normalMapTextureName = textureNameWithNormal + "Normal.png";
				if (!File.Exists(normalMapTextureName))
				{
					Debug.LogError("Footprint Texture not found: " + normalMapTextureName);
					continue;
				}

                // Make a copy of the texture's normal map
				AssetImporter normalMapImporter = AssetImporter.GetAtPath(normalMapTextureName);            
				TextureImporter normalMapTextureImporter = (TextureImporter)AssetImporter.GetAtPath(normalMapTextureName);

				if (normalMapTextureImporter.textureType != TextureImporterType.NormalMap)
				{
					normalMapTextureImporter.textureType = TextureImporterType.NormalMap;
					EditorUtility.SetDirty(normalMapTextureImporter);
					normalMapTextureImporter.SaveAndReimport();
					AssetDatabase.ImportAsset(normalMapTextureName, ImportAssetOptions.ForceUpdate);
				}

				// Make a new Texture
				string materialPath = normalMapTextureName.Replace("Spritesheets", "Materials").Replace(".png", ".mat");
				if (!File.Exists(materialPath))
				{
					Material defaultMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Structures/Default/Default.mat");
					Texture normalMapTexture = AssetDatabase.LoadAssetAtPath<Texture>(normalMapTextureName);
					Material material = new Material(defaultMaterial);
					material.SetTexture("_BumpMap", normalMapTexture);

					AssetDatabase.CreateAsset(material, materialPath);
				}

				// Link object spritesheet with material
				Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
				originalSpriteRenderer.material = newMaterial;
            }
        }
    }
    
	static Color sharedColor = Color.black;

	static void CalculateFootprintPolygonCollider(SpriteRenderer original, SpriteRenderer shadow, PolygonCollider2D polygonCollider2D)
    {
		Sprite sprite = shadow.sprite;
        Rect rect = sprite.rect;

		int x = Mathf.FloorToInt(rect.x);
        int y = Mathf.FloorToInt(rect.y);
        int width = Mathf.FloorToInt(rect.width);
        int height = Mathf.FloorToInt(rect.height);
        
		Color[] colorMap = shadow.sprite.texture.GetPixels(x, y, width, height);

		// Colors we are finding
		Dictionary<Color, Vector2[]> colorPoints = new Dictionary<Color, Vector2[]>();

		for (int p = 0; p < width * height; p++) {
			if(colorMap[p] == sharedColor) continue;
			if((int) colorMap[p].a == 0) continue;
			if(colorPoints.ContainsKey(colorMap[p])) continue;
			colorPoints.Add(colorMap[p], null);
		}

		for(int c = 0; c < colorPoints.Count; c++) {
			int topIndex = 0;
			int bottomIndex = 0;
			int leftIndex = 0;
			int rightIndex = 0;

			int xMax = 0;
			int xMin = 0;
			int yMax = 0;
			int yMin = 0;

			// Find a first valid pixel
			for (int p = 0; p < width * height; p++)
			{
				if (colorMap[p] == colorPoints.ElementAt(c).Key)
				{
					int i = p % width;
					int j = p / width;

					rightIndex = p;
					leftIndex = p;
					bottomIndex = p;
					topIndex = p;

					xMax = i;
					xMin = i;
					yMax = j;
					yMin = j;

					break;
				}
			}

			// Loop through all the pixels and get the 4 directional indices
			Color validColor = colorPoints.ElementAt(c).Key;
			for (int p = 0; p < width * height; p++)
			{
				bool valid = (colorMap[p] == validColor);
				if(colorMap[p] == Color.black) {
					// Right
					if (p + 2 < width * height && p + 2 >= 0 && colorMap[p + 2] == validColor)
						valid = true;
					else if (p + 1 < width * height && p + 1 >= 0 && colorMap[p + 1] == validColor)
						valid = true;
					else if (p - 2 < width * height && p - 2 >= 0 && colorMap[p - 2] == validColor)
						valid = true;
					else if (p - 1 < width * height && p - 1 >= 0 && colorMap[p - 1] == validColor)
						valid = true;
					else if (p + width < width * height && p + width >= 0 && colorMap[p + width] == validColor)
						valid = true;
					else if (p - width < width * height && p - width >= 0 && colorMap[p - width] == validColor)
						valid = true;
				}
				if (valid)
				{
					int i = p % width;
					int j = p / width;

					if (i > xMax)
					{
						rightIndex = p;
						xMax = i;
					}
					else if (i < xMin)
					{
						leftIndex = p;
						xMin = i;
					}
					if (j > yMax)
					{
						bottomIndex = p;
						yMax = j;
					}
					else if (j < yMin)
					{
						topIndex = p;
						yMin = j;
					}
				}
			}

			// Create the collider2D based on the index, since its one pixel to one index
			Vector2 topVector = new Vector2(topIndex % width, topIndex / width) - sprite.pivot;
			Vector2 bottomVector = new Vector2(bottomIndex % width, bottomIndex / width) - sprite.pivot;
			Vector2 leftVector = new Vector2(leftIndex % width, leftIndex / width) - sprite.pivot;
			Vector2 rightVector = new Vector2(rightIndex % width, rightIndex / width) - sprite.pivot;

			topVector = topVector + new Vector2(1.0f, 0.5f);
			bottomVector = bottomVector + new Vector2(1.0f, 0.5f);
			leftVector = leftVector + new Vector2(1.0f, 0.5f);
			rightVector = rightVector + new Vector2(0.0f, 0.5f);
            
			Vector2[] points = new Vector2[4]
			{
				topVector,
				rightVector,
				bottomVector,
				leftVector
			};

			colorPoints[colorPoints.ElementAt(c).Key] = points;
		}

		if(colorPoints.Count == 0) return;

		polygonCollider2D.pathCount = colorPoints.Count;
		polygonCollider2D.isTrigger = true;

		int index = 0;
		foreach(var c in colorPoints) {
			polygonCollider2D.SetPath(index, c.Value);
			index++;
		}
    }
}