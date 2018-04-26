
var ClientMode = false;
var CurrentWindow = null;

IMCore.WindowAnchorStyle = {};
IMCore.WindowAnchorStyle.Left = 1;
IMCore.WindowAnchorStyle.Right = 1 << 1;
IMCore.WindowAnchorStyle.Top = 1 << 2;
IMCore.WindowAnchorStyle.Bottom = 1 << 3;
IMCore.WindowAnchorStyle.All = 15;

IMCore.IWindow = function()
{

	this.ShowDialog = function(parent) { }

	this.Show = function() { }

	this.Hide = function() { }

	this.Minimum = function() { }

	this.Close = function() { }

	this.Move = function() { }

	this.MoveEx = function() { }

	this.Resize = function() { }

	this.GetTag = function() { }

	this.SetTag = function() { }

	this.GetTitle = function() { }

	this.SetTitle = function() { }

	this.IsTop = function() { }

	this.IsVisible = function() { }

	this.BringToTop = function() { }

	this.Load = function(url, callback) { }

	this.GetHtmlWindow = function() { }

	this.OnLoad = new IMCore.Delegate();

	this.OnClosed = new IMCore.Delegate();

	this.OnHidden = new IMCore.Delegate();
	
	this.OnNotify = new IMCore.Delegate();

	this.GetClientWidth = function() { };

	this.GetClientHeight = function() { };

	this.Notify = function() { }
}

IMCore.GetPageUrl = function(url)
{
	if (ClientMode || IMCore.Config.ServiceUrl == "") return url;
	if (IMCore.Config.ServiceUrl == "/") return "/" + url;
	return IMCore.Config.ServiceUrl + "/" + url;
}

IMCore.Utility.ShowWarning = function(text)
{
	if (ClientMode) window.external.ShowWarning(text);
	else alert(text);
}

IMCore.Utility.ShowError = function(text)
{
	if (ClientMode) window.external.ShowError(text);
	else alert(text);
}

IMCore.Utility.ShowFloatForm = function(text, type)
{
	var floatForm = IMCore.CreateWindow(
		{
			Left: 0, Top: 0, Width: 262, Height: 230,
			Title: { InnerHTML: "消息" },
			HasMinButton: false, HasMaxButton: false,
			Resizable: false,
			MinHeight: 80,
			ShowInTaskbar: false
		}
	);

	floatForm.MoveEx("RIGHT|BOTTOM", 10000, 10000, true);
	floatForm.Show();

	floatForm.Load(
		IMCore.GetPageUrl("FloatForm.aspx"),
		function()
		{
			floatForm.GetHtmlWindow().ShowMessage(text, type);
			floatForm.MoveEx("RIGHT|BOTTOM", 0, 0, true);
		}
	);
}

IMCore.SendCommand = function(callback, errorCallback, data, handler, isAysn)
{
	if (isAysn == undefined) isAysn = false;

	if (isAysn)
	{
		IMCore.Session.ResponsesCache.Start();
	}

	var postData = '<?xml version="1.0" encoding="utf-8" ?>\r\n';
	var id = IMCore.GenerateUniqueId() + "-" + Math.round(1000000000 + Math.random() * 100000000);
	postData += String.format(
		'<Command ID="{0}" SessionID=\"{1}" Handler=\"{3}\" IsAsyn=\"{4}\">{2}</Command>\r\n',
		id, IMCore.Session.GetSessionID(), IMCore.Utility.TransferCharForXML(data), handler, isAysn
	);

	if (isAysn)
	{
		if (ClientMode) IMCore.Session.ResponsesCache.NewCommandHandler(id, new CommandHandler(id, callback, errorCallback));
		else IMCore.Session.ResponsesCache.NewCommandHandler(id, callback, errorCallback);
	}

	var post_handler = {
		onsuccess: function(status, responseText)
		{
			try
			{
				var ret = IMCore.Utility.ParseJson(responseText);

				if (ret.IsSucceed)
				{
					if (!isAysn) callback(ret.Data);
				}
				else
				{
					if (isAysn) IMCore.Session.ResponsesCache.InvokeErrorCallback(id);
					else errorCallback(ret.Exception);
				}
			}
			finally
			{
			}
		},
		onerror: function(status, msg)
		{
			if (isAysn) IMCore.Session.ResponsesCache.InvokeErrorCallback(id, new IMCore.Exception("Server Error", msg == "" ? "服务器错误!" : msg));
			else errorCallback(new IMCore.Exception("Server Error", msg == "" ? "服务器错误!" : msg));
		},
		onabort: function()
		{
		}
	}

	IMCore.Post(IMCore.GetPageUrl("command.aspx"), postData, 'text/xml', -1, post_handler);
}

function CommandHandler(id, callback, errorCallback)
{
	this.Callback = function(data, type)
	{
		if (type == "json") data = IMCore.Utility.ParseJson(data);
		callback(data);
	}

	this.ErrorCallback = function(data, type)
	{
		if (type == "json") data = IMCore.Utility.ParseJson(data);
		errorCallback(data);
	}
}

