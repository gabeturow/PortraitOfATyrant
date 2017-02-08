﻿using UnityEngine;
using System;

namespace Slate{

	///Attribute to mark a field or property as an animatable parameter
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class AnimatableParameterAttribute : PropertyAttribute{
		public string link;
		public float? min;
		public float? max;
		public AnimatableParameterAttribute(){}
		public AnimatableParameterAttribute(float min, float max){
			this.min = min;
			this.max = max;
		}
	}

	///Attribute used to show a popup of shader properties of type
	[AttributeUsage(AttributeTargets.Field)]
	public class ShaderPropertyPopupAttribute : PropertyAttribute{
		public Type propertyType;
		public ShaderPropertyPopupAttribute(){}
		public ShaderPropertyPopupAttribute(Type propertyType){
			this.propertyType = propertyType;
		}
	}

    ///Attribute used to restrict float or int to a min value
    [AttributeUsage(AttributeTargets.Field)]
    public class MinAttribute : PropertyAttribute{
        public float min;
        public MinAttribute(float min){
            this.min = min;
        }
    }

    ///Attribute used on Object or string field to mark them as required (red) if not set
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : PropertyAttribute{}

	///Used for a sorting layer popup
	[AttributeUsage(AttributeTargets.Field)]
	public class SortingLayerAttribute : PropertyAttribute { }

}