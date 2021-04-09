﻿// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Retouch_Photo2.Adjustments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a IAdjustment from the string.
        /// </summary>    
        /// <param name="assemblyType"> The type for assembly. </param>
        /// <param name="type"> The source string. </param>
        /// <returns> The created IAdjustment. </returns>
        private static IAdjustment CreateAdjustment(Type assemblyType, string type)
        {
            if (string.IsNullOrEmpty(type) == false)
            {
                Assembly assembly = assemblyType.GetTypeInfo().Assembly;
                IEnumerable<TypeInfo> typeInfos = assembly.DefinedTypes;

                TypeInfo typeInfo = typeInfos.FirstOrDefault(t => t.FullName == $"Retouch_Photo2.Adjustments.Models.{type}Adjustment");
                if (typeInfo != null)
                {
                    object obj = Activator.CreateInstance(typeInfo.AsType());
                    if (obj is IAdjustment adjustment)
                    {
                        return adjustment;
                    }
                }
            }

            return new GrayAdjustment();
        }

    }
}