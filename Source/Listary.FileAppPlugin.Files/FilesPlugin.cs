using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.Extensions.Logging;

namespace Listary.FileAppPlugin.Files
{
    public class FilesPlugin : IFileAppPlugin
    {
        private IFileAppPluginHost _host;

        public bool IsOpenedFolderProvider => true;
        
        public bool IsQuickSwitchTarget => false;
        
        public bool IsSharedAcrossApplications => false;

        public SearchBarType SearchBarType => SearchBarType.Floating;
        
        public async Task<bool> Initialize(IFileAppPluginHost host)
        {
            _host = host;
            return true;
        }

        public IFileWindow BindFileWindow(IntPtr hWnd)
        {
            // Is it from Files?
            string processName = Path.GetFileName(Win32Utils.GetProcessPathFromHwnd(hWnd));
            if (processName == "Files.exe")
            {
                // Is it Files's file window?
                try
                {
                    var automation = new UIA3Automation();
                    AutomationElement Files = automation.FromHandle(hWnd);
                    if (Files.Name.Contains("- Files"))
                    {
                        return new FilesWindow(_host, hWnd, automation, Files);
                    }
                }
                catch (TimeoutException e)
                {
                    _host.Logger.LogWarning($"UIA timeout: {e}");
                }
                catch (Exception e)
                {
                    _host.Logger.LogError($"Failed to bind window: {e}");
                }
            }
            return null;
        }
    }
}
