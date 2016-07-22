#region Using Directives

using System;
using System.Drawing;

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Imaging.Interop;

using Pharmatechnik.Nav.Language.Extension.LanguageService;

#endregion

namespace Pharmatechnik.Nav.Language.Extension.Images {

    static class NavigationBarImages {

        public static class Index {
            public const int ProjectNode     = 0;
            public const int TaskDeclaration = 1;
            public const int TaskDefinition  = 2;
            public const int TriggerSymbol   = 3;
        }

        static IImageHandle ImageListHandle;

        public static IntPtr GetImageList(Color backgroundColor, IVsImageService2 imageService) {

            EnsureImageListHandle(imageService);

            IntPtr hImageList = NavLanguagePackage.GetImageList(ImageListHandle.Moniker, backgroundColor);

            return hImageList;
        }

        static void EnsureImageListHandle(IVsImageService2 imageService) {

            if (ImageListHandle != null) {
                return;
            }
            
            var imageList = new ImageMonikerImageList(
                ImageMonikers.ProjectNode,
                ImageMonikers.TaskDeclaration,
                ImageMonikers.TaskDefinition,
                ImageMonikers.SignalTrigger);

            ImageListHandle = imageService.AddCustomImageList(imageList);
        }
    }
}