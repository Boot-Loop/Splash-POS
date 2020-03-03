﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Core.Utils;

namespace Core.DB
{
    public enum FieldType
    {
        TEXT, INTEGER, BOOL, FLOAT, DATE_TIME, DIMENTION, EMAIL, PHONE_NUMBER, NIC, WEB_SITE, FILE,
        DROP_DOWN, LIST_FIELD,
    }

    [Serializable]
    public abstract class Field
    {
        protected string name;
        protected string replace_tag;
        protected bool is_required = false;
        protected bool is_null = true;

        protected object last_value = null; // value before validation
        protected bool modified = false;
        protected bool last_value_valid = true;
        protected string validation_error_msg = "";

        public string getName() => name;

        abstract public object getDefault();
        abstract public void setValue(object value);
        abstract public object getValue();
        abstract public FieldType getType();

        public string getReplaceTag() => replace_tag;
        public void setRequired(bool is_required) => this.is_required = is_required;
        public bool isRequired() => is_required;
        public bool isNull() => is_null;
        public void setToNull() => this.is_null = true;
        public void setValidationErrorMsg(string msg) => validation_error_msg = msg;
        public string getValidationErrorMsg()
        {
            if (last_value_valid) return "";
            return validation_error_msg;
        }
        public object getLastValue() => last_value;
        public bool isModified() => modified;
        public void _setNotModified() => modified = false;
    }


    [Serializable]
    public class TextField : Field
    {
        int max_length = -1;
        string default_value = null;
        string _value = null;
        [XmlAttribute]
        public string value
        {
            get { return _value; }
            set
            {
                if (this.max_length > 0 && value != null) if (this.max_length < value.Length) throw new ValidationError("value exceded max length : " + max_length.ToString());
                
                last_value = value;
                if (value != null)
                {
                    last_value_valid = false;
                    _validate(value);
                    last_value_valid = true;
                    is_null = false;
                }
                else is_null = true;
                this._value = value;
                modified = true;
            }
        }
        private TextField() { }
        public TextField(string name = null, string replace_tag = null, string text = null, string default_value = null, bool is_required = false, int max_length = -1, string validation_error_msg = "")
        {
            this.name = name; this.replace_tag = replace_tag; this.max_length = max_length; this.value = text;
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        override public FieldType getType() => FieldType.TEXT;
        override public object getDefault() => default_value;
        override public string ToString() => this.value.ToString();
        override public object getValue() => this.value;
        override public void setValue(object value) { this.value = (string)value; }
        virtual public void _validate(string value) { }
    }

    [Serializable]
    public class BoolField : Field
    {

        bool default_value = false;
        bool _value = false;
        [XmlAttribute]
        public bool value
        {
            get { return _value; }
            set
            {
                last_value = value;
                this._value = value;
                modified = true;
                is_null = false;
            }
        }

        private BoolField() { }
        public BoolField(string name = null, string replace_tag = null, bool is_required = false,  bool default_value = false, string validation_error_msg = "")
        {
            this.name = name; this.replace_tag = replace_tag; 
            this.default_value = default_value; this.validation_error_msg = validation_error_msg;
        }

        override public FieldType getType() => FieldType.BOOL;
        override public object getValue() => this.value;
        override public object getDefault() => this.default_value;
        override public void setValue(object value) { this.value = (bool)value; }
        override public string ToString() => this.value.ToString();

    }


    [Serializable]
    public class IntergerField : Field
    {
        bool is_positive = false;
        long default_value = 0;

        long _value = 0;
        [XmlAttribute]
        public long value
        {
            get { return _value; }
            set
            {
                last_value = value;
                last_value_valid = false;
                if (is_positive && value < 0) throw new ArgumentException();
                last_value_valid = true;
                this._value = value;
                modified = true;
                is_null = false;
            }
        }

