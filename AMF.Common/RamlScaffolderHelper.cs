﻿using System;
using System.IO;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using AMF.Tools.Core.WebApiGenerator;

namespace AMF.Common
{
    public static class RamlScaffolderHelper
    {
        public static RamlData GetRamlData(string ramlSource, string targetNamespace)
        {
            var logger = new Logger();
            try
            {
                var ramlInfo = RamlInfoService.GetRamlInfo(ramlSource);

                if (ramlInfo.HasErrors)
                {
                    MessageBox.Show(ramlInfo.ErrorMessage);
                    return null;
                }


				var model = new WebApiGeneratorService(ramlInfo.RamlDocument, targetNamespace).BuildModel();
				var filename = Path.GetFileName(ramlSource);
				if (string.IsNullOrWhiteSpace(filename))
					filename = "source.raml";

                return new RamlData ( model, ramlInfo.RamlContents, filename);
            }
            catch (Exception ex)
            {
                logger.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource,
                    VisualStudioAutomationHelper.GetExceptionInfo(ex));
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += " - " + ex.InnerException.Message;

                MessageBox.Show(errorMessage);
                return null;
            }
        }

    }
}