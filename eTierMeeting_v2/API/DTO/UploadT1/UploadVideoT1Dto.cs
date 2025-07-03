
using System.Collections.Generic;

namespace eTierV2_API.DTO.UploadT1
{
    public class UploadVideoT1Dto
    {

        public List<string> UnitIds { get; set; }
        public string Video_Kind { get; set; }
        public string Video_Title_ENG { get; set; }
        public string Video_Title_CHT { get; set; }
        public string VIdeo_Title_LCL { get; set; }
        public string video { get; set; }
        public string video_Icon { get; set; }
        public string Video_Remark { get; set; }
        public string From_Date { get; set; }
        public string To_Date { get; set; }
    }

}