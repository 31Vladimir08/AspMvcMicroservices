namespace Fias.Api.Models.FiasModels.XmlModels.AddrObj
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ADDRESSOBJECTS
    {

        private ADDRESSOBJECTSOBJECT[] oBJECTField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("OBJECT")]
        public ADDRESSOBJECTSOBJECT[] OBJECT
        {
            get
            {
                return this.oBJECTField;
            }
            set
            {
                this.oBJECTField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ADDRESSOBJECTSOBJECT
    {

        private uint idField;

        private uint oBJECTIDField;

        private string oBJECTGUIDField;

        private uint cHANGEIDField;

        private string nAMEField;

        private string tYPENAMEField;

        private byte lEVELField;

        private byte oPERTYPEIDField;

        private uint pREVIDField;

        private uint nEXTIDField;

        private bool nEXTIDFieldSpecified;

        private System.DateTime uPDATEDATEField;

        private System.DateTime sTARTDATEField;

        private System.DateTime eNDDATEField;

        private byte iSACTUALField;

        private byte iSACTIVEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string NAME
        {
            get
            {
                return this.nAMEField;
            }
            set
            {
                this.nAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TYPENAME
        {
            get
            {
                return this.tYPENAMEField;
            }
            set
            {
                this.tYPENAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte LEVEL
        {
            get
            {
                return this.lEVELField;
            }
            set
            {
                this.lEVELField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    }


}
