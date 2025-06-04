namespace API.Helpers.Utilities
{
    public class KeyValueUtility
    {
        public KeyValueUtility()
        {
        }

        public KeyValueUtility(dynamic key)
        {
            Key = key;
        }

        public KeyValueUtility(dynamic key, object optional)
        {
            Key = key;
            Optional = optional;
        }

        public KeyValueUtility(dynamic key, string value)
        {
            Key = key;
            Value = value;
        }

        public KeyValueUtility(dynamic key, string value, object optional)
        {
            Key = key;
            Value = value;
            Optional = optional;
        }

        public KeyValueUtility(dynamic key, string value_vi, string value_en, string value_zh)
        {
            Key = key;
            Value_vi = value_vi;
            Value_en = value_en;
            Value_zh = value_zh;
        }

        public KeyValueUtility(dynamic key, string value_vi, string value_en, string value_zh, object optional)
        {
            Key = key;
            Value_vi = value_vi;
            Value_en = value_en;
            Value_zh = value_zh;
            Optional = optional;
        }

        public KeyValueUtility(dynamic key, string value, string value_vi, string value_en, string value_zh, object optional)
        {
            Key = key;
            Value = value;
            Value_vi = value_vi;
            Value_en = value_en;
            Value_zh = value_zh;
            Optional = optional;
        }

        public dynamic Key { get; set; }
        public string Value { get; set; }
        public string Value_vi { get; set; }
        public string Value_en { get; set; }
        public string Value_zh { get; set; }
        public object Optional { get; set; }
    }
}