        private IntergerField() { }
        public IntergerField(string name = null, string replace_tag = null, bool is_positive = false, long default_value = 0, bool is_required = false, string validation_error_msg = "")
        {
            this.name = name; this.is_positive = is_positive;
            this.replace_tag = replace_tag; this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        override public FieldType getType() => FieldType.INTEGER;
        override public string ToString() => _value.ToString();
        override public void setValue(object value) { this.value = (long)value; }
        override public object getValue() { return this.value; }
        override public object getDefault() => default_value;
        public bool isPositive() => is_positive;

    }

    [Serializable]
    public class FloatField : Field
    {
        bool is_positive = false;
        double default_value = 0;

        double _value = 0.0d;
        [XmlAttribute]
        public double value
        {
            get { return _value; }
            set
            {
                last_value = value;
                last_value_valid = false;
                if (is_positive && value < 0) throw new ArgumentException();
                last_value_valid = true;
                this._value = value;
                modified = true;
                is_null = false;
            }
        }

        private FloatField() { }
        public FloatField(string name = null, string replace_tag = null, bool is_positive = false,  double default_value = 0, bool is_required = false, string validation_error_msg = "")
        {
            this.name = name; this.is_positive = is_positive; 
            this.replace_tag = replace_tag; this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }
        override public FieldType getType() => FieldType.FLOAT;
        override public string ToString() => _value.ToString();
        override public object getDefault() => default_value;
        override public void setValue(object value) { this.value = (double)value; }
        override public object getValue() => value;
        public bool isPositive() => is_positive;

    }

    [Serializable]
    public class DateTimeField : Field
    {
        public enum Format
        {
            DDSUP_MTXT_YYYY,
            MTXT_D_YYYY,
            MM_DD_YYYY,
        }

        List<string> replace_tags;
        Format format = Format.MM_DD_YYYY;
        DateTime default_value = new DateTime();

        DateTime _value = new DateTime();
        [XmlAttribute]
        public DateTime value
        {
            get { return _value; }
            set
            {
                last_value = value;
                this._value = value;
                modified = true;
                is_null = (value == default(DateTime));
            }
        }

        private DateTimeField() { }
        public DateTimeField(string name = null, string replace_tag = null, List<string> replace_tags = null, DateTime datetime = default(DateTime), DateTime default_value = new DateTime(), Format format = Format.MM_DD_YYYY, bool is_required = false, string validation_error_msg = "")
        {
            if (replace_tag != null && replace_tags != null) throw new ArgumentException("it's ambigues to decide to use replace_tag and replace_tags");
            if (replace_tags != null)
            {
                format = Format.DDSUP_MTXT_YYYY;
                setReplaceTags(replace_tags);
            }
            else if (format == Format.DDSUP_MTXT_YYYY) throw new ArgumentException("format DDSUP_MTXT_YYYY can only be use for multiple replace tag");
            this.name = name; this._value = datetime; this.format = format;
            this.default_value = default_value; this.is_required = is_required; this.validation_error_msg = validation_error_msg;
        }

        public void setReplaceTags(List<string> replace_tags)
        {
            if (replace_tags.Count != 3) throw new ArgumentException("expected count is 3 for replace_tags");
            this.replace_tags = replace_tags;
        }
        public Dictionary<string, string> getReplaceTags()
        {
            if (this.replace_tags == null) throw new NullReferenceException("replace_tags is null");
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add(this.replace_tags[0], this._value.Day.ToString());
            ret.Add(this.replace_tags[1], getDayPrefix());
            ret.Add(this.replace_tags[2], getMonthName() + " " + this._value.Year.ToString());
            return ret;
        }


