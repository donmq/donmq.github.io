using AutoMapper;
using eTierV2_API.DTO;
using eTierV2_API.DTO.Production.T2.CTB;
using eTierV2_API.Models;

namespace eTierV2_API.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<UserDTO, Users>();
            CreateMap<RoleUserDTO, RoleUser>();
            CreateMap<eTM_Dept_ClassificationDTO, eTM_Dept_Classification>();
            CreateMap<VW_DeptFromMESDTO, VW_DeptFromMES>();
            CreateMap<eTM_Dept_Score_DataDTO, eTM_Dept_Score_Data>();
            CreateMap<eTM_MES_MO_RecordDTO, eTM_MES_MO_Record>();
            CreateMap<eTM_MES_Quality_Defect_DataDTO, eTM_MES_Quality_Defect_Data>();
            CreateMap<eTM_Production_T1_VideoDTO, eTM_Production_T1_Video>();
            CreateMap<eTM_SettingsDTO, eTM_Settings>();
            CreateMap<MES_Dept_TargetDTO, MES_Dept_Target>();
            CreateMap<FRI_BA_DefectDTO, FRI_BA_Defect>();
            CreateMap<eTM_Meeting_LogDTO, eTM_Meeting_Log>();
            CreateMap<eTM_Meeting_Log_PageDTO, eTM_Meeting_Log_Page>();
            CreateMap<eTM_Team_UnitDTO, eTM_Team_Unit>();
            CreateMap<eTM_MES_PT1_SummaryDTO, eTM_MES_PT1_Summary>();
            CreateMap<eTM_VideoDTO, eTM_Video>();
            CreateMap<VW_Production_T1_STF_Delivery_RecordDTO, VW_Production_T1_STF_Delivery_Record>();
            CreateMap<VW_Production_T1_UPF_Delivery_RecordDTO, VW_Production_T1_UPF_Delivery_Record>();
            CreateMap<eTM_Page_SettingsDTO, eTM_Page_Settings>();
            CreateMap<eTM_Page_Item_SettingsDTO, eTM_Page_Item_Settings>();
            CreateMap<eTM_HSE_Score_ImageDTO, eTM_HSE_Score_Image>();
            CreateMap<eTM_Video_Play_LogDTO, eTM_Video_Play_Log>();
            CreateMap<VW_T2_Meeting_LogDTO, VW_T2_Meeting_Log>();
            CreateMap<eTM_T2_Meeting_SeetingDTO, eTM_T2_Meeting_Seeting>();
        }
    }
}