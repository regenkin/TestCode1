
IMCore.main = window;

function InitGlobal() {
	var WindowManagement = (function() {
		var m_All = [];

		var obj = {};

		obj.Add = function(win) {
			m_All.push(win);
		}

		obj.Remove = function(win) {
			var i = 0;
			for (; i < m_All.length && m_All[i] != win; i++);
			if (i < m_All.length) m_All.splice(i, 1);
		}

		obj.Notify = function(cmd, data) {
			for (var i in m_All) {
				try {
					m_All[i].OnNotify.Call(cmd, data);
				}
				catch (ex) {
				}
			}
		}

		return obj;
	})();

	IMCore.Session.RegisterGlobal("WindowManagement", WindowManagement);

	var ReponsesProcess = (function() {

		var obj = {};

		function Msg_Cort(m1, m2) {
			if (m1.CreatedTime > m2.CreatedTime) return 1;
			if (m1.CreatedTime < m2.CreatedTime) return -1;
			return 0;
		}

		var m_GlobalHandler = {
			"GLOBAL:IM_MESSAGE_NOTIFY": function(data) {
				if (data.Peer == "*") {
					data.Messages.sort(Msg_Cort);

					for (var i in data.Messages) {
						var msg = data.Messages[i];
						if (msg.Sender.Name.toLowerCase() == "administrator") {
							IMCore.Utility.ShowFloatForm(msg.Content, "json");
						}
						else {
							if (msg.Receiver.ID == __User.ID && msg.Sender.ID == __Peer.ID) {
								(function(msg) {
									__ChatForm.AddMessage(msg);
								})(msg);
							}
						}
					}
				}
				else {
					if (data.Message.Sender.Name.toLowerCase() == "administrator") {
						IMCore.Utility.ShowFloatForm(data.Message.Content, "json");
					}
					else {
						if (data.Message.Receiver.ID == __User.ID && data.Message.Sender.ID == __Peer.ID) {
							(function(msg) {
								__ChatForm.AddMessage(msg);
							})(data.Message);
						}
					}
				}
			},
			"UserStateChanged": function(data) {
				IMCore.Session.GetGlobal("WindowManagement").Notify("UserStateChanged", data);
			}
		}

		obj.Process = function(responseText) {
			var ret = IMCore.Utility.ParseJson(responseText);
			IMCore.Session.WriteLog(responseText);
			if (ret.IsSucceed) {
				var responses = ret.Responses;

				for (var i in responses) {
					var cr = responses[i];

					if (cr.CommandID == "GLOBAL:SessionReset") {
						IMCore.Session.ResponsesCache.InvokeErrorCallback("all", new IMCore.Exception("Server Error", "\u670D\u52A1\u5668\u9519\u8BEF!"));
						break;
					}
					if (m_GlobalHandler[cr.CommandID] != undefined) {
						m_GlobalHandler[cr.CommandID](cr.Data);
					}
					else {
						IMCore.Session.ResponsesCache.InvokeCallback(cr.CommandID, cr.Data);
					}
				}
			}
			else 
			{
				if (ret.Exception.Name == "UnauthorizedException") {
					if (IMCore.Session.ResponsesCache.IsRunning()) {
						IMCore.Session.ResponsesCache.Stop();
						IMCore.Utility.ShowFloatForm("{\"Type\":\"UnauthorizedException\"}", "json");
					}
				}
				else if (ret.Exception.Name == "IncompatibleException") {
					if (IMCore.Session.ResponsesCache.IsRunning()) {
						IMCore.Session.ResponsesCache.Stop();
						IMCore.Utility.ShowFloatForm("{\"Type\":\"IncompatibleException\"}", "json");
					}
				}
			}
		}

		return obj;
	})();

	IMCore.Session.RegisterGlobal("ReponsesProcess", ReponsesProcess);
}

