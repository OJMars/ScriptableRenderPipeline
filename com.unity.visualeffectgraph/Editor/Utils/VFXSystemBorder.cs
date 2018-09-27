using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;
using System;

using UnityObject = UnityEngine.Object;

namespace UnityEditor.VFX.UI
{
    public class VFXContextBorderFactory : UxmlFactory<VFXContextBorder>
    {}

    [InitializeOnLoad]
    public class VFXContextBorder : VisualElement, IDisposable
    {
        Material m_Mat;

        static Mesh s_Mesh;

        public VFXContextBorder()
        {
            RecreateResources();
        }

        void RecreateResources()
        {
            if (s_Mesh == null)
            {
                s_Mesh = new Mesh();
                int verticeCount = 20;

                var vertices = new Vector3[verticeCount];
                var uvsBorder = new Vector2[verticeCount];
                var uvsDistance = new Vector2[verticeCount];

                for (int ix = 0; ix < 4; ++ix)
                {
                    for (int iy = 0; iy < 4 ; ++iy)
                    {
                        vertices[ix + iy * 4] = new Vector3(ix < 2 ? -1 : 1, iy < 2 ? -1 : 1, 0);
                        uvsBorder[ix + iy * 4] = new Vector2(ix == 0 || ix == 3 ? 1 : 0, iy == 0 || iy == 3 ? 1 : 0);
                        uvsDistance[ix + iy * 4] = new Vector2(iy < 2 ? ix / 2 : 2 - ix / 2, iy < 2 ? 0 : 1);
                    }
                }

                for(int i = 16; i < 20; ++i)
                {
                    vertices[i] = vertices[i - 16];
                    uvsBorder[i] = uvsBorder[i - 16];
                    uvsDistance[i] = new Vector2(2, 2);
                }

                vertices[16] = vertices[0];
                vertices[17] = vertices[1];
                vertices[18] = vertices[4];
                vertices[19] = vertices[5];

                uvsBorder[16] = uvsBorder[0];
                uvsBorder[17] = uvsBorder[1];
                uvsBorder[18] = uvsBorder[4];
                uvsBorder[19] = uvsBorder[5];

                uvsDistance[16] = new Vector2(2, 2);
                uvsDistance[17] = new Vector2(2, 2);
                uvsDistance[18] = new Vector2(2, 2);
                uvsDistance[19] = new Vector2(2, 2);

                var indices = new int[4 * 8];

                for (int ix = 0; ix < 3; ++ix)
                {
                    for (int iy = 0; iy < 3; ++iy)
                    {
                        int quadIndex = (ix + iy * 3);
                        if (quadIndex == 4)
                            continue;
                        else if (quadIndex > 4)
                            --quadIndex;
                        int vertIndex = quadIndex * 4;
                        
                        

                        indices[vertIndex] = ix + iy * 4;
                        indices[vertIndex + 1] = ix + (iy + 1) * 4;
                        indices[vertIndex + 2] = ix + 1 + (iy + 1) * 4;
                        indices[vertIndex + 3] = ix + 1 + iy * 4;
                        if (quadIndex == 3)
                        {
                            indices[vertIndex] = 18;
                            indices[vertIndex + 3] = 19;
                        }
                    }
                }

                s_Mesh.vertices = vertices;
                s_Mesh.uv = uvsBorder;
                s_Mesh.uv2 = uvsDistance;
                s_Mesh.SetIndices(indices, MeshTopology.Quads, 0);
            }

            m_Mat = new Material(Shader.Find("Hidden/VFX/GradientBorder"));
        }

        void IDisposable.Dispose()
        {
            UnityObject.DestroyImmediate(m_Mat);
        }

        StyleValue<Color> m_StartColor;
        public Color startColor
        {
            get
            {
                return m_StartColor.GetSpecifiedValueOrDefault(Color.black);
            }
            set
            {
                m_StartColor = value;
            }
        }
        StyleValue<Color> m_EndColor;
        public Color endColor
        {
            get
            {
                return m_EndColor.GetSpecifiedValueOrDefault(Color.black);
            }
            set
            {
                m_EndColor = value;
            }
        }

        protected override void OnStyleResolved(ICustomStyle styles)
        {
            base.OnStyleResolved(styles);

            styles.ApplyCustomProperty("start-color", ref m_StartColor);
            styles.ApplyCustomProperty("end-color", ref m_EndColor);
        }

        protected override void DoRepaint(IStylePainter sp)
        {
            RecreateResources();
            VFXView view = GetFirstAncestorOfType<VFXView>();
            if (view != null && m_Mat != null)
            {
                float radius = style.borderRadius;

                float realBorder = style.borderLeftWidth.value * view.scale;

                Vector4 size = new Vector4(layout.width * .5f, layout.height * 0.5f, 0, 0);
                m_Mat.SetVector("_Size", size);
                m_Mat.SetFloat("_Border", realBorder < 1.75f ?  1.75f / view.scale : style.borderLeftWidth.value);
                m_Mat.SetFloat("_Radius", radius);

                m_Mat.SetColor("_ColorStart", (QualitySettings.activeColorSpace == ColorSpace.Linear) ? startColor.gamma : startColor);
                m_Mat.SetColor("_ColorEnd", (QualitySettings.activeColorSpace == ColorSpace.Linear) ? endColor.gamma : endColor);

                m_Mat.SetPass(0);

                Graphics.DrawMeshNow(s_Mesh, Matrix4x4.Translate(new Vector3(size.x, size.y, 0)));
            }
        }
    }
}
