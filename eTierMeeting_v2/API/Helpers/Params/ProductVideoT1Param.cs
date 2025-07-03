

using System.Collections.Generic;

namespace eTierV2_API.Helpers.Params
{
    public class ProductVideoT1Param
    {
        public string VideoKind {get;set;}
        public string Center {get;set;}
        public string Tier {get;set;}
        public string Section {get;set;}
        public string Unit {get;set;}
        public string From_Date {get;set;}
        public string To_Date {get;set;}
    }
    public class BatchDeleteParam
    {
        public string VideoKind {get;set;}
        public string Center {get;set;}
        public string Tier {get;set;}
        public string Section {get;set;}
        public List<string> Units {get;set;}
        public string From_Date {get;set;}
        public string To_Date {get;set;}
    }
}