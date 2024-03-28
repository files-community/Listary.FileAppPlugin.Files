using System;
using System.Threading.Tasks;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace Listary.FileAppPlugin.Files
{
    public class FilesWindow : IFileWindow, IDisposable
    {
        private readonly IFileAppPluginHost _host;
        private readonly UIA3Automation _automation;
        private readonly AutomationElement _Files;

        public IntPtr Handle { get; }

        public FilesWindow(IFileAppPluginHost host, IntPtr hWnd, UIA3Automation automation, AutomationElement Files)
        {
            _host = host;
            Handle = hWnd;
            _automation = automation;
            _Files = Files;
        }

        public void Dispose()
        {
            _automation.Dispose();
        }

        public async Task<IFileTab> GetCurrentTab()
        {
            return new FilesTab(_host, _Files);
        }
    }
}
