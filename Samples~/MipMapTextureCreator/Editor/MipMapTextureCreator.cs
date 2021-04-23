using UnityEngine;
using UnityEditor;

public class MipMapTextureCreator : ScriptableWizard
{
	public int texSize = 32;
	public bool autoColor = true;
	public Color[] mipMapColors;
	public int mipMaps;

	[MenuItem("Window/Analysis/SceneDebugViewer/MipMapTextureCreator")]
	static void CreateWizard()
	{
		DisplayWizard<MipMapTextureCreator>("Create MipMap Texture", "Create").OnValidate();
	}

	void OnWizardCreate()
	{
		Texture2D tex = new Texture2D(texSize, texSize, TextureFormat.RGBAFloat, mipMaps, false);

		for (int m = 0; m < tex.mipmapCount; m++)
		{
			Color[] c = tex.GetPixels(m);

			for (int i = 0; i < c.Length; ++i)
			{
				c[i] = mipMapColors[m];
			}

			tex.SetPixels(c, m);
		}

		//Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBAFloat, mipMaps, false);

		//for (int m = 0; m < mipMaps; m++)
		//{
		//	tex.SetPixels(new Color[] { mipMapColors[m], mipMapColors[m], mipMapColors[m], mipMapColors[m] }, m);
		//}

		tex.Apply(false, true);

		AssetDatabase.CreateAsset(tex, "Assets/MipMap.asset");
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	private void OnValidate()
	{
		if (autoColor)
		{
			mipMaps = 1 + Mathf.FloorToInt(Mathf.Log(texSize, 2));
			mipMapColors = new Color[mipMaps];

			for (int c = 0; c < mipMaps; c++)
			{
				mipMapColors[c] = Color.HSVToRGB(0.6f - (0.6f / mipMaps * c), 1, 1);
				// mipMapColors[c].a = Mathf.Sin(1.0f / mipMaps * c);
			}
		}
	}
}