        override public string ToString()
        {
            switch (this.format)
            {
                case Format.MM_DD_YYYY:
                    return this._value.ToString("MM/dd/yyyy");
                case Format.MTXT_D_YYYY:
                    return String.Format("{0} {1}, {2}", getMonthName(), this._value.Day.ToString(), this._value.Year);
                case Format.DDSUP_MTXT_YYYY: // unusable code from now;
                    return String.Format("{0}{1} {2} {3}", this._value.Day.ToString(), getDayPrefix(), getMonthName(), this._value.Year);
                default:
                    return _value.ToString();
            }
        }
        override public FieldType getType() => FieldType.DATE_TIME;
        override public object getDefault() => default_value;
        override public void setValue(object value) { this.value = (DateTime)value; is_null = false; }
        override public object getValue() => value;
        public Format getFormat() => this.format;



        private string getDayPrefix()
        {
            int day = this._value.Day;
            if (day % 10 == 1 && day != 11) return "st";
            if (day % 10 == 2 && day != 12) return "nd";
            if (day % 10 == 3 && day != 13) return "rd";
            return "th";
        }
        private string getMonthName()
        {
            switch (this._value.Month)
            {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
                default: return "<month unknown>";
            }
        }
    }

    [Serializable]
    public class EmailField : TextField
    {
        private EmailField() { }
        public EmailField(string name = null, string replace_tag = null, string text = null, string default_value = null, bool is_required = false, int max_length = 75, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, default_value: default_value, is_required: is_required, max_length: max_length, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.EMAIL;
        public override void _validate(string value)
        {
            // if (!Validator.singleton.validateEmail(value)) throw new ValidationError("invalide email");
        }
    }

    [Serializable]
    public class PhoneNumberField : TextField
    {
        private PhoneNumberField() { }
        public PhoneNumberField(string name = null, string replace_tag = null, string text = null, string default_value = null, bool is_required = false, int max_length = 20, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, default_value: default_value, is_required: is_required, max_length: max_length, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.PHONE_NUMBER;
        public override void _validate(string value)
        {
            // todo:
        }
    }

    [Serializable]
    public class NICField : TextField
    {
        private NICField() { }
        public NICField(string name = null, string replace_tag = null, string text = null, string default_value = null, bool is_required = false, int max_length = 10, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, default_value: default_value, is_required: is_required, max_length: max_length, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.NIC;
        public override void _validate(string value)
        {
            // todo:
        }
    }

    [Serializable]
    public class WebSiteField : TextField
    {
        private WebSiteField() { }
        public WebSiteField(string name = null, string replace_tag = null, string text = null, string default_value = null, bool is_required = false, int max_length = -1, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: text, default_value: default_value, is_required: is_required, max_length: max_length, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.WEB_SITE;
        public override void _validate(string value)
        {
            // todo:
        }
    }

    [Serializable]
    public class FileField : TextField
    {
        private FileField() { }
        public FileField(string name = null, string replace_tag = null, string file_path = null, string default_value = null, bool is_required = false, string validation_error_msg = "")
        : base(name: name, replace_tag: replace_tag, text: file_path, default_value: default_value, is_required: is_required, validation_error_msg: validation_error_msg) { }

        override public FieldType getType() => FieldType.FILE;
        public override void _validate(string value)
        {
            /// TODO:
            // var path = Path.Combine(ProjectManager.singleton.project_dir, value);
            // if (!File.Exists(path)) { throw new FileNotFoundException("path: " + path); }
        }
    }


    [Serializable]
    public class ListField<T> : Field
    {
        private List<T> default_value;
        private List<T> _value;
        [XmlArray]
        public List<T> value
        {
            get { return _value; }
            set
            {
                last_value = value;
                this._value = value;
                modified = true;
                is_null = value is null;
            }
        }

        private ListField() { }
        public ListField(string name = null, List<T> list = null, List<T> default_value = null, bool is_required = false, string validation_error_msg = "")
        {
            this.name = name; this.value = list; this.default_value = default_value; 
            this.validation_error_msg = validation_error_msg;
        }

        public override object getDefault() => default_value;
        public override FieldType getType() => FieldType.LIST_FIELD;
        public override object getValue() => value;
        public override void setValue(object value) {
            this.value = (List<T>)value; is_null = false;
        }
    }

}
