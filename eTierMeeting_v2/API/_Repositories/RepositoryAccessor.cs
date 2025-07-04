using System.Threading.Tasks;
using API.Data;
using API.Models;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Repositories.Repositories;
using eTierV2_API.Data;
using eTierV2_API.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API._Repositories
{
    public class RepositoryAccessor : IRepositoryAccessor
    {
        private DBContext _dbContext;
        public RepositoryAccessor(DBContext dbContext, MesDataContext Mescontext, SHCQDataContext SHCQcontext, ciMESDataContext ciMESDataContext, IConfiguration configuration)
        {
            _dbContext = dbContext;

            Users = new Repository<Users>(dbContext, configuration);
            Roles = new Repository<Roles>(dbContext, configuration);
            RoleUser = new Repository<RoleUser>(dbContext, configuration);
            eTM_Dept_Classification = new Repository<eTM_Dept_Classification>(dbContext, configuration);
            eTM_Dept_Score_Data = new Repository<eTM_Dept_Score_Data>(dbContext, configuration);
            eTM_MES_MO_Record = new Repository<eTM_MES_MO_Record>(dbContext, configuration);
            eTM_MES_Quality_Defect_Data = new Repository<eTM_MES_Quality_Defect_Data>(dbContext, configuration);
            eTM_Settings = new Repository<eTM_Settings>(dbContext, configuration);
            MES_Dept_Target = new MesRepository<MES_Dept_Target>(Mescontext);
            MES_Org = new MesRepository<MES_Org>(Mescontext);
            MES_Dept = new MesRepository<MES_Dept>(Mescontext);
            MES_Defect = new MesRepository<MES_Defect>(Mescontext);
            MES_IPQC_Defect = new MesRepository<MES_IPQC_Defect>(Mescontext);
            MES_Dept_Plan = new MesRepository<MES_Dept_Plan>(Mescontext);
            MES_Line = new MesRepository<MES_Line>(Mescontext);
            FRI_BA_Defect = new FRIRepository<FRI_BA_Defect>(SHCQcontext);
            VW_KanBan_POList_V2 = new MesRepository<VW_KanBan_POList_V2>(Mescontext);
            VW_DeptFromMES = new Repository<VW_DeptFromMES>(dbContext, configuration);
            VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a = new Repository<VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a>(dbContext, configuration);
            eTM_Meeting_Log = new Repository<eTM_Meeting_Log>(dbContext, configuration);
            eTM_Team_Unit = new Repository<eTM_Team_Unit>(dbContext, configuration);
            eTM_MES_PT1_Summary = new Repository<eTM_MES_PT1_Summary>(dbContext, configuration);
            VW_MES_4MReason = new Repository<VW_MES_4MReason>(dbContext, configuration);
            VW_Production_T1_STF_Delivery_Record = new Repository<VW_Production_T1_STF_Delivery_Record>(dbContext, configuration);
            VW_Production_T1_UPF_Delivery_Record = new Repository<VW_Production_T1_UPF_Delivery_Record>(dbContext, configuration);
            eTM_Page_Settings = new Repository<eTM_Page_Settings>(dbContext, configuration);
            eTM_Video = new Repository<eTM_Video>(dbContext, configuration);
            eTM_HSE_Score_Data = new Repository<eTM_HSE_Score_Data>(dbContext, configuration);
            eTM_HSE_Score_Image = new Repository<eTM_HSE_Score_Image>(dbContext, configuration);
            eTM_Page_Item_Settings = new Repository<eTM_Page_Item_Settings>(dbContext, configuration);
            eTM_HP_Efficiency_Data = new EfficiencyRepository<eTM_HP_Efficiency_Data>(dbContext, configuration);
            VW_MES_Org_Mapping = new Repository<VW_MES_Org_Mapping>(dbContext, configuration);
            eTM_HP_Dept_Kind = new EfficiencyRepository<eTM_HP_Dept_Kind>(dbContext, configuration);
            eTM_HP_G01_Flag = new EfficiencyRepository<eTM_HP_G01_Flag>(dbContext, configuration);
            VW_eTM_HP_Efficiency_Data = new EfficiencyRepository<VW_eTM_HP_Efficiency_Data>(dbContext, configuration);
            HP_Production_Line_ie21 = new EfficiencyRepository<HP_Production_Line_ie21>(dbContext, configuration);
            VW_LineGroup = new Repository<VW_LineGroup>(dbContext, configuration);
            eTM_Video_Play_Log = new Repository<eTM_Video_Play_Log>(dbContext, configuration);
            eTM_Production_T1_Video = new Repository<eTM_Production_T1_Video>(dbContext, configuration);
            eTM_Meeting_Log_Page = new Repository<eTM_Meeting_Log_Page>(dbContext, configuration);
            eTM_HP_Efficiency_Data_External = new Repository<eTM_HP_Efficiency_Data_External>(dbContext, configuration); 
            ETM_HP_Efficiency_Data_External = new EfficiencyRepository<eTM_HP_Efficiency_Data_External>(dbContext, configuration); 
            eTM_HP_Efficiency_Data_Subcon = new EfficiencyRepository<eTM_HP_Efficiency_Data_Subcon>(dbContext, configuration);
            eTM_T2_Meeting_Seeting = new Repository<eTM_T2_Meeting_Seeting>(dbContext, configuration);
            VW_T2_Meeting_Log = new Repository<VW_T2_Meeting_Log>(dbContext, configuration);
            SM_Basic_Data = new Repository<SM_Basic_Data>(dbContext, configuration);
            SM_Basic_Data_ColDesc = new Repository<SM_Basic_Data_ColDesc>(dbContext, configuration);
            CST_WorkCenter_Plan = new ciMesRepository<CST_WorkCenter_Plan>(ciMESDataContext);
            VW_Prod_T1_CTB_Delivery = new Repository<VW_Prod_T1_CTB_Delivery>(dbContext, configuration);
            VW_Efficiency_ByBrand = new EfficiencyRepository<VW_Efficiency_ByBrand>(dbContext, configuration);
        }

        public IRepository<Users> Users { get; set; }
        public IRepository<Roles> Roles { get; set; }
        public IRepository<RoleUser> RoleUser { get; set; }
        public IRepository<eTM_Dept_Classification> eTM_Dept_Classification { get; set; }
        public IRepository<eTM_Dept_Score_Data> eTM_Dept_Score_Data { get; set; }
        public IRepository<MES_Dept> MES_Dept { get; set; }
        public IRepository<eTM_MES_MO_Record> eTM_MES_MO_Record { get; set; }
        public IRepository<MES_Org> MES_Org { get; set; }
        public IRepository<eTM_MES_Quality_Defect_Data> eTM_MES_Quality_Defect_Data { get; set; }
        public IRepository<eTM_Settings> eTM_Settings { get; set; }
        public IRepository<MES_Dept_Target> MES_Dept_Target { get; set; }
        public IRepository<FRI_BA_Defect> FRI_BA_Defect { get; set; }
        public IRepository<MES_Defect> MES_Defect { get; set; }
        public IRepository<VW_KanBan_POList_V2> VW_KanBan_POList_V2 { get; set; }
        public IRepository<VW_DeptFromMES> VW_DeptFromMES { get; set; }
        public IRepository<VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a> VW_HPBasis_HPMonthlyEstimateProduceTarget_i14a { get; set; }
        public IRepository<eTM_Meeting_Log> eTM_Meeting_Log { get; set; }
        public IRepository<eTM_Team_Unit> eTM_Team_Unit { get; set; }
        public IRepository<eTM_MES_PT1_Summary> eTM_MES_PT1_Summary { get; set; }
        public IRepository<VW_MES_4MReason> VW_MES_4MReason { get; set; }
        public IRepository<VW_Production_T1_STF_Delivery_Record> VW_Production_T1_STF_Delivery_Record { get; set; }
        public IRepository<VW_Production_T1_UPF_Delivery_Record> VW_Production_T1_UPF_Delivery_Record { get; set; }
        public IRepository<eTM_Page_Settings> eTM_Page_Settings { get; set; }
        public IRepository<MES_IPQC_Defect> MES_IPQC_Defect { get; set; }
        public IRepository<eTM_Video> eTM_Video { get; set; }
        public IRepository<MES_Dept_Plan> MES_Dept_Plan { get; set; }
        public IRepository<MES_Line> MES_Line { get; set; }
        public IRepository<eTM_HSE_Score_Data> eTM_HSE_Score_Data { get; set; }
        public IRepository<eTM_HSE_Score_Image> eTM_HSE_Score_Image { get; set; }
        public IRepository<eTM_Page_Item_Settings> eTM_Page_Item_Settings { get; set; }
        public IEfficiencyRepository<eTM_HP_Efficiency_Data> eTM_HP_Efficiency_Data { get; set; }
        public IRepository<VW_MES_Org_Mapping> VW_MES_Org_Mapping { get; set; }
        public IEfficiencyRepository<eTM_HP_Dept_Kind> eTM_HP_Dept_Kind { get; set; }
        public IEfficiencyRepository<eTM_HP_G01_Flag> eTM_HP_G01_Flag { get; set; }
        public IEfficiencyRepository<HP_Production_Line_ie21> HP_Production_Line_ie21 { get; set; }
        public IRepository<VW_LineGroup> VW_LineGroup { get; set; }
        public IRepository<eTM_Video_Play_Log> eTM_Video_Play_Log { get; set; }
        public IRepository<eTM_Production_T1_Video> eTM_Production_T1_Video { get; set; }
        public IRepository<eTM_Meeting_Log_Page> eTM_Meeting_Log_Page { get; set; }
        public IRepository<eTM_HP_Efficiency_Data_External> eTM_HP_Efficiency_Data_External { get; set; }
        public IEfficiencyRepository<eTM_HP_Efficiency_Data_External> ETM_HP_Efficiency_Data_External { get; set; }
        public IRepository<eTM_T2_Meeting_Seeting> eTM_T2_Meeting_Seeting { get; set; }
        public IRepository<VW_T2_Meeting_Log> VW_T2_Meeting_Log { get; set; }
        public IEfficiencyRepository<VW_eTM_HP_Efficiency_Data> VW_eTM_HP_Efficiency_Data { get; set; }
        public IEfficiencyRepository<eTM_HP_Efficiency_Data_Subcon> eTM_HP_Efficiency_Data_Subcon { get; set; }
        public IRepository<SM_Basic_Data> SM_Basic_Data { get; set; }
        public IRepository<SM_Basic_Data_ColDesc> SM_Basic_Data_ColDesc { get; set; }
        public IRepository<CST_WorkCenter_Plan> CST_WorkCenter_Plan { get; set; }
        public IRepository<VW_Prod_T1_CTB_Delivery> VW_Prod_T1_CTB_Delivery { get; set; }
        public IEfficiencyRepository<VW_Efficiency_ByBrand> VW_Efficiency_ByBrand { get; set; }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
    }
}