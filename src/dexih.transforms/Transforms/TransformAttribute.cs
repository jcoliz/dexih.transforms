﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace dexih.transforms.Transforms
{
    [Serializable]
    public class TransformAttribute: Attribute
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ETransformType
        {
            Mapping, Filter, Sort, Group, Aggregate, Series, Join, Rows, Lookup, Validation, Delta, Concatenate, Profile, Internal
        }
        
        public ETransformType TransformType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
}