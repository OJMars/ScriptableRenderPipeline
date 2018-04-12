using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.VFX.Operator
{
    [VFXInfo(category = "Math/Arithmetic")]
    class Step : VFXOperatorFloatUnifiedWithVariadicOutput
    {
        override public string name { get { return "Step"; } }

        public class InputProperties
        {
            [Tooltip("The value to compare")]
            public FloatN Value = 0.0f;
            [Tooltip("The threshold from which the function will return one")]
            public FloatN Threshold = 0.5f;
        }

        override protected VFXExpression[] BuildExpression(VFXExpression[] inputExpression)
        {
            int size = VFXExpression.TypeToSize(inputExpression[0].valueType);

            return new[] {
                VFXOperatorUtility.Saturate(VFXOperatorUtility.Ceil(inputExpression[0] - inputExpression[1])),

                // TODO : It would be nice to have inverted step output (1 if below threshold), but we need to be able to define multiple FloatN output slots.
                //VFXOperatorUtility.Clamp( new VFXExpressionFloor(inputExpression[0])-inputExpression[1], VFXValue.Constant(0.0f), VFXValue.Constant(1.0f)),
            };
        }
    }
}
