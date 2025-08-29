namespace API.DTOs.EmployeeMaintenance
{
    public class EmployeeTransferOperationInboundDto
    {
        private string _employeeIDBefore;
        public string History_GUID { get; set; }
        public string USER_GUID { get; set; }
        public string NationalityBefore { get; set; }
        public string IdentificationNumberBefore { get; set; }
        public string LocalFullNameBefore { get; set; }
        public string DivisionBefore { get; set; }
        public string FactoryBefore { get; set; }
        public string EmployeeIDBefore { get => _employeeIDBefore; set => _employeeIDBefore = value?.Trim(); }
        public string DepartmentBefore { get; set; }
        public string AssignedDivisionBefore { get; set; }
        public string AssignedFactoryBefore { get; set; }
        public string AssignedEmployeeIDBefore { get; set; }
        public string AssignedDepartmentBefore { get; set; }
        public decimal PositionGradeBefore { get; set; }
        public string PositionTitleBefore { get; set; }
        public string WorkTypeBefore { get; set; }
        public string ReasonForChangeBefore { get; set; }
        public DateTime EffectiveDateBefore { get; set; }
        public string EffectiveDateBeforeStr { get; set; }
        public bool EffectiveStatusBefore { get; set; }
        public DateTime UpdateTimeBefore { get; set; }
        public string UpdateByBefore { get; set; }
        // After
        public string NationalityAfter { get; set; }
        public string IdentificationNumberAfter { get; set; }
        public string LocalFullNameAfter { get; set; }
        public string DivisionAfter { get; set; }
        public string FactoryAfter { get; set; }
        public string EmployeeIDAfter { get; set; }
        public string DepartmentAfter { get; set; }
        public string AssignedDivisionAfter { get; set; }
        public string AssignedFactoryAfter { get; set; }
        public string AssignedEmployeeIDAfter { get; set; }
        public string AssignedDepartmentAfter { get; set; }
        public decimal? PositionGradeAfter { get; set; }
        public string PositionTitleAfter { get; set; }
        public string WorkTypeAfter { get; set; }
        public string ReasonForChangeAfter { get; set; }
        public DateTime? EffectiveDateAfter { get; set; }
        public string EffectiveDateAfterStr { get; set; }
        public bool EffectiveStatusAfter { get; set; }
        public DateTime UpdateTimeAfter { get; set; }
        public string UpdateByAfter { get; set; }
    }

    public class EmployeeTransferOperationInboundParam
    {
        private string _identificationNumber;
        private string _employeeID;
        private string _localFullName;
        private string _assignedEmployeeID;
        public string Nationality { get; set; }
        public string IdentificationNumber { get => _identificationNumber; set => _identificationNumber = value?.Trim(); }
        public string Division { get; set; }
        public string Factory { get; set; }
        public string EmployeeID { get => _employeeID; set => _employeeID = value?.Trim(); }
        public string LocalFullName { get => _localFullName; set => _localFullName = value?.Trim().ToLower(); }
        public string AssignedDivision { get; set; }
        public string AssignedFactory { get; set; }
        public string AssignedEmployeeID { get => _assignedEmployeeID; set => _assignedEmployeeID = value?.Trim(); }
        public string ReasonForChange { get; set; }
        public string EffectiveDateStart { get; set; }
        public string EffectiveDateEnd { get; set; }
        public string EffectiveDateStart_Str { get; set; }
        public string EffectiveDateEnd_Str { get; set; }
        public string EffectiveStatus { get; set; }
        public string Language { get; set; }
    }
}