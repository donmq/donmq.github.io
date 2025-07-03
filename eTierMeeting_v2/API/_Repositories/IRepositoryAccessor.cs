using System.Threading.Tasks;
using API.Models;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace eTierV2_API._Repositories
{
    public interface IRepositoryAccessor
    {
        IRepository<Users> Users { get; }
        IRepository<Roles> Roles { get; }
        IRepository<RoleUser> RoleUser { get; }
        IRepository<eTM_Dept_Classification> eTM_Dept_Classification { get; }
        IRepository<eTM_Dept_Score_Data> eTM_Dept_Score_Data { get; }
        IRepository<MES_Dept> MES_Dept { get; }
        IRepository<eTM_MES_MO_Record> eTM_MES_MO_Record { get; }
        IRepository<MES_Org> MES_Org { get; }
        IRepository<eTM_MES_Quality_Defect_Data> eTM_MES_Quality_Defect_Data { get; }
        IRepository<eTM_Settings> eTM_Settings { get; }
        IRepository<MES_Dept_Target> MES_Dept_Target { get; }
        IRepository<FRI_BA_Defect> FRI_BA_Defect { get; }
        IRepository<MES_Defect> MES_Defect { get; }
        IRepository<VW_KanBan_POList_V2> VW_KanBan_POList_V2 { get; }
        IRepository<VW_DeptFromMES> VW_DeptFromMES { get; }
        IRepository<VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a> VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a { get; }
        IRepository<eTM_Meeting_Log> eTM_Meeting_Log { get; }
        IRepository<eTM_Team_Unit> eTM_Team_Unit { get; }
        IRepository<eTM_MES_PT1_Summary> eTM_MES_PT1_Summary { get; }
        IRepository<VW_MES_4MReason> VW_MES_4MReason { get; }
        IRepository<VW_Production_T1_STF_Delivery_Record> VW_Production_T1_STF_Delivery_Record { get; }
        IRepository<VW_Production_T1_UPF_Delivery_Record> VW_Production_T1_UPF_Delivery_Record { get; }
        IRepository<eTM_Page_Settings> eTM_Page_Settings { get; }
        IRepository<MES_IPQC_Defect> MES_IPQC_Defect { get; }
        IRepository<eTM_Video> eTM_Video { get; }
        IRepository<MES_Dept_Plan> MES_Dept_Plan { get; }
        IRepository<MES_Line> MES_Line { get; }
        IRepository<eTM_HSE_Score_Data> eTM_HSE_Score_Data { get; }
        IRepository<eTM_HSE_Score_Image> eTM_HSE_Score_Image { get; }
        IRepository<eTM_Page_Item_Settings> eTM_Page_Item_Settings { get; }
        IEfficiencyRepository<eTM_HP_Efficiency_Data> eTM_HP_Efficiency_Data { get; }
        IEfficiencyRepository<VW_eTM_HP_Efficiency_Data> VW_eTM_HP_Efficiency_Data { get; }
        IRepository<VW_MES_Org_Mapping> VW_MES_Org_Mapping { get; }
        IEfficiencyRepository<eTM_HP_Dept_Kind> eTM_HP_Dept_Kind { get; }
        IEfficiencyRepository<eTM_HP_G01_Flag> eTM_HP_G01_Flag { get; }
        IEfficiencyRepository<HP_Production_Line_ie21> HP_Production_Line_ie21 { get; }
        IRepository<VW_LineGroup> VW_LineGroup { get; }
        IRepository<eTM_Video_Play_Log> eTM_Video_Play_Log { get; }
        IRepository<eTM_Production_T1_Video> eTM_Production_T1_Video { get; }
        IRepository<eTM_Meeting_Log_Page> eTM_Meeting_Log_Page { get; }
        IRepository<eTM_HP_Efficiency_Data_External> eTM_HP_Efficiency_Data_External { get; }
        IEfficiencyRepository<eTM_HP_Efficiency_Data_External> ETM_HP_Efficiency_Data_External { get; }
        IRepository<eTM_T2_Meeting_Seeting> eTM_T2_Meeting_Seeting { get; }
        IEfficiencyRepository<eTM_HP_Efficiency_Data_Subcon> eTM_HP_Efficiency_Data_Subcon { get; }
        IRepository<VW_T2_Meeting_Log> VW_T2_Meeting_Log { get; }
        IRepository<SM_Basic_Data> SM_Basic_Data { get; }
        IRepository<SM_Basic_Data_ColDesc> SM_Basic_Data_ColDesc { get; }
        IRepository<CST_WorkCenter_Plan> CST_WorkCenter_Plan { get; }
        IRepository<VW_Prod_T1_CTB_Delivery> VW_Prod_T1_CTB_Delivery { get; }

        Task<bool> Save();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}