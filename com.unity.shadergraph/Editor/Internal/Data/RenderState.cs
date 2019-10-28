﻿using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.ShaderGraph.Internal
{
    public class RenderState
    {
        public enum Type
        {
            Cull,
            Blend,
            BlendOp,
            ZTest,
            ZWrite,
            ColorMask,
            ZClip,
            Stencil,
        }

        public Type type { get; }
        public string value { get; }

        RenderState(Type type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public static RenderState Cull(Cull value)
        {
            return new RenderState(Type.Cull, $"Cull {value}");
        }

        public static RenderState Cull(string value)
        {
            return new RenderState(Type.Cull, $"Cull {value}");
        }

        public static RenderState Blend(Blend src, Blend dst)
        {
            return new RenderState(Type.Blend, $"Blend {src} {dst}");
        }

        public static RenderState Blend(string src, string dst)
        {
            return new RenderState(Type.Blend, $"Blend {src} {dst}");
        }

        public static RenderState Blend(Blend src, Blend dst, Blend alphaSrc, Blend alphaDst)
        {
            return new RenderState(Type.Blend, $"Blend {src} {dst}, {alphaSrc} {alphaDst}");
        }

        public static RenderState Blend(string src, string dst, string alphaSrc, string alphaDst)
        {
            return new RenderState(Type.Blend, $"Blend {src} {dst}, {alphaSrc} {alphaDst}");
        }

        public static RenderState Blend(string value)
        {
            return new RenderState(Type.Blend, value);
        }

        public static RenderState BlendOp(BlendOp op)
        {
            return new RenderState(Type.BlendOp, $"BlendOp {op}");
        }

        public static RenderState BlendOp(string op)
        {
            return new RenderState(Type.BlendOp, $"BlendOp {op}");
        }

        public static RenderState BlendOp(BlendOp op, BlendOp opAlpha)
        {
            return new RenderState(Type.BlendOp, $"BlendOp {op}, {opAlpha}");
        }

        public static RenderState BlendOp(string op, string opAlpha)
        {
            return new RenderState(Type.BlendOp, $"BlendOp {op}, {opAlpha}");
        }

        public static RenderState ZTest(ZTest value)
        {
            return new RenderState(Type.ZTest, $"ZTest {value}");
        }

        public static RenderState ZTest(string value)
        {
            return new RenderState(Type.ZTest, $"ZTest {value}");
        }

        public static RenderState ZWrite(ZWrite value)
        {
            return new RenderState(Type.ZWrite, $"ZWrite {value}");
        }

        public static RenderState ZWrite(string value)
        {
            return new RenderState(Type.ZWrite, $"ZWrite {value}");
        }

        public static RenderState ZClip(string value)
        {
            return new RenderState(Type.ZClip, $"ZClip {value}");
        }

        public static RenderState ColorMask(string value)
        {
            return new RenderState(Type.ColorMask, $"{value}");
        }        

        public static RenderState Stencil(Stencil value)
        {
            return new RenderState(Type.Stencil, value.ToShaderString());
        }
    }

    public class RenderStateCollection : IEnumerable<ConditionalRenderState>
    {
        private readonly List<ConditionalRenderState> m_RenderStates;

        public RenderStateCollection()
        {
            m_RenderStates = new List<ConditionalRenderState>();
        }

        public void Add(RenderState renderState)
        {
            m_RenderStates.Add(new ConditionalRenderState(renderState, null));
        }

        public void Add(RenderState renderState, FieldCondition fieldCondition)
        {
            m_RenderStates.Add(new ConditionalRenderState(renderState, new FieldCondition[]{ fieldCondition }));
        }

        public void Add(RenderState renderState, FieldCondition[] fieldConditions)
        {
            m_RenderStates.Add(new ConditionalRenderState(renderState, fieldConditions));
        }

        public IEnumerator<ConditionalRenderState> GetEnumerator()
        {
            return m_RenderStates.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ConditionalRenderState : IConditionalShaderString
    {        
        public RenderState renderState { get; }
        public FieldCondition[] fieldConditions { get; }
        public string value => renderState.value;

        public ConditionalRenderState(RenderState renderState, FieldCondition[] fieldConditions)
        {
            this.renderState = renderState;
            this.fieldConditions = fieldConditions;
        }
    }
}
