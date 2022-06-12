using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Common
{
    public static class ObjectExtension
    {
        public static void CloneObject<TSource, TDestination>(TSource source, ref TDestination dest)
        {
            var sourceProperties = source.GetType().GetProperties();
            var destProperties = dest.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var destProperty in destProperties)
                {
                    if (destProperty.Name != "ID" && destProperty.Name != "Created" && destProperty.Name != "Updated" && destProperty.Name != "CreatedBy" && destProperty.Name != "UpdatedBy")
                    {
                        if (sourceProperty.Name == destProperty.Name)
                        {
                            destProperty.SetValue(dest, sourceProperty.GetValue(source));
                            break;
                        }
                    }
                }
            }
        }

        //public static void CloneObject(object objSource, object objDestination)
        //{
        //    var destProps = objDestination.GetType().GetProperties();
        //    foreach (var sourceProp in objSource.GetType().GetProperties())
        //    {
        //        foreach (var item in destProps)
        //        {
        //            if (item.Name != "ID" && item.Name == "Created" && item.Name == "Updated" && item.Name == "CreatedBy" && item.Name == "UpdatedBy")
        //            {
        //                destProps.GetType().GetProperty(item.Name).SetValue(destProps, item.GetValue(objDestination, null), null);
        //            }
        //        }
        //    }
        //}
    }
}
