
var Desktop = null;
var Taskbar = null;
var BaseUrl = (IMCore.Config.ServiceUrl == "/" ? "" : IMCore.Config.ServiceUrl) + "/" + IMCore.Config.ResPath;

(function(){

var m_Modules = {};
var m_ModulesArray = [];

var m_ModuleCtorFormat =
	"\r\n" +
	"var Module=this;\r\n" +
	"Module.DirectoryName='{0}';\r\n" +
	"Module.FileName='{1}';\r\n" +
	"Module.GetResourceUrl = function(relativePath)\r\n" +
	"{\r\n" +
	"	return IMCore.GetUrl(Module.DirectoryName+'/'+relativePath);\r\n" +
	"};\r\n" +
	"\r\n" +
	"{2}\r\n" +
	"Module.Initialize=(typeof(init)=='undefined'?null:init);\r\n" +
	"Module.Dispose=(typeof(dispose)=='undefined'?null:dispose);\r\n";

function CreateModule(path, ctor)
{
	var module = {};
	module.DirectoryName = IMCore.Path.GetDirectoryName(path);
	module.FileName = path;
	module.GetResourceUrl = function(relativePath)
	{
		return IMCore.GetUrl(module.DirectoryName + '/' + relativePath);
	}
	ctor.call(module, module);

	return module;
}

function CreateHttpRequest()
{
	var request = null;
	if (window.XMLHttpRequest)
	{
		request = new XMLHttpRequest();
	}
	else if (window.ActiveXObject)
	{
		request = new ActiveXObject("Microsoft.XMLHttp");
	}
	return request;
}

function GetModuleText(callback, errorCallback, xmlUrl)
{
	var request = CreateHttpRequest();
	if (request)
	{
		var url = xmlUrl;
		request.open("GET", url, true);
		request.onreadystatechange = function()
		{
			if (request.readyState == 4)
			{
				try
				{
					switch (request.status)
					{
						case 200:
							callback(request.responseText);
							break;
						default:
							if (errorCallback) errorCallback(new IMCore.Exception("Server Error", request.statusText));
					}
				}
				catch (ex)
				{
					errorCallback(new IMCore.Exception(ex.name, ex.message));
				}
				finally
				{
					request = null;
				}
			}
		}
		request.send("");
	}
}

function Link(rel, href, type)
{
	var e = document.createElement("link");
	e.rel = rel
	e.type = type
	e.href = href;
	var hs = document.getElementsByTagName("head");
	if (hs.length > 0) hs[0].appendChild(e);
	return e;
}

function GetUrl(path)
{
	return encodeURI(BaseUrl + "/" + path);
}

function RegisterModule(path, ctor)
{
	var fullpath = "Module/" + path;
	m_Modules[fullpath.toUpperCase()] = CreateModule(fullpath, ctor);
}

function LoadModules(completeCallback, errorCallback, paths, index)
{
	function fail(ex)
	{
		errorCallback(ex);
	}

	if (index == undefined) index = 0;

	var path = "Module/" + paths[index];

	var moduleId = path.toUpperCase();
	if (m_Modules[moduleId] == null)
	{
		GetModuleText(load, fail, GetUrl(path));
	}
	else
	{
		loadComplete();
	}

	function load(text)
	{
		var moudle_ctor = new Function(
				String.format(m_ModuleCtorFormat, IMCore.Path.GetDirectoryName(path), path, text, false)
			);
		var moudle = new moudle_ctor();
		if (moudle.Initialize != null)
			moudle.Initialize(complete, fail);
		else
			complete();
		function complete()
		{
			if (m_Modules[moduleId] == null)
			{
				m_Modules[moduleId] = moudle;
				m_ModulesArray.push(moudle);
			}
			loadComplete();
		}
	}

	function loadComplete()
	{
		if (index == paths.length - 1)
			completeCallback();
		else
			LoadModules(completeCallback, errorCallback, paths, index + 1);
	}
}

function GetModule(path)
{
	var moduleId = "MODULE/" + path.toUpperCase();
	return m_Modules[moduleId];
}

function Initialize(config, callback)
{
	if (callback != undefined) callback();
}

function InitUI(callback)
{
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

	document.oncontextmenu = function() { return false; }

	IMCore.LoadModules(
			function()
			{
				Desktop = IMCore.GetModule("Desktop.js").Desktop;
				Taskbar = IMCore.GetModule("Desktop.js").Taskbar;

				if (callback != undefined) callback();
			},
			alert,
			["Controls.js", "Desktop.js"]
		);
}

function Invoke(completeCallback, errorCallback, objs, asynMethod, continueIfError, completeOneCallback)
{
	function callOne(index)
	{
		var obj = objs[index];
		if (obj[asynMethod] != null)
		{
			try
			{
				obj[asynMethod].call(obj, complete, function(ex) { error(ex, obj); });
			}
			catch (ex)
			{
				error(ex, obj);
			}
		}
		else
			complete();
		function complete()
		{
			if (completeOneCallback != undefined) completeOneCallback(obj);
			if (index == objs.length - 1) completeCallback(); else callOne(index + 1);
		}
		function error(msg, o)
		{
			errorCallback(msg, o);
			if (!continueIfError || index == objs.length - 1) completeCallback(); else callOne(index + 1);
		}
	}
	if (objs.length > 0) callOne(0); else completeCallback();
}

function Call(completeCallback, errorCallback, funcs, caller, continueIfError, completeOneCallback)
{
	function callOne(index)
	{
		var func = funcs[index];
		if (func != null)
		{
			try
			{
				func.call(caller, complete, error);
			}
			catch (ex)
			{
				error(ex);
			}
		}
		else
			complete();
		function complete()
		{
			if (completeOneCallback != undefined) completeOneCallback(obj);
			if (index == funcs.length - 1) completeCallback(); else callOne(index + 1);
		}
		function error(msg)
		{
			errorCallback(msg);
			if (!continueIfError || index == funcs.length - 1) completeCallback(); else callOne(index + 1);
		}
	}
	if (objs.length > 0) callOne(0); else completeCallback();
}

function Dispose(completeCallback, errorCallback)
{
	function fail(ex, m)
	{
		errorCallback(ex);
	}

	m_ModulesArray.reverse();
	Invoke(completeCallback, fail, m_ModulesArray, "Dispose", true, completeOneCallback);
	function completeOneCallback(m)
	{
	}
}

IMCore.GetUrl = GetUrl;
IMCore.Link = Link;
IMCore.Initialize = Initialize;
IMCore.InitUI = InitUI;
IMCore.LoadModules = LoadModules;
IMCore.GetModule = GetModule;
IMCore.Dispose = Dispose;
IMCore.Invoke = Invoke;
IMCore.Call = Call;

})();