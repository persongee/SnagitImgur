using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RestSharp;
using SnagitImgur.Plugin.ImageService;
using SnagitImgur.Properties;
using SNAGITLib;

namespace SnagitImgur.Plugin
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("681D1A5C-A78F-4D27-86A2-A07AAC89B8FE")]
    public class PackageOutput : MarshalByRefObject, IComponentInitialize, IOutput, IOutputMenu, IPackageOptionsUI
    {
        private ShareController shareController;

        public void InitializeComponent(object pExtensionHost, IComponent pComponent, componentInitializeType initType)
        {
            var snagitHost = pExtensionHost as ISnagIt;
            if (snagitHost == null)
            {
                throw new InvalidOperationException("Unable to communicate with Snagit");
            }

            shareController = new ShareController(snagitHost);
        }

        public void Output()
        {
            IImageService imageService = GetSelectedImageService();
            shareController.ShareImage(imageService);
        }

        private IImageService GetSelectedImageService()
        {
            return new ImgurService(
                CreateAuthenticator(Settings.Default)
                );
        }

        private IAuthenticator CreateAuthenticator(Settings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.OAuthToken))
            {
                return new OAuth2AuthorizationRequestHeaderAuthenticator(settings.OAuthToken, "Bearer");
            }

            return new AnonymousClientAuthenticator(settings.ClientID);
        }

        public string GetOutputMenuData()
        {
            return "<menu> " +
                      "<menuitem label=\"Send to imgur.com\" id=\"1\" />" +
                      "<menuseparator />" +
                      "<menuitem label=\"Account...\" id=\"2\" />" +
                      "<menuitem label=\"Options...\" id=\"3\" />" +
                      "<menuitem label=\"About\" id=\"4\" />" +
                   "</menu>";
        }

        public void SelectOutputMenuItem(string id)
        {
            switch (id)
            {
                case "1":
                    Output();
                    break;
                case "2":
                    ShowAccount();
                    break;
                case "3":
                    ShowPackageOptionsUI();
                    break;
                case "4":
                    ShowAbout();
                    break;
            }
        }

        private void ShowAccount()
        {
            
        }

        private void ShowAbout()
        {
            MessageBox.Show("About");

        }

        public void ShowPackageOptionsUI()
        {
            MessageBox.Show("Settings");
            
        }
    }
}