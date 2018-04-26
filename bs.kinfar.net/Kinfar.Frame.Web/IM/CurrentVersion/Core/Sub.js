
IMCore.main = (parent == window ? window.external.Desktop : parent);

IMCore.Taskbar = IMCore.main.IMCore.Taskbar;
IMCore.Desktop = IMCore.main.IMCore.Desktop;
IMCore.Login = IMCore.main.IMCore.Login;

function SetClientMode(cm, win)
{
	ClientMode = cm;
	CurrentWindow = win;

	document.oncontextmenu = function() { return false; }

	if (IMCore.GetBrowser() == "IE")
	{
		try
		{
			document.execCommand("BackgroundImageCache", false, true);
		}
		catch (ex)
		{
		}
	}

	var enableSelTag = {
		"TEXTAREA": "",
		"INPUT": ""
	};

	document.onselectstart = function(evt)
	{
		var e = new IMCore.Event(evt, window);
		return (e.GetTarget().tagName != undefined && enableSelTag[e.GetTarget().tagName.toUpperCase()] != undefined)
	}

	IMCore.Utility.AttachEvent(
		document, "keydown",
		function()
		{
			if (event.keyCode == 116 || (event.ctrlKey && event.keyCode == 82))
			{
				event.keyCode = 0;
				event.returnValue = false;
				return false;
			}
			if (event.keyCode == 70 && event.ctrlKey && !event.altKey && !event.shiftKey)
			{
				event.keyCode = 0;
				event.returnValue = false;
				return false;
			}
		}
	);

	if (!ClientMode)
	{
		IMCore.Utility.AttachEvent(
			document, "mousedown",
			function()
			{
				CurrentWindow.BringToTop();
			}
		);
	}

	if (ClientMode)
	{
		IMCore.CreateWindow = function(config)
		{
			var _config = {};
			_config.Left = IMCore.Utility.IsNull(config.Left, 100);
			_config.Top = IMCore.Utility.IsNull(config.Top, 100);
			_config.Width = IMCore.Utility.IsNull(config.Width, 400);
			_config.Height = IMCore.Utility.IsNull(config.Height, 300);
			_config.MinWidth = IMCore.Utility.IsNull(config.MinWidth, Math.min(_config.Width, 400));
			_config.MinHeight = IMCore.Utility.IsNull(config.MinHeight, Math.min(_config.Height, 300));
			_config.HasMinButton = IMCore.Utility.IsNull(config.HasMinButton, true);
			_config.HasMaxButton = IMCore.Utility.IsNull(config.HasMaxButton, true);
			_config.Resizable = IMCore.Utility.IsNull(config.Resizable, true);
			_config.Css = IMCore.Utility.IsNull(config.Css, "window");
			_config.BorderWidth = IMCore.Utility.IsNull(config.BorderWidth, 6);
			_config.ShowInTaskbar = IMCore.Utility.IsNull(config.ShowInTaskbar, _config.HasMinButton);
			_config.Tag = IMCore.Utility.IsNull(config.Tag, null);

			if (config.Title == undefined)
			{
				_config.Title = {
					Height: 18,
					InnerHTML: ""
				};
			}
			else
			{
				_config.Title = {};
				_config.Title.Height = IMCore.Utility.IsNull(config.Title.Height, 18);
				_config.Title.InnerHTML = IMCore.Utility.IsNull(config.Title.InnerHTML, "");
			}

			_config.OnClose = IMCore.Utility.IsNull(config.OnClose, null);

			var win = window.external.CreateWindow(_config);
			IMCore.Session.GetGlobal("WindowManagement").Add(win);
			win.OnClosed.Attach(function(w){IMCore.Session.GetGlobal("WindowManagement").Remove(w);});
			return win;
		}

		IMCore.Session = window.external.Session;
		IMCore.OutputPanel = window.external.Desktop.IMCore.OutputPanel;
	}
	else
	{
		IMCore.CreateWindow = parent.IMCore.CreateWindow;
		IMCore.Session = parent.IMCore.Session;
		IMCore.OutputPanel = parent.IMCore.OutputPanel;
	}

	IMCore.Initialize(
		{},
		function()
		{
			if (window.init != undefined) window.init();
		}
	);
	return true;
}