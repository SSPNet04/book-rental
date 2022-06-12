using System;
using System.Collections.Generic;
using System.Text;

namespace BookRent.DTOs.Inputs
{
    public class UserChangePasswordParam
    {
        /// <summary>
        /// Old password
        /// </summary>
        public string? OldPassword { get; set; }
        /// <summary>
        /// New password
        /// </summary>
        public string? NewPassword { get; set; }
        /// <summary>
        /// Confirm new password
        /// </summary>
        public string? ConfirmPassword { get; set; }
        /// <summary>
        /// New username
        /// </summary>
        public string? Username { get; set; }
    }
}
