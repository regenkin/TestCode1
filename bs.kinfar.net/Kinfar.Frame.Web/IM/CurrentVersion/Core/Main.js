try
{
	if (window.external.Version != undefined && window.external.Version != "1.1.0.11")
	{
		window.external.ShowError("客户端版本不兼容，请升级至1.1.0.11！");
		window.external.ExitApplication();
	}
}
catch(ex)
{
}

IMCore.main = window;

function InitGlobal()
{
	var FriendsInfoCache = (function()
	{

		var obj = {};
		var _friends = null;
		var _friendsIndexName = {};
		var _friendsIndexID = {};
		
		obj.GetFriendInfo = function(name)
		{
			return _friendsIndexName[name.toUpperCase()];
		}

		obj.GetFriends = function(callback, useCache)
		{
			try
			{
				if (useCache == undefined) useCache = true;

				if (_friends == null || !useCache)
				{
					var data = {
						Action: "GetFriends"
					};
					IMCore.SendCommand(
						function(ret)
						{
							if (_friends != null)
							{
								IMCore.Session.GetGlobal("WindowManagement").Notify("FriendInfoChanged", _friends);
							}
							_friends = ret.Friends;
							_friendsIndexName = {};
							_friendsIndexID = {};
							for (var i in _friends)
							{
								_friendsIndexName[_friends[i].Name.toUpperCase()] = _friends[i];
								_friendsIndexID[_friends[i].ID] = _friends[i];
							}
							callback(ret.Friends);
						},
						function(ex)
						{
							callback(null, ex);
						},
						IMCore.Utility.RenderJson(data), "Kinfar.Frame.IMWeb Common_CH", false
					);
				}
				else
				{
					callback(_friends);
				}
			}
			catch (ex)
			{
				callback(null, new IMCore.Exception(ex));
			}
		}

		obj.Refresh = function()
		{
			try
			{
				var data = {
					Action: "GetFriends"
				};
				IMCore.SendCommand(
					function(ret)
					{
						_friends = ret.Friends;
						_friendsIndexName = {};
						_friendsIndexID = {};
						for (var i in _friends)
						{
							_friendsIndexName[_friends[i].Name.toUpperCase()] = _friends[i];
							_friendsIndexID[_friends[i].ID] = _friends[i];
						}
						IMCore.Session.GetGlobal("WindowManagement").Notify("FriendInfoChanged", _friends);
					},
					function(ex)
					{
					},
					IMCore.Utility.RenderJson(data), "Kinfar.Frame.IMWeb Common_CH", false
				);
			}
			catch (ex)
			{
			}
		}
		
		obj.ResetState = function(user, state)
		{
			var id = 0
			if (typeof user == "number") id = user;
			else id = user.ID
			if(_friendsIndexID[id] != undefined) _friendsIndexID[id].State = state;
		}

		return obj;
	})();

	IMCore.Session.RegisterGlobal("FriendsInfoCache", FriendsInfoCache);

	var SingletonForm = (function()
	{

		var obj = {};

		var m_FriendForm = null;

		obj.ShowFriendForm = function()
		{
			if (m_FriendForm == null)
			{
				var config = {
					Left: 0,
					Top: 61,
					Width: 300,
					Height: 575,
					Title: {
						InnerHTML: String.format("欢迎你-{0}", IMCore.Session.GetUserInfo().Nickname)
					},
					MinWidth: 250,
					HasMaxButton: false,
					HasMinButton: false,
					OnClose: function(form)
					{
						form.Hide();
					},
					AnchorStyle: IMCore.WindowAnchorStyle.Right | IMCore.WindowAnchorStyle.Bottom
				};
				m_FriendForm = IMCore.CreateWindow(config);
				m_FriendForm.MoveEx("RIGHT|Bottom", -16, -32, true);
				m_FriendForm.Show();
				m_FriendForm.Load(IMCore.GetPageUrl("FriendForm.aspx"), null);
			}
			else
			{
				m_FriendForm.Show();
			}

			return m_FriendForm;
		}

		var m_AddFriendForm = null;

		obj.ShowAddFriendForm = function(friendName)
		{
			if (m_AddFriendForm == null)
			{
				var config = {
					Left: 0,
					Top: 0,
					Width: 400,
					Height: 300,
					Title: {
						InnerHTML: "添加好友"
					},
					Resizable: false,
					HasMaxButton: false,
					HasMinButton: false,
					OnClose: function(form)
					{
						form.Close();
						m_AddFriendForm = null;
					}
				}
				m_AddFriendForm = IMCore.CreateWindow(config);
				m_AddFriendForm.MoveEx('center', 0, -20, true);
				setTimeout(function() { m_AddFriendForm.Show(); }, 10);
				var url = IMCore.GetPageUrl("AddFriendForm.aspx?random=" + (new Date()).getTime());
				if (friendName != undefined) url += "&Name=" + friendName;
				m_AddFriendForm.Load(url, null);
			}
			else
			{
				setTimeout(function() { m_AddFriendForm.Show(); }, 10);
			}

			return m_AddFriendForm;
		}

		var m_FriendManagementForm = null;

		obj.ShowFriendManagementForm = function()
		{
			if (m_FriendManagementForm == null)
			{
				var config = {
					Left: 0,
					Top: 0,
					Width: 700,
					Height: 550,
					MinWidth: 700,
					MinHeight: 550,
					Title: {
						InnerHTML: "好友/群组管理"
					},
					Resizable: true,
					HasMaxButton: false,
					HasMinButton: true,
					AnchorStyle: IMCore.WindowAnchorStyle.Left | IMCore.WindowAnchorStyle.Bottom
				}
				m_FriendManagementForm = IMCore.CreateWindow(config);
				m_FriendManagementForm.OnClosed.Attach(function() { m_FriendManagementForm = null; });
				m_FriendManagementForm.MoveEx('MIDDLE|BOTTOM', 0, -30, true);
				setTimeout(function() { m_FriendManagementForm.Show(); }, 10);
				m_FriendManagementForm.Load(IMCore.GetPageUrl("Management/Form.aspx"), null);
			}
			else
			{
				setTimeout(function() { m_FriendManagementForm.Show(); }, 10);
			}

			return m_FriendManagementForm;
		}

		var m_MsgHistoryForm = null;

		obj.ShowMsgHistoryForm = function(peer)
		{
			if (m_MsgHistoryForm == null)
			{
				var config = {
					Left: 0,
					Top: 0,
					Width: 700,
					Height: 550,
					MinWidth: 700,
					MinHeight: 550,
					Title: {
						InnerHTML: "消息管理"
					},
					Resizable: true,
					HasMaxButton: ClientMode,
					HasMinButton: true,
					AnchorStyle: IMCore.WindowAnchorStyle.Left | IMCore.WindowAnchorStyle.Bottom
				}
				m_MsgHistoryForm = IMCore.CreateWindow(config);
				m_MsgHistoryForm.OnClosed.Attach(function() { m_MsgHistoryForm = null; });
				m_MsgHistoryForm.MoveEx('MIDDLE|BOTTOM', 0, -30, true);
				setTimeout(function() { m_MsgHistoryForm.Show(); }, 10);
				var url = IMCore.GetPageUrl("MsgHistory/Form.aspx");
				url += "?random=" + (new Date()).getTime();
				if (peer != undefined) url += String.format("&Peer={0}&Type={1}", peer.Name, peer.Type);
				m_MsgHistoryForm.Load(url, null);
			}
			else
			{
				setTimeout(function() { m_MsgHistoryForm.Show(); }, 10);
			}

			return m_MsgHistoryForm;
		}
		
		var m_UpdateSelfInfoForm = null;

		obj.ShowUpdateSelfInfoForm = function()
		{
			if (m_UpdateSelfInfoForm == null)
			{
				var config = {
					Left: 0,
					Top: 0,
					Width: 370,
					Height: 500,
					MinWidth: 370,
					MinHeight: 500,
					Title: {
						InnerHTML: "修改个人资料"
					},
					Resizable: false,
					HasMaxButton: false,
					HasMinButton: true,
					AnchorStyle: IMCore.WindowAnchorStyle.Left | IMCore.WindowAnchorStyle.Top
				}
				m_UpdateSelfInfoForm = IMCore.CreateWindow(config);
				m_UpdateSelfInfoForm.OnClosed.Attach(function() { m_UpdateSelfInfoForm = null; });
				m_UpdateSelfInfoForm.MoveEx('CENTER', 0, 0, true);
				setTimeout(function() { m_UpdateSelfInfoForm.Show(); }, 10);
				var url = IMCore.GetPageUrl("Management/UpdateSelfInfo.aspx");
				url += "?random=" + (new Date()).getTime();
				m_UpdateSelfInfoForm.Load(url, null);
			}
			else
			{
				setTimeout(function() { m_UpdateSelfInfoForm.Show(); }, 10);
			}

			return m_UpdateSelfInfoForm;
		}

		return obj;
	})();

	IMCore.Session.RegisterGlobal("SingletonForm", SingletonForm);

	function ChatFormTag(cf)
	{
		var This = this;

		var m_Msgs = [];

		var m_IsCreated = false;

		This.OnFormCreated = new IMCore.Delegate();

		This.OnFormCreated.Attach(
			function()
			{
				m_IsCreated = true;
				for (var i in m_Msgs)
				{
					cf.GetHtmlWindow().AddMessage(m_Msgs[i]);
				}
			}
		);

		This.AddMessage = function(msg)
		{
			if (m_IsCreated) cf.GetHtmlWindow().AddMessage(msg);
			else m_Msgs.push(msg);
		}
	}

	var ChatService = (function()
	{

		var obj = {};

		var m_ChatForms = {};

		obj.Open = function(peer, slient)
		{
			if (slient == undefined) slient = false;
			var key = peer.toUpperCase();

			if (m_ChatForms[key] == undefined)
			{
				var form = IMCore.CreateWindow(
					{
						Left: 0, Top: 0, Width: 670, Height: 500, MinWidth: 670, MinHeight: 500,
						Title: { InnerHTML: "聊天窗口" },
						AnchorStyle: IMCore.WindowAnchorStyle.Left | IMCore.WindowAnchorStyle.Bottom
					}
				);
				form.SetTag(new ChatFormTag(form));
				form.OnClosed.Attach(function() { delete m_ChatForms[key]; });
				if (slient)
				{
					form.MoveEx("", 10000, 10000, true);
					form.Show();
				}
				else
				{
					if (!ClientMode)
					{
						var r = Math.round(Math.random() * 40);
						form.MoveEx("LEFT|BOTTOM", 16 + r, -32 - r, true);
					}
					else
					{
						form.MoveEx("CENTER|BOTTOM", 0, -30, true);
					}
					form.Show();
				}
				m_ChatForms[key] = form;
				form.Load(
					IMCore.GetPageUrl(String.format("ChatForm.aspx?peer={0}&random={1}", peer, (new Date()).getTime())),
					function()
					{
						if (slient)
						{
							form.Minimum();
							if (!ClientMode)
							{
								var r = Math.round(Math.random() * 40);
								form.MoveEx("LEFT|BOTTOM", 16 + r, -32 - r, true);
							}
							else
							{
								form.MoveEx("CENTER|BOTTOM", 0, -30, true);
							}
						}
					}
				);
			}
			else
			{
				if (!slient) m_ChatForms[key].Show();
			}
			return m_ChatForms[key];
		}

		return obj;

	})();

	IMCore.Session.RegisterGlobal("ChatService", ChatService);

	var WindowManagement = (function()
	{
		var m_All = [];
		
		var obj = {};
		
		obj.Add = function(win)
		{
			m_All.push(win);
		}
		
		obj.Remove = function(win)
		{
			var i = 0;
			for(;i<m_All.length && m_All[i] != win;i++);
			if(i<m_All.length) m_All.splice(i,1);
		}
		
		obj.Notify = function(cmd, data)
		{
			for(var i in m_All)
			{
				try
				{
					m_All[i].OnNotify.Call(cmd,data);
				}
				catch(ex)
				{
				}
			}
		}
		
		return obj;
	})();
	
	IMCore.Session.RegisterGlobal("WindowManagement",WindowManagement);

	var ReponsesProcess = (function()
	{

		var obj = {};

		function Msg_Cort(m1, m2)
		{
			if (m1.CreatedTime > m2.CreatedTime) return 1;
			if (m1.CreatedTime < m2.CreatedTime) return -1;
			return 0;
		}

		var m_GlobalHandler = {
			"GLOBAL:IM_MESSAGE_NOTIFY": function(data)
			{
				if (data.Peer == "*")
				{
					data.Messages.sort(Msg_Cort);

					for (var i in data.Messages)
					{
						var msg = data.Messages[i];
						if (msg.Sender.Name.toLowerCase() == "administrator")
						{
							IMCore.Utility.ShowFloatForm(msg.Content, "json");
						}
						else
						{
							if (msg.Receiver.Type == 0)
							{
								(function(msg)
								{
									var form = IMCore.Session.GetGlobal("ChatService").Open(msg.Sender.Name, true);
									form.GetTag().AddMessage(msg);
								})(msg);
							}
							else
							{
								(function(msg)
								{
									var form = IMCore.Session.GetGlobal("ChatService").Open(msg.Receiver.Name, true);
									form.GetTag().AddMessage(msg);
								})(msg);
							}
						}
					}
				}
				else
				{
					if (data.Message.Sender.Name.toLowerCase() == "administrator")
					{
						IMCore.Utility.ShowFloatForm(data.Message.Content, "json");
					}
					else
					{
						if (data.Message.Receiver.Type == 0)
						{
							(function(msg)
							{
								var form = IMCore.Session.GetGlobal("ChatService").Open(msg.Sender.Name, true);
								form.GetTag().AddMessage(msg);
							})(data.Message);
						}
						else
						{
							(function(msg)
							{
								var form = IMCore.Session.GetGlobal("ChatService").Open(msg.Receiver.Name, true);
								form.GetTag().AddMessage(msg);
							})(data.Message);
						}
					}
				}
			},
			"UserStateChanged":function(data)
			{
				IMCore.Session.GetGlobal("FriendsInfoCache").ResetState(data.User, data.State);
				IMCore.Session.GetGlobal("WindowManagement").Notify("UserStateChanged", data);
			},
			"GLOBAL:REFRESH_FIRENDS": function(data)
			{
				IMCore.Session.GetGlobal("FriendsInfoCache").Refresh();
			}
		}

		obj.Process = function(responseText)
		{
			var ret = IMCore.Utility.ParseJson(responseText);
			IMCore.Session.WriteLog(responseText);
			if (ret.IsSucceed)
			{
				var responses = ret.Responses;

				for (var i in responses)
				{
					var cr = responses[i];

					if (cr.CommandID == "GLOBAL:SessionReset")
					{
						IMCore.Session.ResponsesCache.InvokeErrorCallback("all", new IMCore.Exception("Server Error", "服务器错误!"));
						break;
					}
					if (m_GlobalHandler[cr.CommandID] != undefined)
					{
						m_GlobalHandler[cr.CommandID](cr.Data);
					}
					else
					{
						IMCore.Session.ResponsesCache.InvokeCallback(cr.CommandID, cr.Data);
					}
				}
			}
			else
			{
				if(ret.Exception.Name == "UnauthorizedException")
				{
					if(IMCore.Session.ResponsesCache.IsRunning())
					{
						IMCore.Session.ResponsesCache.Stop();
						IMCore.Utility.ShowFloatForm("{\"Type\":\"UnauthorizedException\"}", "json");
					}
				}
				else if(ret.Exception.Name == "IncompatibleException")
				{
					if(IMCore.Session.ResponsesCache.IsRunning())
					{
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

function SetClientMode(cm, win)
{
	ClientMode = cm;

	var enableSelTag = {
		"TEXTAREA": "",
		"INPUT": ""
	};

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
			win.OnClosed.Attach(function(w) { IMCore.Session.GetGlobal("WindowManagement").Remove(w); });
			return win;
		}

		IMCore.Session = window.external.Session;
	}
	else
	{
		IMCore.CreateWindow = function(config)
		{
			var win = new Window(config);
			IMCore.Session.GetGlobal("WindowManagement").Add(win);
			win.OnClosed.Attach(function(w) { IMCore.Session.GetGlobal("WindowManagement").Remove(w); });
			return win;
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
			Title: { InnerHTML: "输 出" },
			HasMinButton: false,
			OnClose: function(form)
			{
				form.Hide();
			}
		}
	);
	
	if (window.init != undefined) window.init();
			
	return true;
}

String.format = function(fmt)
{
	var params = arguments;
	var pattern = /{{|{[1-9][0-9]*}|\x7B0\x7D/g;
	return fmt.replace(
		pattern,
		function(p)
		{
			if (p == "{{") return "{";
			return params[parseInt(p.substr(1, p.length - 2), 10) + 1]
		}
	);
}

var __LoginForm = null;
var __StartServiceCallback = null;

function StartService(callback)
{
	__StartServiceCallback = callback;
	if (__LoginForm == null && (IMCore.Session == undefined || IMCore.Session.GetUserName() == null))
	{
		SetClientMode(false, null);
	}
	else
	{
		if (__StartServiceCallback != undefined && __StartServiceCallback != null) 
		{
			__StartServiceCallback();
			__StartServiceCallback = null;
		}
	}
}

IMCore.Login = function(auto)
{
	if(__LoginForm != null) return;
	
	__LoginForm = IMCore.CreateWindow(
		{
			Left: 0, Top: 0, Width: 612, Height: 430,
			HasMinButton: true, HasMaxButton: false,
			Resizable: false,
			Title: { InnerHTML: "用户登录" }
		}
	);

	__LoginForm.OnClosed.Attach(
		function(f)
		{
			__LoginForm = null;
			if (IMCore.Session.GetUserName() != null && IMCore.Session.GetUserName() != "")
			{
				if (__StartServiceCallback != undefined && __StartServiceCallback != null) 
				{
					__StartServiceCallback();
					__StartServiceCallback = null;
				}
			}
			if ((IMCore.Session.GetUserName() == null || IMCore.Session.GetUserName() == "") && ClientMode)
			{
				window.external.ExitApplication();
			}
		}
	);

	__LoginForm.MoveEx('center', 0, -20, true);
	__LoginForm.Hide();
	
	var url = String.format("login.aspx?auto={0}",auto ? "true" : "false");

    if (IMCore.Session.GetUserName() != null && IMCore.Session.GetUserName() != "")
    {
    	url += "&name=" + IMCore.Session.GetUserName();
    }
    
	IMCore.Session.Reset();

	__LoginForm.Load(
		IMCore.GetPageUrl(url),
		function()
		{
		}
	);
}

function init()
{
	IMCore.Login(!ClientMode);
}