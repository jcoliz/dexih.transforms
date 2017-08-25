﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace dexih.transforms
{
    /// <summary>
    /// Maps a DbDataReader row to an object.
    /// </summary>
    /// <typeparam name="T">Object Type to Map</typeparam>
    public class PocoMapper<T>
    {
        private readonly DbDataReader _reader;
        private readonly List<(PropertyInfo propertyInfo, int ordinal)> _fieldMappings;
        
        public PocoMapper(DbDataReader reader)
        {
            _reader = reader;
            
            _fieldMappings = new List<(PropertyInfo propertyInfo, int ordinal)>();
            //Create a list of properties in the type T, and match them to ordinals in the inputReader.
            var properties = typeof(T).GetProperties();
            foreach (var propertyInfo in properties)
            {
                // if the property has a field attribute, use this as the reference in the reader
                var field = propertyInfo.GetCustomAttribute<FieldAttribute>(false);
                var name = field == null ? propertyInfo.Name : field.Name;
                var ordinal = reader.GetOrdinal(name);
                if (ordinal >= 0)
                {
                    _fieldMappings.Add((propertyInfo, ordinal));
                }
            }
        }

        /// <summary>
        /// Get the object using the current item in the DbDataReader
        /// </summary>
        /// <returns>Object</returns>
        public T GetItem()
        {
            var item = (T) Activator.CreateInstance(typeof(T));

            foreach (var mapping in _fieldMappings)
            {
                var value = _reader[mapping.ordinal];
                
                if (mapping.propertyInfo.PropertyType.GetTypeInfo().IsEnum && value is string)
                {
                    value = Enum.Parse(mapping.propertyInfo.PropertyType, (string)value);
                }
                mapping.Item1.SetValue(item, value is DBNull ? null : value);
            }

            return item;
        }
    }
}