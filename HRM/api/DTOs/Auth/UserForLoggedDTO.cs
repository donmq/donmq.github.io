namespace API.Dtos.Auth
{
    public class UserLoginParam
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Factory { get; set; }
    }
    public class DirectoryInfomation
    {
        public string Seq { get; set; }
        public string Directory_Name { get; set; }
        public string Directory_Code { get; set; }
    }
    public class ProgramInfomation
    {
        public int Seq { get; set; }
        public string Program_Name { get; set; }
        public string Program_Code { get; set; }
        public string Parent_Directory_Code { get; set; }
    }
    public class FunctionInfomation
    {
        public string Program_Code { get; set; }
        public string Function_Code { get; set; }
    }public class CodeInformation
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Kind { get; set; }
        public IEnumerable<CodeLang> Translations { get; set; }
    }
    
    public class CodeLang
    {
        public string Lang { get; set; }
        public string Name { get; set; }
    }
    public class UserForLoggedDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Factory { get; set; }
        public string Account { get; set; }
    }
    public class AuthProgram
    {
        public IEnumerable<DirectoryInfomation> Directories { get; set; }
        public IEnumerable<ProgramInfomation> Programs { get; set; }
        public IEnumerable<FunctionInfomation> Functions { get; set; }
    }
    public class DataResponseDTO
    {
        public UserForLoggedDTO User { get; set; }
        public List<CodeInformation> Code_Information { get; set; }
    }
    public class ResultResponse
    {
        public DataResponseDTO Data { get; set; }
        public string Token { get; set; }
    }
}