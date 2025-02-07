#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace MyBox.Internal
{
	[InitializeOnLoad]
	public class MustBeAssignedConditionalFieldExclude
	{
		static MustBeAssignedConditionalFieldExclude()
		{
			MustBeAssignedAttributeChecker.ExcludeFieldFilter += ExcludeCheckIfConditionalFieldHidden;
		}
		
		private static readonly Type _conditionallyVisibleType = typeof(ConditionalFieldAttribute);
		
		private static bool ExcludeCheckIfConditionalFieldHidden(FieldInfo field, UnityEngine.Object obj)
		{
			if (_conditionallyVisibleType == null) return false;
			if (!field.IsDefined(_conditionallyVisibleType, false)) return false;

			// Get a specific attribute of this field
			var conditionalFieldAttribute = field.GetCustomAttributes(_conditionallyVisibleType, false)
				.Select(a => a as ConditionalFieldAttribute)
				.SingleOrDefault();

			return conditionalFieldAttribute != null &&
			       !ConditionalFieldUtility.BehaviourPropertyIsVisible(obj, field.Name, conditionalFieldAttribute);
		}
	}
}
#endif