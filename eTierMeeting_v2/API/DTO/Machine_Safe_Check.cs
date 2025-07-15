namespace Machine_API.DTO;

public class SurveyRequest
{
    public string UserName { get; set; }
    public string AssnoID { get; set; }
    public string OwnerFty { get; set; }
    public DateTime CheckDate { get; set; }
    public List<QuestionRequest> Questions { get; set; }
}

public class QuestionRequest
{
    public int Key { get; set; }
    public string Value { get; set; }
    public string Answer { get; set; }
    public IFormFile Image { get; set; }
}

public class Machine_Safe_CheckDto
{
    public string MachineID { get; set; }
    public string MachineName { get; set; }
    public string OwnerFty { get; set; }
    public string Location { get; set; }
}

public class ReportCheckMachineSafetyParam
{
    public string Lang { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
}

public class Machine_Safe_Check_ReportDto
{
    public string CheckDate { get; set; }
    public string MachineCode { get; set; }
    public string MachineName { get; set; }
    public string Location { get; set; }
    public int QuestionCode { get; set; }
    public string Answer { get; set; }
    public string ImagePath { get; set; }
}

public class AnswerListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}
