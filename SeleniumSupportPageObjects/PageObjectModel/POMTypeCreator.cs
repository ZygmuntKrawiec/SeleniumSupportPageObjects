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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
