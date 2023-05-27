using Fias.Api.Interfaces.XmlModels;

namespace Fias.Api.Models.FiasModels.XmlModels.ParamTypes
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class PARAMTYPES : IXmlModel
    {

        private PARAMTYPESPARAMTYPE[] pARAMTYPEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PARAMTYPE")]
        public PARAMTYPESPARAMTYPE[] PARAMTYPE
        {
            get
            {
                return this.pARAMTYPEField;
            }
            set
            {
                this.pARAMTYPEField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PARAMTYPESPARAMTYPE
    {

        private byte idField;

        private string nAMEField;

        private string dESCField;

        private string cODEField;

        private bool iSACTIVEField;

        private System.DateTime uPDATEDATEField;

        private System.DateTime sTARTDATEField;

        private System.DateTime eNDDATEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte ID
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
        public string DESC
        {
            get
            {
                return this.dESCField;
            }
            set
            {
                this.dESCField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CODE
        {
            get
            {
                return this.cODEField;
            }
            set
            {
                this.cODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool ISACTIVE
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
