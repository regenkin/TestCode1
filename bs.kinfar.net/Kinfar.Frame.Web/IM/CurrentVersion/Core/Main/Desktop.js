var Desktop = (function(){

	var obj = {}; 
	
	var _dom = null;
	var _scrollDom = null;
	var _moveDiv = null;
	
	var _moveVar = null;
	
	function body_onmousemove()
	{
		if(_moveVar != null)
		{
			_moveVar.Callback("moving",_moveVar.Data, new IMCore.Event(arguments[0]));
		}
	}
	
	function body_onmouseup()
	{
		if(_moveVar != null)
		{
			_moveDiv.style.cursor = _moveVar.Cursor;
			_moveDiv.style.display = "none";
			_moveVar.Callback("end",_moveVar.Data, new IMCore.Event(arguments[0]));
			_moveVar = null;
		}
	}
	
	var m_All = [];
	
	function Find(win)
	{
		var i = 0;
		for(;i<m_All.length && m_All[i] != win; i++);
		return i < m_All.length ? i : -1;
	}
	
	obj.AddWindow = function(win)
	{
		_dom.appendChild(win.GetDom());
		win.GetDom().style.zIndex = m_All.length > 0 ? parseInt(m_All[m_All.length - 1].GetDom().style.zIndex)+1 : 1000;
		m_All.push(win);
	}
	
	obj.RemoveWindow = function(win)
	{
		var index = Find(win);
		if(index >= 0)
		{
			_dom.removeChild(win.GetDom());
			m_All.splice(index, 1);
		}
	}
	
	obj.GetDom = function()
	{
		return _dom;
	}
	
	
	obj.GetScrollDom = function()
	{
		return _scrollDom;
	}
	
	obj.SetTop = function(win)
	{
		if(m_All.length > 0 && win != m_All[m_All.length-1])
		{
			var index = Find(win);
			if(index == -1) return;
			m_All.splice(index, 1);
			var zIndex = parseInt(m_All[m_All.length - 1].GetDom().style.zIndex)+1;
			win.GetDom().style.zIndex = zIndex;
			m_All.push(win);
			
			if(zIndex > 10000)
			{
				for(var i = 0; i< m_All.length;i++) m_All[i].GetDom().style.zIndex = i + 1000;
			}
		}
	}
	
	obj.IsTop = function(win)
	{
		if(m_All.length <= 0) return false;
		return m_All[m_All.length-1] == win;
	}
	
	obj.EnterMove = function(callback, data, cursor)
	{
		_moveVar={
			Callback:callback,
			Data:data,
			PreCursor:_moveDiv.style.cursor
		};
		
		_moveDiv.style.cursor = cursor;
		_moveDiv.style.display = "block";
	}
	
	obj.GetWidth = function()
	{
		return Math.max(document.documentElement.clientWidth,200);
	}
	
	obj.GetHeight = function()
	{
		return Math.max(document.documentElement.clientHeight,200);
	}
	
	var _width = 0, _height = 0 ;
	
	function Resize(width, height)
	{
		for(var i in m_All)
		{
			try
			{
				var win = m_All[i];
				var anchorStyle = win.GetAnchorStyle();
				var needResize = false, needMove = false;
				var newLeft = win.GetLeft(), newTop = win.GetTop(), newWidth = win.GetWidth(), newHeight = win.GetHeight();
				
				if((anchorStyle & IMCore.WindowAnchorStyle.Left) == 0 && (anchorStyle & IMCore.WindowAnchorStyle.Right) == 0)
				{
				}
				else if((anchorStyle & IMCore.WindowAnchorStyle.Left) != 0 && (anchorStyle & IMCore.WindowAnchorStyle.Right) == 0)
				{
				}
				else if((anchorStyle & IMCore.WindowAnchorStyle.Left) == 0 && (anchorStyle & IMCore.WindowAnchorStyle.Right) != 0)
				{
					newLeft += (width - _width);
					needMove = true;
				}
				else if((anchorStyle & IMCore.WindowAnchorStyle.Left) != 0 && (anchorStyle & IMCore.WindowAnchorStyle.Right) != 0)
				{
					newWidth += (width - _width);
					needResize = true;
				}
				
				if((anchorStyle & IMCore.WindowAnchorStyle.Top) == 0 && (anchorStyle & IMCore.WindowAnchorStyle.Bottom) == 0)
				{
				}
				else if((anchorStyle & IMCore.WindowAnchorStyle.Top) != 0 && (anchorStyle & IMCore.WindowAnchorStyle.Bottom) == 0)
				{
				}
				else if((anchorStyle & IMCore.WindowAnchorStyle.Top) == 0 && (anchorStyle & IMCore.WindowAnchorStyle.Bottom) != 0)
				{
					newTop += (height - _height);
					needMove = true;
				}
				else if((anchorStyle & IMCore.WindowAnchorStyle.Top) != 0 && (anchorStyle & IMCore.WindowAnchorStyle.Bottom) != 0)
				{
					newHeight += (height - _height);
					needResize = true;
				}
				
				if(needMove) win.Move(newLeft, newTop);
				if(needResize) win.Resize(newWidth, newHeight);
			}
			catch(ex)
			{
			}
		}
		_moveDiv.style.width = width + "px";
		_moveDiv.style.height = height + "px";
		Taskbar.Move();
		Taskbar.Resize();
		_width = width;
		_height = height;
	}
	
	obj.Create = function()
	{
		_scrollDom = document.createElement("DIV");
		_scrollDom.style.width = "10px";
		_scrollDom.style.height = "10px";
		_scrollDom.style.position = "absolute";
		document.body.appendChild(_scrollDom);
		
		_dom = document.createElement("DIV");
		_dom.className = "desktop";
		_dom.style.width = "10px";
		_dom.style.height = "10px";
		_dom.style.left = "0px";
		_dom.style.top = "0px";
		_dom.innerHTML = "<div class='move_div'></div>";
		_moveDiv = _dom.firstChild;
		_moveDiv.style.display = "none";
		_scrollDom.appendChild(_dom);
		
		_moveDiv.style.width = document.documentElement.clientWidth + "px";
		_moveDiv.style.height = document.documentElement.clientHeight + "px";
		
		_scrollDom.style.left = document.documentElement.scrollLeft + "px";
		_scrollDom.style.top = document.documentElement.scrollTop + "px";
		
		IMCore.Utility.AttachEvent(
			window, "resize",
			function()
			{				
				Resize(document.documentElement.clientWidth,document.documentElement.clientHeight);
			}
		);
		
		IMCore.Utility.AttachEvent(
			window, "scroll",
			function()
			{
				_scrollDom.style.left = document.documentElement.scrollLeft + "px";
				_scrollDom.style.top = document.documentElement.scrollTop + "px";
			}
		);
		
		var enableSelTag = {
			"TEXTAREA":"",
			"INPUT":""
		};
		
		_dom.onselectstart=function(evt)
		{
			var e = new IMCore.Event(evt, window);
			
			if(e.GetTarget().tagName != undefined && enableSelTag[e.GetTarget().tagName.toUpperCase()] != undefined)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		IMCore.Utility.AttachEvent(document.body, "mousemove", body_onmousemove);
		IMCore.Utility.AttachEvent(document.body, "mouseup", body_onmouseup);
		
		Taskbar.Create();
		Resize(document.documentElement.clientWidth,document.documentElement.clientHeight);
	}
	
	return obj;
	
})();

var Taskbar = (function(){

	var obj = {};
	
	var _dom = null;
	var task_container = null;
	var _taskbtn=null;
	
	obj.Resize = function()
	{
		_dom.style.width = (Desktop.GetWidth() - 32) + "px";
		_dom.style.height = "28px";
		var width = Math.round((Desktop.GetWidth() - 300) / 156 - 0.5) * 156 + 4;
		if (width < 0) width = 0;
		task_container.style.width = width + "px";

		_taskbtn.style.display = task_container.scrollHeight > 24 ? "block" : "none";
	}
	
	obj.Move = function()
	{
		_dom.style.left = "16px";
		_dom.style.top = (Desktop.GetHeight() - 28) + "px";
	}
	
	obj.Create = function()
	{
		_dom = document.createElement("DIV");
		_dom.className = "taskbar";
		_dom.innerHTML = "<div class='icon'></div>" +
			"<div class='btn_friends'>我的好友</div><div class='task_container'></div><div class='udbtn'><div class='up_btn'></div><div class='down_btn'></div></div>";
		_dom.childNodes[1].onclick = function()
		{
			IMCore.Session.GetGlobal("SingletonForm").ShowFriendForm();
		}
		task_container = _dom.childNodes[2];
		_taskbtn = _dom.childNodes[3];
		
		IMCore.Utility.AttachButtonEvent(_taskbtn.childNodes[0], "up_btn","up_btn_hover","up_btn_press");
		IMCore.Utility.AttachButtonEvent(_taskbtn.childNodes[1], "down_btn","down_btn_hover","down_btn_press");
		
		_taskbtn.childNodes[0].onmousedown = function()
		{
			var scrollTop = task_container.scrollTop;
			if(scrollTop % 24 == 0) task_container.scrollTop = (Math.round(scrollTop / 24)  - 1)* 24;
			else task_container.scrollTop = Math.round(scrollTop / 24 - 0.5) * 24;
		}
		
		_taskbtn.childNodes[1].onmousedown = function()
		{
			var scrollTop = task_container.scrollTop;
			if(scrollTop % 24 == 0) task_container.scrollTop = (Math.round(scrollTop / 24)  + 1)* 24;
			else task_container.scrollTop =(Math.round(scrollTop / 24 - 0.5)  + 1) * 24;
		}
		
		obj.Resize();
		obj.Move();
		obj.Hide();
		Desktop.GetDom().appendChild(_dom);
	}
	
	obj.Show = function()
	{
		_dom.style.display="block";
		
		obj.Resize();
	}
	
	obj.Hide = function()
	{
		_dom.style.display="none";
	}
	
	function TaskbarItem(win)
	{
		var This=this;
		
		var title = win.GetTitle();
		
		var dom = document.createElement("DIV");
		
		dom.innerHTML = title;
		
		dom.className = "task";
		
		dom.onclick = function()
		{
			if(win.IsVisible())
			{
				if(win.IsTop()) win.Hide();
				else win.BringToTop();
			}
			else
			{
				win.Show();
			}
		}
		
		IMCore.Utility.AttachButtonEvent(dom, "task", "task_hover", "task_press");
	
		this.Shine=function(highlight)
		{
			if(highlight==undefined) highlight=false;
			var count=6;
			var interval=setInterval(
				function()
				{
					if(count>0)
					{
						switch(count)
						{
						case 1:
						case 3:
						case 5:
							dom.className="task";
							break;
						case 2:
						case 4:
						case 6:
							dom.className="task_highlight";
							break;
						}
						count--;
					}
					else
					{
						dom.className=(highlight?"task_highlight":"task");
						clearInterval(interval);
					}
				},
				200
			);
		}
		
		this.GetDom = function()
		{
			return dom;
		}
		
		this.GetWindow = function()
		{
			return win;
		}
		
		this.SetText = function(text)
		{
			dom.innerHTML = text;
		}
	}
	
	var _tasks = [];
	
	obj.AddTask = function(win)
	{
		var task = new TaskbarItem(win);
		
		_tasks.push(task);
		
		task_container.appendChild(task.GetDom());
		
		obj.Resize();
		
		return task;
	}
	
	obj.RemoveTask = function(win)
	{
		var i = 0;
		for(;i<_tasks.length && _tasks[i].GetWindow()!=win;i++);
		if(i<_tasks.length)
		{
			task_container.removeChild(_tasks[i].GetDom());
			_tasks.splice(i,1);
			obj.Resize();
		}
	}
	
	return obj;
	
})();