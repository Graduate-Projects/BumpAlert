using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.Resources;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace Bump.Constants
{
    public static class MaterialConfiguration
    {
        public static MaterialSnackbarConfiguration SnackbarConfiguration = new MaterialSnackbarConfiguration
        {
            BackgroundColor = Color.White,
            MessageTextColor = Color.Black,
            TintColor = Color.White,
            CornerRadius = 8,
            ScrimColor = Color.White
        };
    }
}
