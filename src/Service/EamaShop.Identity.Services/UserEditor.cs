using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EamaShop.Identity.Services
{
    /// <summary>
    /// User Information Editor
    /// </summary>
    public abstract class UserEditor
    {

        /// <summary>
        /// Set user's nickname
        /// </summary>
        /// <exception cref="ArgumentNullException">value can not be null.</exception>
        public abstract string NickName { set; }
        /// <summary>
        /// Sets user head image as a new uri string.
        /// </summary>
        public abstract string HeadImageUri { set; }
        /// <summary>
        /// Sets 
        /// </summary>
        public abstract Gender Sexy { set; }
        /// <summary>
        /// Sets user's country
        /// </summary>
        public abstract string Country { set; }

        public abstract string City { set; }

        public abstract string Province { set; }
    }
}
