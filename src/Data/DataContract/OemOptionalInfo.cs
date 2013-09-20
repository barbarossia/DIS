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
using System.Collections;
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
        //new add for V1.6
        private const string zFrmFactorCl1 = "ZFRM_FACTOR_CL1";
        private const string zFrmFactorCl2 = "ZFRM_FACTOR_CL2";
        private const string zScreenSize = "ZSCREEN_SIZE";
        private const string zTouchScreen = "ZTOUCH_SCREEN";

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

        //add for V1.6
        public string ZFRM_FACTOR_CL1
        {
            get { return Values[zFrmFactorCl1]; }
            set { SetField(zFrmFactorCl1, value); }
        }

        public string ZFRM_FACTOR_CL2
        {
            get { return Values[zFrmFactorCl2]; }
            set { SetField(zFrmFactorCl2, value); }
        }

        public string ZSCREEN_SIZE
        {
            get { return Values[zScreenSize]; }
            set { SetField(zScreenSize, value); }
        }

        public string ZTOUCH_SCREEN
        {
            get { return Values[zTouchScreen]; }
            set { SetField(zTouchScreen, value); }
        }

        public bool HasOHRData
        {
            get 
            {
                return !(string.IsNullOrEmpty(ZFRM_FACTOR_CL1) || string.IsNullOrEmpty(ZFRM_FACTOR_CL2) || string.IsNullOrEmpty(ZSCREEN_SIZE) || string.IsNullOrEmpty(ZTOUCH_SCREEN)||string.IsNullOrEmpty(ZPC_MODEL_SKU));
            }
        }

        public Dictionary<string, string> Values { get; private set; }

        public OemOptionalInfo()
            : this(null, null, null, null, null, null, null, null, null)
        {
        }

        public OemOptionalInfo(string xml)
            : this(null, null, null, null, null, null, null, null, null)
        {
            if (!string.IsNullOrEmpty(xml))
                this.xml = XElement.Parse(xml);
        }

        public OemOptionalInfo(string zPcModelSku, string zOemExtId,
          string zManufGeoLoc, string zPgmEligValues, string zChannelRelId, string zFrmFactor1,
          string zFrmFactor2, string screenSize, string touchScreen)
        {
            Values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            ZPC_MODEL_SKU = zPcModelSku;
            ZOEM_EXT_ID = zOemExtId;
            ZMANUF_GEO_LOC = zManufGeoLoc;
            ZPGM_ELIG_VALUES = zPgmEligValues;
            ZCHANNEL_REL_ID = zChannelRelId;

            ZFRM_FACTOR_CL1 = zFrmFactor1;
            ZFRM_FACTOR_CL2 = zFrmFactor2;
            ZSCREEN_SIZE = screenSize;
            ZTOUCH_SCREEN = touchScreen;
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

            if (!string.IsNullOrEmpty(ZFRM_FACTOR_CL1))
                fields.Add(new Field() { Name = zFrmFactorCl1, Value = ZFRM_FACTOR_CL1 });
            if (!string.IsNullOrEmpty(ZFRM_FACTOR_CL2))
                fields.Add(new Field() { Name = zFrmFactorCl2, Value = ZFRM_FACTOR_CL2 });
            if (!string.IsNullOrEmpty(ZSCREEN_SIZE))
                fields.Add(new Field() { Name = zScreenSize, Value = ZSCREEN_SIZE });
            if (!string.IsNullOrEmpty(ZTOUCH_SCREEN))
                fields.Add(new Field() { Name = zTouchScreen, Value = ZTOUCH_SCREEN });
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

    //add for OHR data
    public static class OHRData
    {
        private static Dictionary<string, List<string>> zfrm_factorValue;
        public static Dictionary<string, List<string>> ZFRM_FACTORValue
        {
            get
            {
                zfrm_factorValue = new Dictionary<string, List<string>>();
                zfrm_factorValue.Add("Desktop", new List<string> { "Standard", "AIO" });
                zfrm_factorValue.Add("Notebook", new List<string> { "Standard", "Ultraslim", "Convertible" });
                zfrm_factorValue.Add("Tablet", new List<string> { "Standard", "Detachable" });
                return zfrm_factorValue;
            }
        }

        public static List<string> ZTOUCH_SCREENValue
        {
            get 
            {
                return new List<string>() { "Touch", "Non-touch" };
            }
        }
    }


}
