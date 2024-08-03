using SolarEase.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ML;

namespace SolarEase.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> userManager;
        public AccountRepository(UserManager<Account> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<Account?> GetByIdAsync(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<(IdentityResult, string?)> UpdateAsync(string Id, Account account, string newPassword)
        {
            var existingAccount = await userManager.FindByIdAsync(Id);

            existingAccount.UserName = account.UserName;
            existingAccount.Email = account.Email;
            existingAccount.ValidationCode = account.ValidationCode;
            existingAccount.ValidationCodeExpiration = account.ValidationCodeExpiration;

            string resetToken = null; // Initialize resetToken variable

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                var returnUser = existingAccount;

                // Generate password reset token
                var code = await userManager.GeneratePasswordResetTokenAsync(returnUser);

                resetToken = code; // Capture the generated token

                // Reset the password using the new password
                var resetPasswordResult = await userManager.ResetPasswordAsync(returnUser, code, newPassword);

                if (!resetPasswordResult.Succeeded)
                {
                    // Handle password reset failure
                    return (resetPasswordResult, resetToken);
                }
            }

            var updateResult = await userManager.UpdateAsync(existingAccount);
            return (updateResult, resetToken);
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            var existingAccount = await userManager.FindByIdAsync(userId);
            if (existingAccount == null)
            {
                return false; // Account not found
            }

            var deleteResult = await userManager.DeleteAsync(existingAccount);
            return deleteResult.Succeeded;
        }

        public async Task<bool> CreateAsync(Account account, string password)
        {
            var user = new Account
            {
                UserName = account.UserName,
                Email = account.Email,
                Type = account.Type
            };

            var result = await userManager.CreateAsync(user, password);
            return result.Succeeded;
        }


        public async Task<IdentityResult> UpdateValidationAsync(string Id, Account account)
        {
            var existingAccount = await userManager.FindByIdAsync(Id);

            existingAccount.ValidationCode = account.ValidationCode;
            existingAccount.ValidationCodeExpiration = account.ValidationCodeExpiration;
            
            var updateResult = await userManager.UpdateAsync(existingAccount);

            return updateResult;
        }

    }
}

