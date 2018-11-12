using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumSupportPageObjects.PageObjectModel
{
    public class POMTypeCreator : IDisposable
    {
        private static AssemblyName _assemblyName;
        private static AssemblyBuilder _assemblyBuilder;
        private static ModuleBuilder _moduleBuilder;
        private static List<Type> _listOfAllTypes;

        private TypeBuilder _typeBulider;
        private ConstructorBuilder _ctor0;
        private Type _createdType;
        private List<PropertiesFeaturesContainer> _listOfProperties;

        static POMTypeCreator()
        {
            _listOfAllTypes = new List<Type>();
        }

        public POMTypeCreator(string newDomainName, string newTypeName, string newAssemblyName = "DefaultName")
        {
            if (_assemblyName == null)
            {
                _assemblyName = new AssemblyName(newAssemblyName);
            }

            if (_assemblyBuilder == null)
            {
                POMTypeCreator._assemblyBuilder = AppDomain.CurrentDomain//CreateDomain(newDomainName)
                                                          .DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.RunAndSave);
            }

            if (_moduleBuilder == null)
            {
                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyName.Name, _assemblyName.Name + ".dll");
            }

            _typeBulider = _moduleBuilder.DefineType(newTypeName, TypeAttributes.Public);

            _listOfProperties = new List<PropertiesFeaturesContainer>();
            // Create default ctor for new type.
            createDefaultConstructor();
        }

        private void createDefaultConstructor()
        {
            _ctor0 = _typeBulider.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
            ILGenerator ctorIL = _ctor0.GetILGenerator();
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ret);
        }

        public void Add(string propertyName, How how, string locator)
        {
            var PFContainer = new PropertiesFeaturesContainer()
            {
                PropertyName = propertyName,
                How = how,
                Locator = locator,
            };

            _listOfProperties.Add(PFContainer);
        }

        public void CreateType()
        {
            if (_listOfProperties.Count == 0)
            {
                return;
            }

            foreach (var pFContainer in _listOfProperties)
            {
                generateNewProperty(pFContainer);
            }
            _createdType = _typeBulider.CreateType();
            _listOfAllTypes.Add(_createdType);
        }

        private void generateNewProperty(PropertiesFeaturesContainer propertyFeatureContainer)
        {
            FieldBuilder newField = _typeBulider.DefineField("_" + propertyFeatureContainer.PropertyName, typeof(IWebElement), FieldAttributes.Private);
            PropertyBuilder newProperty = _typeBulider.DefineProperty(
                                                        propertyFeatureContainer.PropertyName,
                                                        PropertyAttributes.HasDefault,
                                                        typeof(IWebElement),
                                                        null);

            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            MethodBuilder getNewProperty = _typeBulider.DefineMethod(
                                                        "get_" + propertyFeatureContainer.PropertyName,
                                                        getSetAttr,
                                                        typeof(IWebElement),
                                                        Type.EmptyTypes);

            ILGenerator getIL = getNewProperty.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, newField);
            getIL.Emit(OpCodes.Ret);

            var parameterType = new Type[] { typeof(IWebElement) };
            MethodBuilder setNewProperty = _typeBulider.DefineMethod(
                                                        "set_" + propertyFeatureContainer.PropertyName,
                                                        getSetAttr,
                                                        null,
                                                        parameterType);

            ILGenerator setIL = setNewProperty.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, newField);
            setIL.Emit(OpCodes.Ret);

            newProperty.SetGetMethod(getNewProperty);
            newProperty.SetSetMethod(setNewProperty);

            ConstructorInfo ctorFindsByInfo = typeof(FindsByAttribute).GetConstructor(new Type[] { typeof(How), typeof(string) });
            CustomAttributeBuilder newFindsByAttribute = new CustomAttributeBuilder(ctorFindsByInfo, new object[] { propertyFeatureContainer.How, propertyFeatureContainer.Locator });
            newProperty.SetCustomAttribute(newFindsByAttribute);

            ConstructorInfo ctorCacheLookupInfo = typeof(CacheLookupAttribute).GetConstructor(Type.EmptyTypes);
            CustomAttributeBuilder newCacheLookup = new CustomAttributeBuilder(ctorCacheLookupInfo, Type.EmptyTypes);
            newProperty.SetCustomAttribute(newCacheLookup);
        }

        public void Dispose()
        {
            _typeBulider = null;
            _ctor0 = null;
            _createdType = null;
            _listOfProperties = null;
        }

        public static void SaveAssembly()
        {
            _assemblyBuilder.Save(_assemblyName.Name + ".dll");
            _assemblyName = null;
            _assemblyBuilder = null;
            _moduleBuilder = null;
            _listOfAllTypes.Clear();
        }
    }
}
