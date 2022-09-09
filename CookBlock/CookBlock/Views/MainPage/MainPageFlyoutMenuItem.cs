using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBlock.Views.MainPage
{
    public class MainPageFlyoutMenuItem
    {
        public MainPageFlyoutMenuItem()
        {
            TargetType = typeof(MainPageFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public string IconSource { get; set; }
        public Type TargetType { get; set; }
    }
}