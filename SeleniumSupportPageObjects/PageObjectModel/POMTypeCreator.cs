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


        public void Dispose()
        {
            throw new NotImplementedException();
        }       
    }
}
