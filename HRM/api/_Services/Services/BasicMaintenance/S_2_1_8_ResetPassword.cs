using API.Data;
using API._Services.Interfaces.BasicMaintenance;
using API.DTOs.BasicMaintenance;

namespace API._Services.Services.BasicMaintenance
{
    public class S_2_1_8_ResetPassword : BaseServices, I_2_1_8_ResetPassword
    {
        public S_2_1_8_ResetPassword(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> ResetPassword(ResetPasswordParam param)
        {
            var account = await _repositoryAccessor.HRMS_Basic_Account.FirstOrDefaultAsync(x => x.Account == param.Account);

            if (account == null)
                return new OperationResult(false, "Account not exists");
            
            if(string.IsNullOrWhiteSpace(param.NewPassword))
                return new OperationResult(false, "Password empty");
                
            account.Password = param.NewPassword;
            account.Password_Reset = false;

            try
            {
                _repositoryAccessor.HRMS_Basic_Account.Update(account);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch
            {
                return new OperationResult(false);
            }
        }
    }
}