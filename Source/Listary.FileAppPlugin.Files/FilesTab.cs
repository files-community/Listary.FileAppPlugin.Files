using FlaUI.Core.AutomationElements;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Listary.FileAppPlugin.Files
{
	public class FilesTab : IFileTab, IGetFolder, IOpenFolder
	{
		private readonly IFileAppPluginHost _host;
		private readonly TextBox _currentPathGet;
		private readonly TextBox _currentPathSet;

		public FilesTab(IFileAppPluginHost host, AutomationElement Files)
		{
			_host = host;

			// Find window content to reduce the scope
			var _windowContent = Files.FindFirstChild(cf => cf.ByClassName("Microsoft.UI.Content.DesktopChildSiteBridge"));
			var _paneContent = _windowContent.FindFirstChild(cf => cf.ByClassName("InputSiteWindowClass"));

			_currentPathGet = _paneContent.FindFirstChild(cf => cf.ByAutomationId("CurrentPathGet"))?.AsTextBox();
			if (_currentPathGet == null)
			{
				_host.Logger.LogError("Failed to find CurrentPathGet");
				return;
			}

			_currentPathSet = _paneContent.FindFirstChild(cf => cf.ByAutomationId("CurrentPathSet"))?.AsTextBox();
			if (_currentPathSet == null)
			{
				_host.Logger.LogError("Failed to find CurrentPathSet");
				return;
			}
		}

		public async Task<string> GetCurrentFolder()
		{
			try
			{
				return _currentPathGet.Text;
			}
			catch (Exception e)
			{
				_host.Logger.LogError($"Failed to get current folder: {e}");
				return null;
			}
		}

		public async Task<bool> OpenFolder(string path)
		{
			try
			{
				_currentPathSet.Text = path;
				return true;
			}
			catch (Exception e)
			{
				_host.Logger.LogError($"Failed to get current folder: {e}");
				return false;
			}
		}
	}
}
