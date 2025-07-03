using eTierV2_API._Repositories;
using eTierV2_API._Services.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API._Services.Interfaces.Production.T1.STF;
using eTierV2_API._Services.Interfaces.Production.T1.UPF;
using eTierV2_API._Services.Interfaces.Production.T2.C2B;
using eTierV2_API._Services.Interfaces.Production.T2.CTB;
using eTierV2_API._Services.Interfaces.Production.T5;
using eTierV2_API._Services.Interfaces.Report;
using eTierV2_API._Services.Services;
using eTierV2_API._Services.Services.Production.T1.C2B;
using eTierV2_API._Services.Services.Production.T1.STF;
using eTierV2_API._Services.Services.Production.T1.UPF;
using eTierV2_API._Services.Services.Production.T2.C2B;
using eTierV2_API._Services.Services.Production.T2.CTB;
using eTierV2_API._Services.Services.Production.T5;
using eTierV2_API._Services.Services.Report;
using eTierV2_API.Helpers.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace eTierV2_API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
           //Repository
            services.AddScoped<IRepositoryAccessor, RepositoryAccessor>();

            //Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeptClassificationServcie, DeptClassificationService>();
            services.AddScoped<IT2MeetingTimeSettingService, T2MeetingTimeSettingService>();
            services.AddScoped<IProductionT1SafetyService, ProductionT1SafetyService>();
            services.AddScoped<IProductionT1QualityService, ProductionT1QualityService>();
            services.AddScoped<IProductionT1KaizenService, ProductionT1KaizenService>();
            services.AddScoped<IUploadT1Service, UploadT1Service>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IEfficiencyService, EfficiencyService>();
            services.AddScoped<IProductionT1ModelPreparationService, ProductionT1ModelPreparationService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IRecordMeetingDurationService, RecordMeetingDurationService>();
            services.AddScoped<IProductionT1STFSafetyService, ProductionT1STFSafetyService>();
            services.AddScoped<IProductionT1STFQualityService, ProductionT1STFQualityService>();
            services.AddScoped<IProductionT1STFModelPreparationService, ProductionT1STFModelPreparationService>();
            services.AddScoped<IProductionT1STFKaizenService, ProductionT1STFKaizenService>();
            services.AddScoped<IProductionT1STFDeliveryService, ProductionT1STFDeliveryService>();
            services.AddScoped<IProductionT1STFEfficiencyService, ProductionT1STFEfficiencyService>();
            services.AddScoped<IProductionT1UPFEfficiencyService, ProductionT1UPFEfficiencyService>();
            services.AddScoped<IProductionT1UPFQualityService, ProductionT1UPFQualityService>();
            services.AddScoped<IProductionT1UPFSafetyService, ProductionT1UPFSafetyService>();
            services.AddScoped<IProductionT1UPFDeliveryService, ProductionT1UPFDeliveryService>();
            services.AddScoped<IProductionT1UPFModelPreparationService, ProductionT1UPFModelPreparationService>();
            services.AddScoped<IProductionT1UPFKaizenService, ProductionT1UPFKaizenService>();
            services.AddScoped<IT5ExternalUploadService, T5ExternalUploadService>();

            //T2
            services.AddScoped<IPageEnableDisableService, PageEnableDisableService>();
            services.AddScoped<IProductionT2CTBQualityService, ProductionT2CTBQualityService>();
            services.AddScoped<IPageItemSettingService, PageItemSettingService>();
            services.AddScoped<IHSEUResultUploadService, HSEUResultUploadService>();
            services.AddScoped<IProductionT2CTBEfficiencyService, ProductionT2CTBEfficiencyService>();
            services.AddScoped<IProductionT2CTBSectionService, ProductionT2CTBSectionService>();
            services.AddScoped<IProductionT2SafetyService, ProductionT2SafetyService>();

            //T5
            services.AddScoped<IEfficiencyKanbanService, EfficiencyKanbanService>();
            // FunctionUtility
            services.AddScoped<IFunctionUtility, FunctionUtility>();

            services.AddScoped<I_2_3_1_Meeting_Audit_Report, S_2_3_1_Meeting_Audit_Report>();

        }
    }
}