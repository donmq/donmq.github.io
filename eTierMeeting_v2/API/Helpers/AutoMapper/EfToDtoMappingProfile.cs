using AutoMapper;
using eTierV2_API.DTO;
using eTierV2_API.DTO.Production.T2.CTB;
using eTierV2_API.Models;

namespace eTierV2_API.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<Users, UserDTO>();
            CreateMap<eTM_Dept_Classification, eTM_Dept_ClassificationDTO>();
            CreateMap<VW_DeptFromMES, VW_DeptFromMESDTO>();
            CreateMap<eTM_Dept_Score_Data, eTM_Dept_Score_DataDTO>();
            CreateMap<eTM_MES_MO_Record, eTM_MES_MO_RecordDTO>();
            CreateMap<eTM_MES_Quality_Defect_Data, eTM_MES_Quality_Defect_DataDTO>();
            CreateMap<eTM_Production_T1_Video, eTM_Production_T1_VideoDTO>()
                .AfterMap((src, dest) =>
                {
                    dest.Dept_ID = dest.Dept_ID?.Trim();
                    dest.Video_Kind = dest.Video_Kind?.Trim();
                    dest.Video_Title_CHT = dest.Video_Title_CHT?.Trim();
                    dest.Video_Title_ENG = dest.Video_Title_ENG?.Trim();
                    dest.VIdeo_Title_LCL = dest.VIdeo_Title_LCL?.Trim();
                    dest.Video_Icon_Path = dest.Video_Icon_Path?.Trim();
                    dest.Video_Path = dest.Video_Path?.Trim();
                    dest.Video_Remark = dest.Video_Remark?.Trim();
                    dest.Insert_By = dest.Insert_By?.Trim();
                    dest.Update_By = dest.Update_By?.Trim();
                });
            CreateMap<eTM_Settings, eTM_SettingsDTO>();
            CreateMap<MES_Dept_Target, MES_Dept_TargetDTO>();
            CreateMap<FRI_BA_Defect, FRI_BA_DefectDTO>();
            CreateMap<eTM_Meeting_Log, eTM_Meeting_LogDTO>();
            CreateMap<eTM_Meeting_Log_Page, eTM_Meeting_Log_PageDTO>();
            CreateMap<eTM_Team_Unit, eTM_Team_UnitDTO>();
            CreateMap<eTM_MES_PT1_Summary, eTM_MES_PT1_SummaryDTO>();
            CreateMap<eTM_Video, eTM_VideoDTO>();
            CreateMap<VW_Production_T1_STF_Delivery_Record, VW_Production_T1_STF_Delivery_RecordDTO>();
            CreateMap<VW_Production_T1_UPF_Delivery_Record, VW_Production_T1_UPF_Delivery_RecordDTO>();
            CreateMap<eTM_Page_Settings, eTM_Page_SettingsDTO>();
            CreateMap<eTM_Page_Item_Settings, eTM_Page_Item_SettingsDTO>();
            CreateMap<eTM_HSE_Score_Image, eTM_HSE_Score_ImageDTO>();
            CreateMap<eTM_Video_Play_Log, eTM_Video_Play_LogDTO>();
            CreateMap<VW_T2_Meeting_Log, VW_T2_Meeting_LogDTO>();
            CreateMap<eTM_T2_Meeting_Seeting, eTM_T2_Meeting_SeetingDTO>();
        }
    }
}