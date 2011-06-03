// Type: System.Runtime.Serialization.DataContractSerializer
// Assembly: System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Runtime.Serialization.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace System.Runtime.Serialization
{
    public sealed class DataContractSerializer : XmlObjectSerializer
    {
        #region Constructors and Destructors

        public DataContractSerializer(Type type);

        public DataContractSerializer(Type type, IEnumerable<Type> knownTypes);

        public DataContractSerializer(
            Type type,
            IEnumerable<Type> knownTypes,
            int maxItemsInObjectGraph,
            bool ignoreExtensionDataObject,
            bool preserveObjectReferences,
            IDataContractSurrogate dataContractSurrogate);

        public DataContractSerializer(
            Type type,
            IEnumerable<Type> knownTypes,
            int maxItemsInObjectGraph,
            bool ignoreExtensionDataObject,
            bool preserveObjectReferences,
            IDataContractSurrogate dataContractSurrogate,
            DataContractResolver dataContractResolver);

        public DataContractSerializer(Type type, string rootName, string rootNamespace);

        public DataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes);

        public DataContractSerializer(
            Type type,
            string rootName,
            string rootNamespace,
            IEnumerable<Type> knownTypes,
            int maxItemsInObjectGraph,
            bool ignoreExtensionDataObject,
            bool preserveObjectReferences,
            IDataContractSurrogate dataContractSurrogate);

        public DataContractSerializer(
            Type type,
            string rootName,
            string rootNamespace,
            IEnumerable<Type> knownTypes,
            int maxItemsInObjectGraph,
            bool ignoreExtensionDataObject,
            bool preserveObjectReferences,
            IDataContractSurrogate dataContractSurrogate,
            DataContractResolver dataContractResolver);

        public DataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace);

        public DataContractSerializer(
            Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes);

        public DataContractSerializer(
            Type type,
            XmlDictionaryString rootName,
            XmlDictionaryString rootNamespace,
            IEnumerable<Type> knownTypes,
            int maxItemsInObjectGraph,
            bool ignoreExtensionDataObject,
            bool preserveObjectReferences,
            IDataContractSurrogate dataContractSurrogate);

        public DataContractSerializer(
            Type type,
            XmlDictionaryString rootName,
            XmlDictionaryString rootNamespace,
            IEnumerable<Type> knownTypes,
            int maxItemsInObjectGraph,
            bool ignoreExtensionDataObject,
            bool preserveObjectReferences,
            IDataContractSurrogate dataContractSurrogate,
            DataContractResolver dataContractResolver);

        #endregion

        #region Properties

        public DataContractResolver DataContractResolver { get; }

        public IDataContractSurrogate DataContractSurrogate { get; }

        public bool IgnoreExtensionDataObject { get; }

        public ReadOnlyCollection<Type> KnownTypes { get; }

        public int MaxItemsInObjectGraph { get; }

        public bool PreserveObjectReferences { get; }

        #endregion

        #region Public Methods

        public override bool IsStartObject(XmlReader reader);

        public override bool IsStartObject(XmlDictionaryReader reader);

        public override object ReadObject(XmlReader reader);

        public override object ReadObject(XmlReader reader, bool verifyObjectName);

        public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName);

        public object ReadObject(
            XmlDictionaryReader reader, bool verifyObjectName, DataContractResolver dataContractResolver);

        public override void WriteEndObject(XmlWriter writer);

        public override void WriteEndObject(XmlDictionaryWriter writer);

        public override void WriteObject(XmlWriter writer, object graph);

        public void WriteObject(XmlDictionaryWriter writer, object graph, DataContractResolver dataContractResolver);

        public override void WriteObjectContent(XmlWriter writer, object graph);

        public override void WriteObjectContent(XmlDictionaryWriter writer, object graph);

        public override void WriteStartObject(XmlWriter writer, object graph);

        public override void WriteStartObject(XmlDictionaryWriter writer, object graph);

        #endregion
    }
}
