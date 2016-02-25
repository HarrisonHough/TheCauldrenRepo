// Amplify Color - Advanced Color Grading for Unity Pro
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AmplifyColor
{
	public sealed class WatcherPostprocessor : AssetPostprocessor
	{
		public static event Action OnRefresh;

		static void OnPostprocessAllAssets( string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPaths )
		{
			if ( OnRefresh != null )
				OnRefresh();
		}
	}

	public class LUTBrowser : EditorWindow
	{
		private const string BaseFolder = "/AmplifyColor/";
		private const string BaseLUTFolder = "/AmplifyColor/LUTs/";
		private const string BaseSampleFolder = "/AmplifyColor/Textures/Preview/";

		private const int PreviewWidth = 512;
		private const int PreviewHeight = 256;
		private List<Texture2D> m_previewImages = new List<Texture2D>();

		private Dictionary<string, HashSet<string>> m_categories = new Dictionary<string, HashSet<string>>();
		private HashSet<Texture2D> m_loaded;
		private bool m_initialized = false;

		private Material m_previewMat;
		private Camera[] m_previewCameras;
		private int m_selectedPreview = 1;
		private int m_selectedApply = 0;

		private Texture2D m_selectedLUT = null;

		private const float MinPreviewSize = PreviewWidth / 8;
		private const float MaxPreviewSize = PreviewWidth;
		private float m_previewSize = PreviewWidth / 4;

		private bool m_dragging = false;

		private GUIStyle m_styleFoldout;
		private GUIStyle m_stylePreview;
		private GUIStyle m_stylePreviewFocus;
		private Vector2 m_scrollPos = new Vector2( 0, 0 );

		[MenuItem( "Window/Amplify Color/LUT Browser", false, 0 )]
		public static void Init()
		{
			GetWindow<LUTBrowser>( false, "LUT Browser", true );
		}

		private string FindBaseFolder( string[] guids, string pattern )
		{
			foreach ( string guid in guids )
			{
				string path = AssetDatabase.GUIDToAssetPath( guid );
				int startIndex = path.IndexOf( pattern );
				if ( startIndex > 0 )
					return path.Substring( 0, startIndex + pattern.Length - 1 );
			}
			return "";
		}

		private void CheckMaterial()
		{
			if ( m_previewMat == null )
				m_previewMat = new Material( Shader.Find( "Hidden/Amplify Color/Preview" ) ) { hideFlags = HideFlags.HideAndDontSave };
		}

		private void DestroyMaterial()
		{
			if ( m_previewMat != null )
			{
				Material.DestroyImmediate( m_previewMat );
				m_previewMat = null;
			}
		}

		private void Initialize()
		{
			m_categories.Clear();
			m_previewImages.Clear();

			RefreshLibrary();
			CheckMaterial();

			WatcherPostprocessor.OnRefresh -= RefreshLibrary;
			WatcherPostprocessor.OnRefresh += RefreshLibrary;

			m_initialized = true;
		}

		private void Shutdown()
		{
			WatcherPostprocessor.OnRefresh -= RefreshLibrary;

			m_categories.Clear();
			m_previewImages.Clear();

			DestroyMaterial();
			Resources.UnloadUnusedAssets();
		}

		private void OnEnable()
		{
			Initialize();
		}

		private void OnDisable()
		{
			Shutdown();
		}

		public void OnLostFocus()
		{
			if ( !m_dragging )
				m_selectedLUT = null;

			m_dragging = false;
		}

		private void RefreshLUTLibrary( string[] guids )
		{
			foreach ( string guid in guids )
			{
				string path = AssetDatabase.GUIDToAssetPath( guid );
				if ( Path.GetExtension( path ).ToUpper() != ".PNG" )
					continue;

				TextureImporter importer = AssetImporter.GetAtPath( path ) as TextureImporter;
				if ( importer == null )
					continue;

				bool sizeCheck = ( importer.maxTextureSize == 1024 );
				bool filterCheck = ( importer.filterMode == FilterMode.Bilinear ) || ( importer.filterMode == ( FilterMode ) ( -1 ) );
				bool linearCheck = importer.linearTexture;
				bool mipCheck = !importer.mipmapEnabled;
				bool anisoCheck = ( importer.anisoLevel == 0 );
				bool formatCheck = ( importer.textureFormat == TextureImporterFormat.AutomaticTruecolor );
				bool wrapCheck = ( importer.wrapMode == TextureWrapMode.Clamp );
				bool typeCheck = ( importer.textureType == TextureImporterType.Advanced );

				if ( sizeCheck && filterCheck && linearCheck && mipCheck && anisoCheck && formatCheck && wrapCheck && typeCheck )
				{
					// strong candidate; final filter at draw
					string subfolder;
					if ( path.StartsWith( "Assets/" ) )
						subfolder = path.Substring( ( "Assets/" ).Length );
					else
						subfolder = path;

					int subfolderEndIndex = subfolder.LastIndexOfAny( new char[] { '/', '\\' } );

					string category;
					if ( subfolderEndIndex > 0 )
						category = subfolder.Substring( 0, subfolderEndIndex ).Replace( "/", " / " );
					else
						category = "Miscellaneous";

					if ( !string.IsNullOrEmpty( category ) )
					{
						HashSet<string> categorySet = null;
						if ( !m_categories.TryGetValue( category, out categorySet ) )
						{
							categorySet = new HashSet<string>();
							m_categories.Add( category, categorySet );
						}

						if ( !categorySet.Contains( path ) )
							categorySet.Add( path );
					}
				}
			}
		}

		private void RefreshPreviewLibrary( string[] guids )
		{
			string previewFolder = FindBaseFolder( guids, BaseSampleFolder );
			m_previewImages = new List<Texture2D>();

			if ( !string.IsNullOrEmpty( previewFolder ) )
			{
				string[] previewGuids = AssetDatabase.FindAssets( "t:Texture2D", new string[] { previewFolder } );

				foreach ( string guid in previewGuids )
				{
					string path = AssetDatabase.GUIDToAssetPath( guid );

					TextureImporter importer = AssetImporter.GetAtPath( path ) as TextureImporter;
					if ( importer.textureType != TextureImporterType.GUI || importer.linearTexture != false || importer.maxTextureSize != 512 ||
					    importer.textureFormat != TextureImporterFormat.AutomaticCompressed )
					{
						importer.textureType = TextureImporterType.GUI;
						importer.linearTexture = false;
						importer.maxTextureSize = 512;
						importer.textureFormat = TextureImporterFormat.AutomaticCompressed;
						AssetDatabase.ImportAsset( path );
					}

					m_previewImages.Add( AssetDatabase.LoadAssetAtPath<Texture2D>( path ) );
				}
			}
		}

		private void RefreshLibrary()
		{
			string[] guids = AssetDatabase.FindAssets( "t:Texture2D" );
			m_categories = new Dictionary<string, HashSet<string>>();

			RefreshLUTLibrary( guids );
			RefreshPreviewLibrary( guids );
		}

		private void UpdatePreview()
		{
			int previewIndex = m_selectedApply - 1;
			if ( m_selectedLUT != null && previewIndex >= 0 && m_previewCameras.Length > 0 )
			{
				Type actype = Type.GetType( "AmplifyColorEffect, Assembly-CSharp" );
				if ( actype != null )
				{
					var ac = m_previewCameras[ previewIndex ].GetComponent( actype );
					if ( ac != null && actype.GetField( "LutTexture" ).GetValue( ac ) != m_selectedLUT )
					{
						actype.GetField( "LutTexture" ).SetValue( ac, m_selectedLUT );
						UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
					}
				}
			}
		}

		private bool Foldout( string name )
		{
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();

			bool state = EditorPrefs.GetBool( "LUTBrowser.Foldout." + name, false );
			if ( GUILayout.Button( "", GUILayout.ExpandWidth( true ), GUILayout.Height( 20 ) ) )
				state = !state;

			GUILayout.Space( -20 );
			GUILayout.BeginHorizontal();

			GUILayout.Space( 10 );

			EditorGUILayout.Foldout( state, " " + name, m_styleFoldout );

			GUILayout.EndHorizontal();
			EditorPrefs.SetBool( "LUTBrowser.Foldout." + name, state );

			GUILayout.EndVertical();
			GUILayout.EndHorizontal();

			return state;
		}

		private bool DrawLibrary( float previewSize, bool showSource )
		{
			bool repaint = false;
			RenderTexture prevRT = RenderTexture.active;

			int sourceIndex = m_selectedPreview - m_previewImages.Count;
			Camera sourceCamera = ( sourceIndex >= 0 && m_previewCameras.Length > 0 ) ? m_previewCameras[ sourceIndex ] : null;

			bool linearPath = ( PlayerSettings.colorSpace == ColorSpace.Linear );
			RenderTexture sourceRT = null;
			RenderTexture gradedRT = RenderTexture.GetTemporary( 512, 256, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB );
			gradedRT.Create();

			if ( sourceCamera != null )
			{
				RenderTextureReadWrite rw = linearPath ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB;
				RenderTextureFormat fmt = sourceCamera.hdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32;

				sourceRT = RenderTexture.GetTemporary( 512, 256, 24, fmt, rw );
				sourceRT.Create();

				GameObject clonedCameraObj = Instantiate( sourceCamera.gameObject );
				clonedCameraObj.hideFlags = HideFlags.HideAndDontSave;

				Camera clonedCamera = clonedCameraObj.GetComponent<Camera>();
				clonedCamera.enabled = false;

				MonoBehaviour ac = clonedCameraObj.GetComponent( "AmplifyColorEffect" ) as MonoBehaviour;
				if ( ac != null )
					ac.enabled = false;

				clonedCamera.targetTexture = sourceRT;
				clonedCamera.Render();
				clonedCamera.targetTexture = null;

				DestroyImmediate( clonedCameraObj );
			}
			else if ( m_selectedPreview < m_previewImages.Count )
			{
				sourceRT = RenderTexture.GetTemporary( 512, 256, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB );
				sourceRT.Create();

				bool prevSRGB = GL.sRGBWrite;
				GL.sRGBWrite = true;
				Graphics.Blit( m_previewImages[ m_selectedPreview ], sourceRT, m_previewMat, 0 );
				GL.sRGBWrite = prevSRGB;
			}

			RenderTexture.active = prevRT;

			int thumbsPerLine = Mathf.FloorToInt( position.width / previewSize );
			while ( thumbsPerLine * ( previewSize + 4 ) > position.width )
				thumbsPerLine--;

			foreach ( KeyValuePair<string,HashSet<string>> pair in m_categories )
			{
				int lineCount = 0;

				if ( Foldout( pair.Key ) )
				{
					GUILayout.Space( 8 );
					GUILayout.BeginHorizontal();
					GUILayout.Space( 5 );
					foreach ( string path in pair.Value )
					{
						Texture2D lut = AssetDatabase.LoadAssetAtPath<Texture2D>( path );

						bool prevSRGB = GL.sRGBWrite;
						GL.sRGBWrite = true;
						prevRT = RenderTexture.active;
						if ( showSource && ( m_selectedLUT == null || lut == m_selectedLUT ) )
						{
							Graphics.Blit( sourceRT, gradedRT, m_previewMat, linearPath ? 1 : 0 );
						}
						else
						{
							m_previewMat.SetTexture( "_LUT", lut );
							Graphics.Blit( sourceRT, gradedRT, m_previewMat, linearPath ? 3 : 2 );
						}
						RenderTexture.active = prevRT;
						GL.sRGBWrite = prevSRGB;

						GUIStyle style = ( lut == m_selectedLUT ) ? m_stylePreviewFocus : m_stylePreview;
						GUILayout.Space( 3 );

						string name = lut.name.Replace( '_', ' ' );
						if ( GUILayout.RepeatButton( new GUIContent( name, gradedRT, name ), style, GUILayout.Width( previewSize ), GUILayout.Height( previewSize / 2 + 12 ) ) )
						{
							if ( m_selectedLUT != lut )
							{
								m_selectedLUT = lut;
								repaint = true;

								if ( Event.current.button == 1 )
									EditorGUIUtility.PingObject( lut );
							}
						}

						lineCount++;
						if ( lineCount >= thumbsPerLine )
						{
							GUILayout.FlexibleSpace();
							GUILayout.EndHorizontal();
							GUILayout.Space( 3 );
							GUILayout.BeginHorizontal();
							GUILayout.Space( 5 );
							lineCount = 0;
						}
					}
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					GUILayout.Space( 8 );
				}
			}

			if ( sourceRT != null )
				RenderTexture.ReleaseTemporary( sourceRT );
			if ( gradedRT != null )
				RenderTexture.ReleaseTemporary( gradedRT );

			return repaint;
		}

		void HandleDragAndDrop()
		{
			if ( Event.current.type == EventType.MouseDrag && !m_dragging )
			{
				if ( m_selectedLUT != null )
				{
					DragAndDrop.PrepareStartDrag();
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
					DragAndDrop.objectReferences = new UnityEngine.Object[] { m_selectedLUT };
					DragAndDrop.StartDrag( m_selectedLUT.name );
					DragAndDrop.SetGenericData( "LUTBrowser", this );
					Event.current.Use();
					m_dragging = true;
				}
			}

			if ( ( Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform ) && DragAndDrop.GetGenericData( "LUTBrowser" ) == this )
			{
				if ( m_selectedLUT != null )
					DragAndDrop.objectReferences = new UnityEngine.Object[] { m_selectedLUT };
			}

			if ( Event.current.type == EventType.DragExited || ( m_dragging && Event.current.type == EventType.MouseUp && Event.current.button == 0 ) )
			{
				m_dragging = false;
				DragAndDrop.SetGenericData( "LUTBrowser", null );
			}
		}

		void PrepareStyles()
		{
			m_styleFoldout = new GUIStyle( EditorStyles.foldout );

			m_styleFoldout.normal.textColor = EditorStyles.textField.normal.textColor;
			m_styleFoldout.onNormal.textColor = EditorStyles.textField.normal.textColor;
			m_styleFoldout.hover.textColor = EditorStyles.textField.normal.textColor;
			m_styleFoldout.onHover.textColor = EditorStyles.textField.normal.textColor;
			m_styleFoldout.focused.textColor = EditorStyles.textField.normal.textColor;
			m_styleFoldout.onFocused.textColor = EditorStyles.textField.normal.textColor;
			m_styleFoldout.active.textColor = EditorStyles.textField.normal.textColor;
			m_styleFoldout.onActive.textColor = EditorStyles.textField.normal.textColor;

			m_stylePreview = new GUIStyle( EditorStyles.label );
			m_stylePreview.fontSize = 9;
			m_stylePreview.alignment = TextAnchor.MiddleCenter;
			m_stylePreview.imagePosition = ImagePosition.ImageAbove;
			m_stylePreview.margin = new RectOffset( 0, 0, 0, 0 );
			m_stylePreview.padding = new RectOffset( 2, 2, 4, 2 );

			m_stylePreviewFocus = new GUIStyle( "SelectionRect" );
			m_stylePreviewFocus.fontSize = 9;
			m_stylePreviewFocus.alignment = TextAnchor.MiddleCenter;
			m_stylePreviewFocus.imagePosition = ImagePosition.ImageAbove;
			m_stylePreviewFocus.margin = m_stylePreview.margin;
			m_stylePreviewFocus.padding = m_stylePreview.padding;
		}

		private void OnGUI()
		{
			if ( !m_initialized )
				Initialize();

			CheckMaterial();
			PrepareStyles();

			bool showSource = false;

			GUILayout.BeginHorizontal( EditorStyles.toolbar );
			{
				List<string> previewCameraNames = new List<string>();
				previewCameraNames.Add( "None" );

				List<string> previewSourceNames = new List<string>();
				foreach ( Texture2D tex in m_previewImages )
					previewSourceNames.Add( tex.name.Replace( '_', ' ' ) );

				m_previewCameras = GameObject.FindObjectsOfType<Camera>();
				foreach ( Camera cam in m_previewCameras )
				{
					previewCameraNames.Add( cam.name );
					previewSourceNames.Add( cam.name );
				}

				if ( m_selectedApply > m_previewCameras.Length )
					m_selectedApply = 0;

				if ( m_selectedPreview >= previewSourceNames.Count )
					m_selectedPreview = 0;

				GUILayout.Label( "Apply", EditorStyles.toolbarButton, GUILayout.MaxWidth( 40 ) );
				int selectedPreview = EditorGUILayout.Popup( m_selectedApply, previewCameraNames.ToArray(), EditorStyles.toolbarPopup, GUILayout.MaxWidth( 120 ) );
				if ( selectedPreview != m_selectedApply )
				{
					m_selectedApply = selectedPreview;
					UpdatePreview();
				}

				GUILayout.FlexibleSpace();

				GUILayout.Label( "Preview", EditorStyles.toolbarButton, GUILayout.MaxWidth( 50 ) );
				m_selectedPreview = EditorGUILayout.Popup( m_selectedPreview, previewSourceNames.ToArray(), EditorStyles.toolbarPopup, GUILayout.MaxWidth( 120 ) );
				if ( GUILayout.RepeatButton( "<<", EditorStyles.toolbarButton, GUILayout.MaxWidth( 22 ) ) )
					showSource = true;

				GUILayout.BeginVertical();
				GUILayout.Space( 0 );
				m_previewSize = GUILayout.HorizontalSlider( m_previewSize, MinPreviewSize, MaxPreviewSize, GUILayout.MaxWidth( 100 ) );
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();

			m_scrollPos = GUILayout.BeginScrollView( m_scrollPos );
			GUILayout.BeginVertical();
			{
				if ( DrawLibrary( m_previewSize, showSource ) )
				{
					UpdatePreview();
					this.Repaint();
				}
			}
			GUILayout.EndVertical();
			GUILayout.EndScrollView();

			HandleDragAndDrop();
		}
	}
}
