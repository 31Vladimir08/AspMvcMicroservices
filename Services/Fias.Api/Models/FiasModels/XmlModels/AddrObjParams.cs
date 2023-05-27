using Fias.Api.Interfaces.XmlModels;

namespace Fias.Api.Models.FiasModels.XmlModels.AddrObjParams
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class PARAMS : IXmlModel
    {

        private PARAMSPARAM[] pARAMField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PARAM")]
        public PARAMSPARAM[] PARAM
        {
            get
            {
                return this.pARAMField;
            }
            set
            {
                this.pARAMField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PARAMSPARAM
    {

        private uint idField;

        private uint oBJECTIDField;

        private uint cHANGEIDField;

        private uint cHANGEIDENDField;

        private byte tYPEIDField;

        private string vALUEField;

        private System.DateTime uPDATEDATEField;

        private System.DateTime sTARTDATEField;

        private System.DateTime eNDDATEField;

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
        public uint CHANGEIDEND
        {
            get
            {
                return this.cHANGEIDENDField;
            }
            set
            {
                this.cHANGEIDENDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte TYPEID
        {
            get
            {
                return this.tYPEIDField;
            }
            set
            {
                this.tYPEIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string VALUE
        {
            get
            {
                return this.vALUEField;
            }
            set
            {
                this.vALUEField = value;
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
    }


}
