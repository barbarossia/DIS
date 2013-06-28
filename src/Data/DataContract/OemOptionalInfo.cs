//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace DIS.Data.DataContract
{
    public class OemOptionalInfo : IXmlSerializable
    {
        private const string zPcModelSkuName = "ZPC_MODEL_SKU";
        private const string zOemExtIdName = "ZOEM_EXT_ID";
        private const string zManufGeoLocName = "ZMANUF_GEO_LOC";
        private const string zPgmEligValuesName = "ZPGM_ELIG_VAL";
        private const string zChannelRelIdName = "ZCHANNEL_REL_ID";

        private const string rootXmlElementName = "OEMOptionalInfo";
        private const string nameXmlElementName = "Name";
        private const string valueXmlElementName = "Value";
        private const string fieldXmlElementName = "Field";
        private const string NumbericRegEx = "^[0-9]+$";

        private XElement xml
        {
            get
            {
                return new XElement(rootXmlElementName,
                    from p in Values
                    where !string.IsNullOrEmpty(p.Value)
                    select new XElement(fieldXmlElementName,
                        new XElement(nameXmlElementName, p.Key),
                        new XElement(valueXmlElementName, p.Value)));
            }
            set
            {
                List<XElement> fields = value.Elements(fieldXmlElementName).ToList();
                foreach (XElement f in fields)
                {
                    SetField(f.Element(nameXmlElementName).Value, f.Element(valueXmlElementName).Value);
                }
            }
        }

        public string ZPC_MODEL_SKU
        {
            get
            {
                return Values[zPcModelSkuName];
            }
            set
            {
                SetField(zPcModelSkuName, value);
            }
        }

        public string ZOEM_EXT_ID
        {
            get
            {
                return Values[zOemExtIdName];
            }
            set
            {
                SetField(zOemExtIdName, value);
            }
        }

        public string ZMANUF_GEO_LOC
        {
            get
            {
                return Values[zManufGeoLocName];
            }
            set
            {
                SetField(zManufGeoLocName, value);
            }
        }

        public string ZPGM_ELIG_VALUES
        {
            get
            {
                return Values[zPgmEligValuesName];
            }
            set
            {
                SetField(zPgmEligValuesName, value);
            }
        }

        public string ZCHANNEL_REL_ID
        {
            get
            {
                return Values[zChannelRelIdName];
            }
            set
            {
                SetField(zChannelRelIdName, value);
            }
        }

        public Dictionary<string, string> Values { get; private set; }

        public OemOptionalInfo()
            : this(null, null, null, null, null)
        {
        }

        public OemOptionalInfo(string xml)
            : this(null, null, null, null, null)
        {
            if (!string.IsNullOrEmpty(xml))
                this.xml = XElement.Parse(xml);
        }

        public OemOptionalInfo(string zPcModelSku, string zOemExtId,
            string zManufGeoLoc, string zPgmEligValues, string zChannelRelId)
        {
            Values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            ZPC_MODEL_SKU = zPcModelSku;
            ZOEM_EXT_ID = zOemExtId;
            ZMANUF_GEO_LOC = zManufGeoLoc;
            ZPGM_ELIG_VALUES = zPgmEligValues;
            ZCHANNEL_REL_ID = zChannelRelId;
        }

        public override string ToString()
        {
            return xml.ToString();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            xml = XElement.Parse(reader.ReadInnerXml());
        }

        public void WriteXml(XmlWriter writer)
        {
            xml.WriteTo(writer);
        }

        public List<Field> ToFields()
        {
            List<Field> fields = new List<Field>();
            if (!string.IsNullOrEmpty(ZPC_MODEL_SKU))
                fields.Add(new Field() { Name = zPcModelSkuName, Value = ZPC_MODEL_SKU });
            if (!string.IsNullOrEmpty(ZMANUF_GEO_LOC))
                fields.Add(new Field() { Name = zManufGeoLocName, Value = ZMANUF_GEO_LOC });
            if (!string.IsNullOrEmpty(ZPGM_ELIG_VALUES))
                fields.Add(new Field() { Name = zPgmEligValuesName, Value = ZPGM_ELIG_VALUES });
            if (!string.IsNullOrEmpty(ZOEM_EXT_ID))
                fields.Add(new Field() { Name = zOemExtIdName, Value = ZOEM_EXT_ID });
            if (!string.IsNullOrEmpty(ZCHANNEL_REL_ID))
                fields.Add(new Field() { Name = zChannelRelIdName, Value = ZCHANNEL_REL_ID });
            return fields;
        }

        public OemOptionalInfo(List<Field> fields)
            : this()
        {
            foreach (Field field in fields)
            {
                SetField(field.Name, field.Value);
            }
        }

        private void SetField(string fieldName, string value)
        {
            switch (fieldName.ToUpper())
            {
                case zOemExtIdName:
                    if (!string.IsNullOrEmpty(value) &&
                        !System.Text.RegularExpressions.Regex.IsMatch(value, NumbericRegEx))
                        throw new ApplicationException("OEM Extended Identifier value required numeric characters.");
                    break;
            }

            if (Values.ContainsKey(fieldName))
                Values[fieldName] = value;
            else
                Values.Add(fieldName, value);
        }
    }
}
