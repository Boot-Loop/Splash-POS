using Core.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels
{
    public class HomeViewModel
    {
        public StaffModel LoggedInUser { get; private set; }

        public HomeViewModel(StaffModel user) {
            this.LoggedInUser = user;
            Console.WriteLine("User is " + user.FirstName.value);
        }
    }
}
