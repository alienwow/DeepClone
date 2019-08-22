﻿using Natasha;
using System;
using System.Reflection;

namespace DeepClone.Model
{
    /// <summary>
    /// 构建信息
    /// </summary>
    public class BuilderInfo
    {

        public Type DeclaringType;
        public string DeclaringTypeName;
        public string DeclaringAvailableName;


        public Type MemberType;
        public string MemberTypeName;
        public string MemberTypeAvailableName;
        public string MemberName;


        public Type ElementType;
        public string ElementTypeName;
        public string ElementTypeAvailableName;


        public Type ArrayBaseType;
        public string ArrayBaseTypeName;
        public string ArrayBaseTypeAvaliableName;


        public int ArrayLayer;
        public int ArrayDimensions;


        public bool IsStatic;


        public string StaticName;

        public static implicit operator BuilderInfo(MemberInfo info)
        {
            if (info.MemberType == MemberTypes.Field)
            {

                var tempInfo = (FieldInfo)(info);
                var instance = new BuilderInfo
                {

                    MemberName = tempInfo.Name,
                    MemberType = tempInfo.FieldType,
                    MemberTypeName = tempInfo.FieldType.GetDevelopName(),
                    MemberTypeAvailableName = tempInfo.FieldType.GetAvailableName(),

                };


                HandlerDeclaringType(instance, tempInfo.DeclaringType);
                HandlerArrayType(instance, tempInfo.FieldType);


                if (tempInfo.IsStatic)
                {

                    instance.IsStatic = true;
                    instance.StaticName = $"{instance.DeclaringTypeName}";

                }
                return instance;

            }
            else if (info.MemberType == MemberTypes.Property)
            {

                var tempInfo = (PropertyInfo)(info);
                var instance = new BuilderInfo
                {

                    MemberName = tempInfo.Name,
                    MemberType = tempInfo.PropertyType,
                    MemberTypeName = tempInfo.PropertyType.GetDevelopName(),
                    MemberTypeAvailableName = tempInfo.PropertyType.GetAvailableName(),

                };


                HandlerDeclaringType(instance, tempInfo.DeclaringType);
                HandlerArrayType(instance, tempInfo.PropertyType);


                if (tempInfo.GetGetMethod(true).IsStatic)
                {

                    instance.IsStatic = true;
                    instance.StaticName = $"{instance.DeclaringTypeName}";

                }
                return instance;

            }

            return null;
        }




        public static implicit operator BuilderInfo(Type type)
        {

            var instance = new BuilderInfo();
            HandlerDeclaringType(instance, type);
            HandlerArrayType(instance, type);
            return instance;

        }




        public static BuilderInfo HandlerArrayType(BuilderInfo instance, Type type)
        {

            if (type.IsArray)
            {

                Type temp = type;
                instance.ArrayBaseType = type.GetElementType();
                instance.ArrayBaseTypeName = instance.ArrayBaseType.GetDevelopName();
                instance.ArrayBaseTypeAvaliableName = instance.ArrayBaseType.GetAvailableName();


                int count = 0;
                while (temp.HasElementType)
                {

                    count++;
                    temp = temp.GetElementType();

                }
                instance.ElementType = temp;
                instance.ArrayLayer = count;


                var ctor = type.GetConstructors()[0];
                instance.ArrayDimensions = ctor.GetParameters().Length;


                instance.ElementTypeName = instance.ElementType.GetDevelopName();
                instance.ElementTypeAvailableName = instance.ElementType.GetAvailableName();

            }

            return instance;

        }




        public static BuilderInfo HandlerDeclaringType(BuilderInfo instance, Type type)
        {

            instance.DeclaringType = type;
            instance.DeclaringTypeName = type.GetDevelopName();
            instance.DeclaringAvailableName = type.GetAvailableName();
            return instance;

        }

    }

}