function SessionConstructor()
{

	var obj = this;

	var m_UserName = null;
	var m_UserInfo = null;
	var m_SessionID = null;
	var m_Cookie = null;
	var GlobalHandler = {};

	obj.InitService = function(username,userinfo, cookie, sessionId)
	{
		m_UserName = username;
		m_UserInfo = userinfo;
		m_SessionID = sessionId;
		m_Cookie = cookie;

		IMCore.Session.ResponsesCache.Start();
	}

	obj.GetUserInfo = function()
	{
		return m_UserInfo;
	}

	obj.GetUserName = function()
	{
		return m_UserName;
	}

	obj.GetSessionID = function()
	{
		return m_SessionID;
	}

	obj.GetCookie = function()
	{
		return m_Cookie;
	}
	
	obj.ResetUserInfo = function(info)
	{
		m_UserInfo = info;
		IMCore.Session.GetGlobal("WindowManagement").Notify("UserInfoChanged", info);
	}
	
	obj.Reset = function()
	{
		m_UserName = null;
		m_UserInfo = null;
		m_SessionID = null;
		m_Cookie = null;
	}

	obj.WriteLog = function(log)
	{
		try
		{
			IMCore.OutputPanel.GetHtmlWindow().Write(log);
		}
		catch (ex)
		{
		}
	}

	var m_GlobalObject = {};

	obj.RegisterGlobal = function(key, value)
	{
		m_GlobalObject[key.toUpperCase()] = value;
	}

	obj.RemoveGlobal = function(key)
	{
		delete m_GlobalObject[key.toUpperCase()];
	}

	obj.GetGlobal = function(key)
	{
		return m_GlobalObject[key.toUpperCase()] == undefined ? null : m_GlobalObject[key.toUpperCase()];
	}

	obj.ResponsesCache = (function()
	{
		var CommandCallbackCache = {};

		var obj = {};

		var baseTime = new Date(2009, 0, 1);

		var m_Controler = null;
		var m_Stop = false;
		var m_IsRunning = false;

		obj.NewCommandHandler = function(id, callback, errorCallback)
		{
			var handler = new CommandHandler(id, callback, errorCallback)
			CommandCallbackCache[id] = handler;
		}

		obj.InvokeCallback = function(cmdid, data)
		{
			if (CommandCallbackCache[cmdid] != undefined)
			{
				CommandCallbackCache[cmdid].Callback(data);
				delete CommandCallbackCache[cmdid];
			}
		}

		obj.InvokeErrorCallback = function(cmdid, data)
		{
			if (cmdid == "all")
			{
				var callbacks = CommandCallbackCache;

				CommandCallbackCache = {};

				for (var key in callbacks)
				{
					try
					{
						callbacks[key].ErrorCallback(data);
					}
					catch (ex)
					{
					}
				}
			}
			else
			{
				if (CommandCallbackCache[cmdid] != undefined)
				{
					CommandCallbackCache[cmdid].ErrorCallback(data);
					delete CommandCallbackCache[cmdid];
				}
			}
		}
		
		obj.IsRunning = function()
		{
			return m_IsRunning;
		}

		obj.Start = function()
		{
			if (!m_IsRunning)
			{
				m_IsRunning = true;
				m_Stop = false;
				Send();
			}
		}

		obj.Stop = function()
		{
			m_Stop = true;
			if (m_Controler != null) m_Controler.Abort();
		}

		function Send()
		{
			if (m_Stop)
			{
				m_IsRunning = false;
				return;
			}

			var RequestID = IMCore.GenerateUniqueId();

			var data = String.format('RequestID={0}&SessionID={1}&ClientMode=false&ServerVersion={2}', RequestID, IMCore.Session.GetSessionID(), IMCore.Config.Version);

			var post_handler = {
				onsuccess: function(status, responseText)
				{
					try
					{
						IMCore.Session.GetGlobal("ReponsesProcess").Process(responseText);
					}
					catch (ex)
					{
					}
					setTimeout(Send, 10);
				},
				onerror: function(status, msg)
				{
					try
					{
						var ex = new IMCore.Exception("Server Error", msg == "" ? "服务器错误!" : msg);
						IMCore.Session.ResponsesCache.InvokeErrorCallback("all", ex);
						IMCore.Utility.ShowFloatForm(ex.toString(), "text");
					}
					catch (ex)
					{
					}
					setTimeout(Send, 5000);
				},
				onabort: function()
				{
					IMCore.Session.WriteLog("Abort");
					setTimeout(Send, 10);
				}
			};

			m_Controler = IMCore.Post(
				IMCore.GetPageUrl("response.aspx") + "?ID=" + RequestID,
				data, 'application/x-www-form-urlencoded', 2 * 60 * 1000,
				post_handler
			);
		}

		return obj;

	})();

	return obj;
}