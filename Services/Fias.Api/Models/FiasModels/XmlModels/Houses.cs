using Fias.Api.Interfaces.XmlModels;

namespace Fias.Api.Models.FiasModels.XmlModels.Houses
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "HOUSES")]
    public partial class HOUSES : IXmlModel
    {
        private HouseModel[] hOUSEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("HOUSE")]
        public HouseModel[] HOUSE
        {
            get
            {
                return this.hOUSEField;
            }
            set
            {
                this.hOUSEField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "HOUSE")]
    public partial class HouseModel : IXmlRowModel
    {
        private uint idField;

        private uint oBJECTIDField;

        private string oBJECTGUIDField;

        private uint cHANGEIDField;

        private string? hOUSENUMField;

        private byte hOUSETYPEField;

        private byte oPERTYPEIDField;

        private uint pREVIDField;

        private uint nEXTIDField;

        private bool nEXTIDFieldSpecified;

        private System.DateTime uPDATEDATEField;

        private System.DateTime sTARTDATEField;

        private System.DateTime eNDDATEField;

        private byte iSACTUALField;

        private byte iSACTIVEField;

        private string aDDNUM1Field;

        private bool aDDNUM1FieldSpecified;

        private byte aDDTYPE1Field;

        private bool aDDTYPE1FieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "ID")]
        public uint ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "OBJECTID")]
        public uint OBJECTID
        {
            get
            {
                return this.oBJECTIDField;
            }
            set
            {
                this.oBJECTIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "OBJECTGUID")]
        public string OBJECTGUID
        {
            get
            {
                return this.oBJECTGUIDField;
            }
            set
            {
                this.oBJECTGUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "CHANGEID")]
        public uint CHANGEID
        {
            get
            {
                return this.cHANGEIDField;
            }
            set
            {
                this.cHANGEIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "HOUSENUM")]
        public string? HOUSENUM
        {
            get
            {
                return this.hOUSENUMField;
            }
            set
            {
                this.hOUSENUMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "HOUSETYPE")]
        public byte HOUSETYPE
        {
            get
            {
                return this.hOUSETYPEField;
            }
            set
            {
                this.hOUSETYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "OPERTYPEID")]
        public byte OPERTYPEID
        {
            get
            {
                return this.oPERTYPEIDField;
            }
            set
            {
                this.oPERTYPEIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "PREVID")]
        public uint PREVID
        {
            get
            {
                return this.pREVIDField;
            }
            set
            {
                this.pREVIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "NEXTID")]
        public uint NEXTID
        {
            get
            {
                return this.nEXTIDField;
            }
            set
            {
                this.nEXTIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NEXTIDSpecified
        {
            get
            {
                return this.nEXTIDFieldSpecified;
            }
            set
            {
                this.nEXTIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date", AttributeName = "UPDATEDATE")]
        public System.DateTime UPDATEDATE
        {
            get
            {
                return this.uPDATEDATEField;
            }
            set
            {
                this.uPDATEDATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date", AttributeName = "STARTDATE")]
        public System.DateTime STARTDATE
        {
            get
            {
                return this.sTARTDATEField;
            }
            set
            {
                this.sTARTDATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date", AttributeName = "ENDDATE")]
        public System.DateTime ENDDATE
        {
            get
            {
                return this.eNDDATEField;
            }
            set
            {
                this.eNDDATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "ISACTUAL")]
        public byte ISACTUAL
        {
            get
            {
                return this.iSACTUALField;
            }
            set
            {
                this.iSACTUALField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "ISACTIVE")]
        public byte ISACTIVE
        {
            get
            {
                return this.iSACTIVEField;
            }
            set
            {
                this.iSACTIVEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "ADDNUM1")]
        public string ADDNUM1
        {
            get
            {
                return this.aDDNUM1Field;
            }
            set
            {
                this.aDDNUM1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ADDNUM1Specified
        {
            get
            {
                return this.aDDNUM1FieldSpecified;
            }
            set
            {
                this.aDDNUM1FieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "ADDTYPE1")]
        public byte ADDTYPE1
        {
            get
            {
                return this.aDDTYPE1Field;
            }
            set
            {
                this.aDDTYPE1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ADDTYPE1Specified
        {
            get
            {
                return this.aDDTYPE1FieldSpecified;
            }
            set
            {
                this.aDDTYPE1FieldSpecified = value;
            }
        }
    }
}