String.format = function(fmt) {
	var params = arguments;
	var pattern = /{{|{[1-9][0-9]*}|\x7B0\x7D/g;
	return fmt.replace(
		pattern,
		function(p) {
			if (p == "{{") return "{";
			return params[parseInt(p.substr(1, p.length - 2), 10) + 1]
		}
	);
}

function SetClientMode(cm, win) {
	ClientMode = cm;
	CurrentWindow = win;

	var enableSelTag = {
		"TEXTAREA": "",
		"INPUT": ""
	};

	if (ClientMode) {
		IMCore.CreateWindow = function(config) {
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
			_config.AllowDrop = IMCore.Utility.IsNull(config.AllowDrop, false);

			if (config.Title == undefined) {
				_config.Title = {
					Height: 18,
					InnerHTML: ""
				};
			}
			else {
				_config.Title = {};
				_config.Title.Height = IMCore.Utility.IsNull(config.Title.Height, 18);
				_config.Title.InnerHTML = IMCore.Utility.IsNull(config.Title.InnerHTML, "");
			}

			_config.OnClose = IMCore.Utility.IsNull(config.OnClose, null);

			var win = window.external.CreateWindow(_config);
			IMCore.Session.GetGlobal("WindowManagement").Add(win);
			win.OnClosed.Attach(function(w) { IMCore.Session.GetGlobal("WindowManagement").Remove(w); });
			return win;
		}

		IMCore.CreateMenu = function(config) {
			return window.external.CreateMenu(config);
		}

		IMCore.Session = window.external.Session;
	}
	else 
	{
		IMCore.CreateWindow = function(config) {
			var win = new Window(config);
			IMCore.Session.GetGlobal("WindowManagement").Add(win);
			win.OnClosed.Attach(function(w) { IMCore.Session.GetGlobal("WindowManagement").Remove(w); });
			return win;
		}

		IMCore.CreateMenu = function(config) {
			var menu = new Menu(config);
			return menu;
		}

		IMCore.Session = new SessionConstructor();
	}

	InitGlobal();

	Desktop.Create();

	IMCore.Taskbar = Taskbar;
	IMCore.Desktop = Desktop;

	IMCore.OutputPanel = IMCore.CreateWindow(
		{
			Left: 200, Top: 150, Width: 600, Height: 450,
			Title: { InnerHTML: "\u8F93 \u51FA" },
			HasMinButton: false,
			OnClose: function(form) {
				form.Hide();
			}
		}
	);

	var data = IMCore.Utility.ParseJson(document.getElementById("data_json").value);

	__User = data.User;
	__Peer = data.Peer;

	IMCore.OutputPanel.MoveEx("", 10000, 10000, true);
	IMCore.OutputPanel.Show();

	IMCore.OutputPanel.Load(
		IMCore.GetPageUrl("Output.aspx"),
		function() {
			IMCore.OutputPanel.Hide();
			IMCore.Taskbar.Hide();
			IMCore.Session.InitService(
				data.User.Name,
				data.User,
				document.cookie,
				data.SessionID
			);

			__ChatForm = IMCore.CreateWindow(
				{
					Left: 0, Top: 0, Width: IMCore.Desktop.GetWidth(), Height: IMCore.Desktop.GetHeight(), MinWidth: 670, MinHeight: 500,
					BorderWidth: 0,
					Title: {
						InnerHTML: "\u804A\u5929\u7A97\u53E3",
						Height: 0
					},
					OnClose: function() {
						__ChatForm.Hide();
					},
					AnchorStyle: IMCore.WindowAnchorStyle.All
				}
			);

			var tag = {
				User: data.User,
				Peer: data.Peer
			};
			tag.OnFormCreated = new IMCore.Delegate();
			tag.OnFormCreated.Attach(
				function(panel) 
				{
					__ChatForm.AddMessage = function(msg) 
					{
						if (!__ChatForm.IsVisible()) __ChatForm.Show();
						panel.AddMessage(msg);
					}
				}
			);
			__ChatForm.SetTag(tag);

			__ChatForm.MoveEx("LEFT|TOP", 0, 0, true);
			__ChatForm.Resize(IMCore.Desktop.GetWidth(), IMCore.Desktop.GetHeight());
			__ChatForm.Show();

			__ChatForm.Load(
				IMCore.GetPageUrl(String.format("ChatForm.aspx?peer={0}&random={1}", data.Peer.Name, (new Date()).getTime()))
			);

			if (__StartServiceCallback != undefined && __StartServiceCallback != null) {
				__StartServiceCallback();
				__StartServiceCallback = null;
			}
		}
    );

	return true;
}

var __User, __Peer;
var __ChatForm = null;
var __StartServiceCallback = null;

function StartService(callback) {
	__StartServiceCallback = callback;
	SetClientMode(false, null);
}