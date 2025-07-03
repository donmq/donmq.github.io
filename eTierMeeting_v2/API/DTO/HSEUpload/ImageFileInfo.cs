using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace eTierV2_API.DTO.HSEUpload
{
    public class ImageRemark {
        public string Name {get;set;}
        public string Remark {get;set;}
    }
    public class ImageDataUpload {
        public List<IFormFile> Images {get;set;}
        public List<string> Remarks {get;set;}
        public int HseID {get;set;}
    }
}