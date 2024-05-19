﻿using System.Reflection;

namespace Net8_JWT.WebAPI.Utilities
{
    public static class Mapper
    {
        /*passing value to target*/
        public static TTarget Map<TSource, TTarget>(this TSource source)
        {
            var target = Activator.CreateInstance<TTarget>();
            if (source == null) return target;
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

            /* find fields */
            var srcFields = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                             where aProp.CanRead     //check if prop is readable
                             select new
                             {
                                 Name = aProp.Name,
                                 Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                             }).ToList();
            var trgFields = (from PropertyInfo aProp in target.GetType().GetProperties(flags)
                             where aProp.CanWrite   //check if prop is writeable
                             select new
                             {
                                 Name = aProp.Name,
                                 Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                             }).ToList();

            /* common fields where name and type same*/
            var commonFields = srcFields.Intersect(trgFields).ToList();

            /* assign values */
            foreach (var aField in commonFields)
            {
                var value = source.GetType().GetProperty(aField.Name).GetValue(source, null);
                PropertyInfo propertyInfos = target.GetType().GetProperty(aField.Name);
                propertyInfos.SetValue(target, value, null);
            }
            return target;
        }
    }
}
